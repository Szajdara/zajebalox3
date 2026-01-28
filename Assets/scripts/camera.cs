// Importuje podstawowe klasy Unity (MonoBehaviour, GameObject, Transform itp.)
using System.Collections;

// Importuje kolekcje generyczne (List, Dictionary – tu nieużywane)
using System.Collections.Generic;

// Importuje klasy Unity
using UnityEngine;

// Definicja klasy Camera (kamera śledząca gracza)
// Dziedziczy po MonoBehaviour, więc można ją przypiąć do obiektu w scenie (GameObject kamery)
public class Camera : MonoBehaviour
{
    // Odwołanie do transformacji gracza, którego kamera będzie śledzić
    [SerializeField] private Transform player;

    // Zmienna pomocnicza do przechowywania pozycji kamery
    private Vector3 pos;

    // Awake wywoływane zanim Start, przy wczytaniu obiektu
    private void Awake()
    {
        // Jeśli w Inspectorze nie przypisano gracza
        if (!player)
            // Znajdź obiekt Hero w scenie i przypisz jego transform
            player = FindObjectOfType<Hero>().transform;
    }

    // Update wywoływany co klatkę
    private void Update()
    {
        // Pobieramy aktualną pozycję gracza
        pos = player.position;

        // Ustawiamy z, żeby kamera była zawsze trochę odsunięta od świata (ważne dla 2D)
        pos.z = -10f;

        // Płynne przesuwanie kamery w kierunku gracza
        // Vector3.Lerp → interpolacja liniowa między starą pozycją a nową
        // Time.deltaTime sprawia, że ruch jest niezależny od FPS
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }
}
