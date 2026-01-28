// Umożliwia korzystanie z kolekcji i coroutine (tu akurat nieużywane)
using System.Collections;

// Umożliwia korzystanie z kolekcji generycznych (List, Dictionary – też nieużywane tutaj)
using System.Collections.Generic;

// Importuje klasy Unity (MonoBehaviour, GameObject, Transform itp.)
using UnityEngine;

// Importuje system zarządzania scenami w Unity (ładuj, przełączaj sceny)
using UnityEngine.SceneManagement;

// Definicja klasy Scene_mod
// Dziedziczy po MonoBehaviour, więc można ją przypiąć do obiektu w scenie
public class Scene_mod : MonoBehaviour
{
    // Publiczna metoda, którą można przypisać np. do przycisku w UI
    public void LoadGame()
    {
        // Ładuje scenę o nazwie "SampleScene"
        // Wymaga, żeby scena była dodana do Build Settings
        SceneManager.LoadScene("SampleScene");
    }
}
