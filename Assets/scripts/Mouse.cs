// Importuje podstawowe klasy Unity (MonoBehaviour, GameObject itp.)
using System.Collections;

// Importuje kolekcje generyczne (List, Dictionary – tu nieużywane)
using System.Collections.Generic;

// Importuje klasy Unity
using UnityEngine;

// Definicja klasy Mouse
// Dziedziczy po MonoBehaviour, więc można ją przypiąć do obiektu w scenie
public class Mouse : MonoBehaviour
{
    // Metoda wywoływana, gdy inny Collider2D wchodzi w trigger tego obiektu
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Sprawdzamy, czy obiekt, który wszedł w trigger, ma tag "Player"
        if (other.CompareTag("Player"))
        {
            // Jeśli tak, dodajemy 1 mysz do licznika w GameMana (singleton)
            GameMana.Instance.AddMouse();

            // Niszczy obiekt myszy (bo gracz ją "zbiera")
            Destroy(gameObject);
        }
    }
}
