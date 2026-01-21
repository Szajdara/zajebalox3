using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class hero : entity
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private int lives = 5;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private bool isGrounded = false;

    [SerializeField] public bool isAttacking = false;
    [SerializeField] public bool isRecharged = true;

    public Transform attackPos;
    public float attackRange;
    public LayerMask enemy;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public static hero Instance { get; set; }
    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        Instance = this;
        isRecharged = true;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {

        if (isGrounded) State = States.idle;

        if (Input.GetButton("Horizontal"))
        {
            Run();
        }
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            Jump();
        }
        if (isRecharged && Input.GetButtonDown("Fire1")) // or any button you want for attack
        {
            Attack();
        }
    }
    private void Run()
    {
        if (isGrounded) State = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        if (dir.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (dir.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + Vector3.down * 0.1f, 0.4f);
        isGrounded = colliders.Length > 1;

        if (!isGrounded) State = States.jump;
    }

    public override void GetDamage() 
    {
        lives -= 1;
        Debug.Log(lives);
    }

    private void Attack()
    {
        if (isGrounded && isRecharged)
        {

            State = States.atak;
            isAttacking = true;
            isRecharged = false;

            StartCoroutine(AttackAnimation());
            StartCoroutine(AttackCooldown());
        }
    }

    private void OnAttack()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackPos.position, attackRange, enemy);

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].GetComponent<entity>().GetDamage();
        }
    }
    private IEnumerator AttackAnimation()
    {
        // Wait a short time for the attack to "hit"
        yield return new WaitForSeconds(1.5f); // adjust to match your animation timing
        OnAttack(); // deal damage to enemies in range

        // Wait until the animation ends
        isAttacking = false;
        if (isGrounded)
            State = States.idle;
    }

    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(1.5f); // attack cooldown in seconds
        isRecharged = true;
    }

    public enum States
    {
        idle,
        run,
        jump,
        atak
    }
}
