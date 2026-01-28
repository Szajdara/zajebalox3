// Importuje podstawowe klasy do pracy z kolekcjami (np. listy, tablice dynamiczne)
using System.Collections;

// Importuje generyczne kolekcje (np. List<T>, Dictionary<TKey, TValue>)
using System.Collections.Generic;

// Importuje wszystkie klasy Unity (MonoBehaviour, GameObject, Collision2D itd.)
using UnityEngine;

// Definicja klasy Trug
// Klasa dziedziczy po MonoBehaviour, więc może być przypięta do obiektu w Unity
public class Trug : MonoBehaviour
{
    // Metoda wywoływana AUTOMATYCZNIE przez Unity
    // Uruchamia się w momencie, gdy ten obiekt zderzy się z innym obiektem 2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Sprawdzamy, czy obiekt, z którym nastąpiło zderzenie,
        // jest dokładnie tym obiektem, który należy do bohatera
        if (collision.gameObject == Hero.Instance.gameObject)
        {
            // Jeśli tak, wywołujemy metodę GetDamage() u bohatera,
            // czyli zadajemy mu obrażenia
            Hero.Instance.GetDamage();
        }
    }
}
