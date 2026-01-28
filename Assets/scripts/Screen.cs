// Umożliwia korzystanie z kolekcji i coroutine (tu akurat nieużywane, ale często dodawane domyślnie)
using System.Collections;

// Umożliwia korzystanie z generycznych kolekcji (List, Dictionary itp. – tutaj też nieużywane)
using System.Collections.Generic;

// Importuje klasy Unity (MonoBehaviour, Screen, FullScreenMode itd.)
using UnityEngine;

// Klasa Screen_mod
// Dziedziczy po MonoBehaviour, więc może być przypięta do obiektu w Unity
public class Screen_mod : MonoBehaviour
{
    // Metoda Start wywoływana raz, gdy scena się uruchamia
    void Start()
    {
        // Ustawia tryb pełnego ekranu jako "FullScreenWindow"
        // Czyli pełny ekran bez zmiany trybu monitora (borderless fullscreen)
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;

        // Włącza tryb pełnoekranowy
        Screen.fullScreen = true;

        // Ustawia rozdzielczość ekranu na 1920x1080
        // Trzeci parametr 'true' oznacza, że gra ma działać w trybie pełnoekranowym
        Screen.SetResolution(1920, 1080, true);
    }
}
