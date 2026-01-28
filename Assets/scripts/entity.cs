// Importuje podstawowe klasy Unity (MonoBehaviour, GameObject itp.)
using System.Collections;

// Importuje kolekcje generyczne (List, Dictionary – tu nieużywane)
using System.Collections.Generic;

// Importuje klasy Unity
using UnityEngine;

// Definicja klasy Entity
// Bazowa klasa dla wszystkich bytów w grze (np. wrogów, psa, bohatera)
public class Entity : MonoBehaviour
{
    // Metoda wirtualna GetDamage
    // 'virtual' oznacza, że można ją nadpisać w klasach dziedziczących (np. Dog, Dogenemy)
    public virtual void GetDamage()
    {
        // Pusta implementacja – konkretne klasy określają, co robi GetDamage
    }

    // Metoda wirtualna Die
    // 'virtual' oznacza, że można ją nadpisać w klasach dziedziczących
    public virtual void Die()
    {
        // Niszczy obiekt w grze (np. wróg ginie)
        Destroy(this.gameObject);
    }
}
