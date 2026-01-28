// Importuje podstawowe klasy Unity (MonoBehaviour, GameObject, Transform itp.)
using System.Collections;

// Importuje kolekcje generyczne (List, Dictionary – tu nieużywane)
using System.Collections.Generic;

// Importuje klasy Unity
using UnityEngine;

// Definicja klasy Music
// Dziedziczy po MonoBehaviour, więc można ją przypiąć do obiektu w scenie
public class Music : MonoBehaviour
{
    // Statyczna zmienna przechowująca referencję do jedynego obiektu Music
    private static Music instance;

    // Awake jest wywoływane zanim Start, przy wczytaniu obiektu
    void Awake()
    {
        // Sprawdza, czy instancja już istnieje
        if (instance != null)
        {
            // Jeśli tak, niszczymy ten obiekt, żeby nie było duplikatów
            Destroy(gameObject);

            // Kończymy dalsze wykonywanie Awake
            return;
        }

        // Jeśli instancja jeszcze nie istnieje, przypisujemy ten obiekt
        instance = this;

        // Nie niszcz tego obiektu przy zmianie sceny
        // Dzięki temu muzyka leci dalej, nawet gdy zmieniamy poziomy
        DontDestroyOnLoad(gameObject);
    }
}
