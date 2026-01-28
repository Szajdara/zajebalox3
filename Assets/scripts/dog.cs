// Importuje klasy Unity (MonoBehaviour, GameObject itp.)
using System.Collections;

// Importuje kolekcje generyczne (List, Dictionary – tu nieużywane)
using UnityEngine;

// Import statyczny EventTrigger – w tym skrypcie NIEUŻYWANE, można usunąć
using static UnityEngine.EventSystems.EventTrigger;

// Definicja klasy Dog (wróg – pies)
// Dziedziczy po Entity, więc ma podstawowe cechy wroga (życie, śmierć itp.)
public class Dog : Entity
{
    // Prędkość poruszania się psa
    [SerializeField] private float speed = 3.5f;

    // Liczba żyć psa
    [SerializeField] private int lives = 2;

    // Kierunek ruchu psa (Vector3.right = w prawo, Vector3.left = w lewo)
    private Vector3 dir;

    // Flaga blokująca obracanie psa zbyt często
    private bool canFlip = true;

    // Start wywoływany przy starcie obiektu
    private void Start()
    {
        // Ustawiamy początkowy kierunek ruchu na prawo
        dir = Vector3.right;
    }

    // Update wywoływany co klatkę
    private void Update()
    {
        // Wywołujemy metodę poruszania się psa
        Move();
    }

    // Metoda odpowiadająca za ruch psa
    private void Move()
    {
        // Punkt przed psem, w którym sprawdzamy czy stoi przeszkoda
        Vector3 checkPos = transform.position + dir * 0.7f;

        // Sprawdzamy wszystkie kolidery w małym okręgu (promień 0.1) w tym punkcie
        Collider2D[] colliders = Physics2D.OverlapCircleAll(checkPos, 0.1f);

        // Flaga, czy pies napotkał przeszkodę
        bool hitSomething = false;

        // Przechodzimy przez wszystkie znalezione kolidery
        foreach (var col in colliders)
        {
            // Ignorujemy samego siebie
            if (col.gameObject != gameObject)
            {
                // Napotkano przeszkodę
                hitSomething = true;
                break;
            }
        }

        // Jeśli coś stoi na drodze i pies może się obrócić
        if (hitSomething && canFlip)
        {
            // Zmieniamy kierunek ruchu na przeciwny
            dir *= -1f;

            // Obracamy sprite psa
            Flip();
        }

        // Przesuwamy psa w danym kierunku z uwzględnieniem czasu (FPS niezależny ruch)
        transform.position += dir * speed * Time.deltaTime;
    }

    // Obraca sprite psa w osi X
    private void Flip()
    {
        // Pobieramy aktualną skalę obiektu
        Vector3 scale = transform.localScale;

        // Odwracamy go w osi X
        scale.x *= -1;

        // Zapisujemy nową skalę
        transform.localScale = scale;

        // Blokujemy możliwość kolejnego obrotu
        canFlip = false;

        // Uruchamiamy coroutine, która odblokuje obrót po 0.5 sekundy
        StartCoroutine(FlipCooldown());
    }

    // Coroutine odblokowująca obrót po czasie
    private IEnumerator FlipCooldown()
    {
        // Czekamy 0.5 sekundy
        yield return new WaitForSeconds(0.5f);

        // Odblokowujemy możliwość obracania
        canFlip = true;
    }

    // Metoda wywoływana przy kolizji 2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Jeśli pies zderzył się z bohaterem
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            // Zadajemy bohaterowi obrażenia
            Hero.Instance.GetDamage();
        }
    }

    // Nadpisanie metody GetDamage z Entity
    public override void GetDamage()
    {
        // Wypisuje informację w konsoli, że pies został trafiony
        Debug.Log("DOG HIT!");

        // Zmniejszamy liczbę żyć psa
        lives--;

        // Jeśli pies nie ma już życia
        if (lives <= 0)
            // Wywołujemy metodę Die z Entity, która niszczy obiekt
            Die();
    }

#if UNITY_EDITOR
    // Metoda wywoływana w edytorze Unity do rysowania gizmos
    private void OnDrawGizmos()
    {
        // Jeśli gra nie jest uruchomiona, nic nie rysujemy
        if (!Application.isPlaying) return;

        // Kolor gizmos
        Gizmos.color = Color.red;

        // Rysujemy małą kulkę pokazującą punkt sprawdzania przeszkód
        Gizmos.DrawSphere(transform.position + dir * 0.7f, 0.1f);
    }
#endif
}
