// Importuje klasy systemowe Unity (coroutines)
using System;

// Importuje kolekcje (tu nieużywane, można usunąć)
using System.Collections;
using System.Collections.Generic;

// Importuje podstawowe klasy Unity
using UnityEngine;

// Importuje klasy UI (Text, Image itp.)
using UnityEngine.UI;

// Importuje zarządzanie scenami (ładuj sceny po śmierci)
using UnityEngine.SceneManagement;

// Definicja klasy Hero (gracz)
// Dziedziczy po Entity, więc ma metody GetDamage() i Die()
public class Hero : Entity
{
    // --- Parametry ruchu i życia ---
    [SerializeField] private float speed = 3f; // prędkość poruszania się
    [SerializeField] private int health;       // aktualne zdrowie
    [SerializeField] private int lives;        // maksymalne życie
    [SerializeField] private float jumpForce = 7f; // siła skoku
    [SerializeField] private bool isGrounded = false; // czy gracz stoi na ziemi

    // --- UI serc ---
    [SerializeField] private Image[] hearts; // serca w UI
    [SerializeField] private Sprite aliveHeart; // pełne serce
    [SerializeField] private Sprite deadHeart;  // puste serce

    // --- Atak ---
    [SerializeField] public bool isAttacking = false; // czy aktualnie atakuje
    [SerializeField] public bool isRecharged = true;  // cooldown ataku
    public Transform attackPos;     // punkt ataku
    public float attackRange = 0.5f; // zasięg ataku
    public LayerMask enemy;         // warstwa wrogów

    // --- Śmierć ---
    [SerializeField] private GameObject deathSprite; // opcjonalny sprite śmierci
    [SerializeField] private float deathY = -20f;    // jeśli gracz spadnie niżej → śmierć

    // --- Dźwięki ---
    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource getDamage;
    [SerializeField] private AudioSource AttackSound;

    // --- Komponenty ---
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    // Flaga, czy gracz jest martwy
    private bool isDead = false;

    // Singleton, aby Hero był łatwo dostępny w innych skryptach
    public static Hero Instance { get; set; }

    // Wewnętrzna właściwość dla animacji
    private States State
    {
        get { return (States)anim.GetInteger("state"); } // pobiera stan z animatora
        set { anim.SetInteger("state", (int)value); }    // ustawia stan w animatorze
    }

    // --- Awake ---
    private void Awake()
    {
        lives = 5;        // ustawiamy maksymalne życie
        health = lives;   // zdrowie na start = maks
        rb = GetComponent<Rigidbody2D>(); // pobieramy Rigidbody
        anim = GetComponent<Animator>();  // pobieramy Animator
        sprite = GetComponentInChildren<SpriteRenderer>(); // Sprite gracza
        Instance = this;  // przypisujemy singleton
        isRecharged = true; // atak gotowy od startu
    }

    // --- FixedUpdate ---
    private void FixedUpdate()
    {
        CheckGround(); // sprawdzamy, czy gracz stoi na ziemi
    }

    // --- Update ---
    private void Update()
    {
        // Jeśli gracz spadnie poniżej deathY → ginie
        if (!isDead && transform.position.y < deathY)
        {
            Die();
        }

        // Jeśli gracz stoi i nie atakuje → idle
        if (!isDead && isGrounded && !isAttacking)
            State = States.idle;

        // Ruch w lewo/prawo
        if (Input.GetButton("Horizontal") && !isDead)
            Run();

        // Skok
        if (isGrounded && Input.GetButtonDown("Jump") && !isDead)
            Jump();

        // Atak
        if (isRecharged && Input.GetKeyDown(KeyCode.X))
        {
            Attack();
        }

        // Ograniczenie health do max
        if (health > lives)
            health = lives;

        // --- Aktualizacja serc w UI ---
        for (int i = 0; i < hearts.Length; i++)
        {
            // pełne lub puste serce
            hearts[i].sprite = (i < health) ? aliveHeart : deadHeart;

            // włączamy tylko tyle serc ile lives
            hearts[i].enabled = (i < lives);
        }
    }

    // --- Run ---
    private void Run()
    {
        if (isGrounded && !isAttacking)
            State = States.run;

        // kierunek ruchu
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        // przesuwamy gracza
        transform.position = Vector3.MoveTowards(
            transform.position,
            transform.position + dir,
            speed * Time.deltaTime
        );

        // odwracamy sprite w zależności od kierunku
        if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // --- Jump ---
    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse); // siła skoku
        jumpSound.Play(); // dźwięk skoku
    }

    // --- Sprawdzanie, czy gracz stoi na ziemi ---
    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            transform.position + Vector3.down * 0.1f, // punkt tuż pod graczem
            0.4f // promień sprawdzania
        );
        isGrounded = colliders.Length > 1; // jeśli są jakieś kolidery → stoi na ziemi

        if (!isGrounded)
            State = States.jump; // zmiana animacji na jump
    }

    // --- GetDamage ---
    public override void GetDamage()
    {
        if (isDead) return; // jeśli gracz martwy → nic nie robimy

        getDamage.Play(); // odtwarzamy dźwięk obrażeń
        health--;        // zmniejszamy zdrowie

        if (health <= 0)
        {
            health = 0;
            Die();       // śmierć
        }

        Debug.Log("Health: " + health); // wypisanie zdrowia w konsoli
    }

    // --- Atak ---
    private void Attack()
    {
        if (!isGrounded || !isRecharged) return; // nie atakujemy w powietrzu lub podczas cooldown

        State = States.atak;
        isAttacking = true;
        isRecharged = false;
        AttackSound.Play(); // odtwarzamy dźwięk ataku

        StartCoroutine(AttackAnimation()); // animacja ataku
        StartCoroutine(AttackCooldown());  // cooldown ataku
    }

    private IEnumerator AttackAnimation()
    {
        OnAttack(); // sprawdzamy trafienia wroga
        yield return new WaitForSeconds(0.3f); // czas trwania ataku
        isAttacking = false; // koniec animacji ataku
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(0.8f); // czas odnowienia ataku
        isRecharged = true;
    }

    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(
            attackPos.position, attackRange, enemy // zasięg i warstwa wrogów
        );

        for (int i = 0; i < colliders.Length; i++)
        {
            Entity e = colliders[i].GetComponent<Entity>();
            if (e != null)
                e.GetDamage(); // zadajemy obrażenia każdemu w zasięgu
        }
    }

    // --- Rysowanie gizmos dla attackPos ---
    private void OnDrawGizmosSelected()
    {
        if (attackPos == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    // --- Die ---
    public override void Die()
    {
        isDead = true;
        State = States.die;

        rb.velocity = Vector2.zero;          // zatrzymujemy ruch
        rb.bodyType = RigidbodyType2D.Static; // blokujemy Rigidbody

        StartCoroutine(LoadGameOver()); // po 3s ładuje scenę gameOver
    }

    private IEnumerator LoadGameOver()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("gameOver"); // scena po śmierci
    }

    // --- Stany gracza ---
    public enum States
    {
        idle,  // stojący
        run,   // biegnący
        jump,  // skaczący
        atak,  // atakujący
        die    // martwy
    }
}
