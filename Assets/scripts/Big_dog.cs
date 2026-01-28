// Umożliwia korzystanie z IEnumerator i coroutine (np. WaitForSeconds)
using System.Collections;

// Importuje podstawowe klasy Unity (MonoBehaviour, Transform, Vector3 itd.)
using UnityEngine;

// Import statyczny (tu akurat NIEUŻYWANY – można go spokojnie usunąć)
using static UnityEngine.EventSystems.EventTrigger;

// Definicja klasy Big_dog
// Klasa dziedziczy po Entity, czyli ma wspólne cechy z innymi bytami (np. życie, śmierć)
public class Big_dog : Entity
{
    // Prędkość poruszania się psa
    // SerializeField → widoczna w Inspectorze mimo że private
    [SerializeField] private float speed = 3.5f;

    // Liczba żyć / punktów życia psa
    [SerializeField] private int lives = 2;

    // Kierunek poruszania się (prawo / lewo)
    private Vector3 dir;

    // Flaga blokująca zbyt częste obracanie się
    private bool canFlip = true;

    // Punkt, z którego wykonywany jest atak (np. pysk psa)
    [SerializeField] private Transform attackPos;

    // Zasięg ataku psa
    [SerializeField] private float attackRange = 0.7f;

    // Warstwa, na której znajduje się gracz (Hero)
    [SerializeField] private LayerMask playerLayer;

    // Czy pies może aktualnie zaatakować
    [SerializeField] private bool isRecharged = true;

    // Czas przerwy pomiędzy atakami
    [SerializeField] private float attackCooldown = 1f;

    // Metoda Start wywoływana raz przy starcie obiektu
    private void Start()
    {
        // Ustawiamy początkowy kierunek ruchu w prawo
        dir = Vector3.right;
    }

    // Update wywoływany co klatkę
    private void Update()
    {
        // Ruch psa
        Move();

        // Próba wykonania ataku
        TryAttack();
    }

    // Metoda odpowiedzialna za poruszanie się psa
    private void Move()
    {
        // Punkt przed psem, w którym sprawdzamy czy coś stoi na drodze
        Vector3 checkPos = transform.position + dir * 2f;

        // Sprawdzamy wszystkie kolidery w okręgu o promieniu 1f
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPos, 1f);

        // Flaga informująca czy pies coś napotkał
        bool hitSomething = false;

        // Przechodzimy przez wszystkie znalezione kolidery
        foreach (var col in colliders)
        {
            // Jeśli obiekt NIE jest samym psem
            if (col.gameObject != gameObject)
            {
                // To znaczy, że pies na coś trafił
                hitSomething = true;
                break;
            }
        }

        // Jeśli pies coś napotkał i może się obrócić
        if (hitSomething && canFlip)
        {
            // Zmieniamy kierunek ruchu na przeciwny
            dir *= -1f;

            // Obracamy sprite
            Flip();
        }

        // Przesuwamy psa w danym kierunku
        // Time.deltaTime → ruch niezależny od FPS
        transform.position += dir * speed * Time.deltaTime;
    }

    // Metoda obracająca psa
    private void Flip()
    {
        // Pobieramy aktualną skalę obiektu
        Vector3 scale = transform.localScale;

        // Odwracamy obiekt w osi X
        scale.x *= -1;

        // Zapisujemy nową skalę
        transform.localScale = scale;

        // Blokujemy możliwość kolejnego obrotu
        canFlip = false;

        // Uruchamiamy coroutine odblokowującą obrót po czasie
        StartCoroutine(FlipCooldown());
    }

    // Coroutine blokująca obracanie na 0.5 sekundy
    private IEnumerator FlipCooldown()
    {
        // Czekamy pół sekundy
        yield return new WaitForSeconds(0.5f);

        // Odblokowujemy obracanie
        canFlip = true;
    }

    // Metoda wywoływana przy fizycznej kolizji 2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Sprawdzamy czy pies zderzył się z bohaterem
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            // Bohater otrzymuje obrażenia
            Hero.Instance.GetDamage();
        }
    }

    // Nadpisanie metody GetDamage z klasy Entity
    public override void GetDamage()
    {
        // Wypisanie informacji do konsoli
        Debug.Log("DOG HIT!");

        // Zmniejszamy liczbę żyć psa
        lives--;

        // Jeśli pies nie ma już żyć
        if (lives <= 0)
            // Wywołujemy śmierć (metoda z Entity)
            Die();
    }

    // Próba wykonania ataku
    private void TryAttack()
    {
        // Jeśli atak jest na cooldownie albo nie ma punktu ataku → kończymy
        if (!isRecharged || attackPos == null) return;

        // Szukamy gracza w zasięgu ataku
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPos.position,
            attackRange,
            playerLayer
        );

        // Sprawdzamy wszystkie trafione obiekty
        foreach (var hit in hits)
        {
            // Próbujemy pobrać komponent Hero
            Hero h = hit.GetComponent<Hero>();

            // Jeśli to faktycznie bohater
            if (h != null)
                // Zadajemy mu obrażenia
                h.GetDamage();
        }

        // Jeśli ktoś został trafiony
        if (hits.Length > 0)
            // Uruchamiamy cooldown ataku
            StartCoroutine(AttackCooldown());
    }

    // Coroutine odpowiedzialna za cooldown ataku
    private IEnumerator AttackCooldown()
    {
        // Blokujemy możliwość ataku
        isRecharged = false;

        // Czekamy określony czas
        yield return new WaitForSeconds(attackCooldown);

        // Odblokowujemy atak
        isRecharged = true;
    }

#if UNITY_EDITOR
    // Gizmos widoczne tylko w edytorze Unity

    private void OnDrawGizmos()
    {
        // Nie rysujemy jeśli gra nie jest uruchomiona
        if (!Application.isPlaying) return;

        // Kolor gizma
        Gizmos.color = Color.red;

        // Mała kula pokazująca punkt sprawdzania przeszkód
        Gizmos.DrawSphere(transform.position + dir * 0.7f, 0.1f);
    }

    private void OnDrawGizmosSelected()
    {
        // Jeśli nie ma punktu ataku, nic nie rysujemy
        if (attackPos == null) return;

        // Kolor gizma
        Gizmos.color = Color.yellow;

        // Okrąg pokazujący zasięg ataku
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }
#endif
}
