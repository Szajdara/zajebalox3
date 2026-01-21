using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dogenemy : entity
{
    [SerializeField] private int lives = 3;

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject == hero.Instance.gameObject)
        {
            hero.Instance.GetDamage();
            lives--;
            Debug.Log("Lives left: " + lives);
        }
        if (lives < 1)
            Die();

    }
}
