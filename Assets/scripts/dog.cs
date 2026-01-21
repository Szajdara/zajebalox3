using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dog : entity
{
    private float speed = 3.5f;
    private Vector3 dir;
    private SpriteRenderer sprite;

    private void Start()
    {
        dir = transform.right;
    }

    private void Update()
    {
        Move();
    }
    private void Move()
    {
        Collider2D[] colliedrs = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * dir.x * 0.7f, 0.1f);

        //if (colliedrs.Length > 0) dir *= -1f;
        //transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.right, speed * Time.deltaTime);

        transform.position += dir * speed * Time.deltaTime;


        if (colliedrs.Length > 1)
        {
            dir *= -1f;
            Flip();
        }


    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == hero.Instance.gameObject)
        {
            hero.Instance.GetDamage();
        }
    }
}
