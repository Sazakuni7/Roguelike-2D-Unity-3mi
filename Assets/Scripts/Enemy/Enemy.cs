using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public static event Action<float> OnEnemyDestroyed; // Evento est�tico con experiencia
    [SerializeField] private float vida = 6f;
    [SerializeField] private float experienciaOtorgada = 10f; // Experiencia que otorga este enemigo

    private bool haMuerto = false;

    public void RecibirDa�o(float da�o)
    {
        vida -= da�o;
        if (vida <= 0) Morir();
    }

    private void Morir()
    {
        if (haMuerto) return; // Evitar m�ltiples ejecuciones
        haMuerto = true;

        Debug.Log($"Enemigo destruido. Experiencia otorgada: {experienciaOtorgada}");
        OnEnemyDestroyed?.Invoke(experienciaOtorgada);
        Destroy(gameObject);
    }
}