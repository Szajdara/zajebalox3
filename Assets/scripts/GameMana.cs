// Importuje podstawowe klasy Unity (MonoBehaviour, GameObject itp.)
using System.Collections;

// Importuje kolekcje generyczne (List, Dictionary – tu nieużywane)
using System.Collections.Generic;

// Importuje klasy Unity
using UnityEngine;

// Importuje system zarządzania scenami w Unity (ładuj, przełączaj sceny)
using UnityEngine.SceneManagement;

// Importuje UI (Text, Button itd.)
using UnityEngine.UI;

// Definicja klasy GameMana
// Dziedziczy po MonoBehaviour, więc można ją przypiąć do obiektu w scenie
public class GameMana : MonoBehaviour
{
    // Singleton → statyczna referencja do jedynego obiektu GameMana
    public static GameMana Instance;

    // Liczba złapanych myszy
    public int miceCount = 0;

    // Ile myszy trzeba złapać, aby wygrać
    public int winLimit = 10;

    // Odwołanie do tekstu w UI, który pokazuje liczbę myszy
    public Text miceText; // można też użyć TMP_Text (TextMeshPro)

    // Awake jest wywoływane zanim Start, przy wczytaniu obiektu
    private void Awake()
    {
        // Ustawiamy instancję singletona
        Instance = this;

        // Aktualizujemy UI przy starcie gry
        UpdateUI();
    }

    // Metoda wywoływana, gdy gracz złapie mysz
    public void AddMouse()
    {
        // Zwiększamy licznik myszy
        miceCount++;

        // Odświeżamy UI
        UpdateUI();

        // Jeśli gracz złapał już wystarczająco dużo myszy
        if (miceCount >= winLimit)
        {
            // Wywołujemy zwycięstwo
            Win();
        }
    }

    // Metoda aktualizująca UI
    private void UpdateUI()
    {
        // Zmieniamy tekst w UI, np. "Myszki: 3 / 10"
        miceText.text = "Myszki: " + miceCount + " / " + winLimit;
    }

    // Metoda wywoływana przy zwycięstwie
    private void Win()
    {
        // Uruchamiamy coroutine, żeby odczekać chwilę przed zmianą sceny
        StartCoroutine(WinCoroutine());
    }

    // Coroutine odczekująca chwilę przed przejściem do sceny zwycięstwa
    private IEnumerator WinCoroutine()
    {
        // Czekamy 1 sekundę
        yield return new WaitForSeconds(1f);

        // Ładujemy scenę o nazwie "Win"
        SceneManager.LoadScene("Win");
    }
}
