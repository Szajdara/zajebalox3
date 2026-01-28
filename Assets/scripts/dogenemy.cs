// Importuje podstawowe klasy Unity (MonoBehaviour, GameObject itp.)
using System.Collections;

// Importuje kolekcje generyczne (List, Dictionary – tu nieużywane)
using System.Collections.Generic;

// Importuje klasy Unity
using UnityEngine;

// Definicja klasy Dogenemy
// Dziedziczy po Entity, więc ma podstawowe cechy wroga (np. życie, śmierć)
public class Dogenemy : Entity
{
    // Liczba żyć psa-wroga
    public int lives = 1;

    // Metoda wywoływana przy kolizji 2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Sprawdza, czy kolidujący obiekt to bohater
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            // Jeśli tak, bohater dostaje obrażenia
            Hero.Instance.GetDamage();
        }
    }

    // Nadpisuje metodę GetDamage z klasy Entity
    public override void GetDamage()
    {
        // Wypisuje w konsoli informację, że pies został trafiony
        Debug.Log("DOG HIT!!!");

        // Zmniejsza liczbę żyć psa
        lives--;

        // Wypisuje aktualne HP psa w konsoli
        Debug.Log("Dog HP: " + lives);

        // Jeśli pies nie ma już życia
        if (lives <= 0)
            // Wywołuje metodę Die z klasy Entity, aby zniszczyć obiekt lub wykonać animację śmierci
            Die();
    }
}
