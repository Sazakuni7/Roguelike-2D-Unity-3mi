using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public static event Action<float> OnEnemyDestroyed; // Evento estático con experiencia
    [SerializeField] private float vida = 6f;
    [SerializeField] private float experienciaOtorgada = 10f; // Experiencia que otorga este enemigo

    private bool haMuerto = false;

    public void RecibirDaño(float daño)
    {
        vida -= daño;
        if (vida <= 0) Morir();
    }

    private void Morir()
    {
        if (haMuerto) return; // Evitar múltiples ejecuciones
        haMuerto = true;

        Debug.Log($"Enemigo destruido. Experiencia otorgada: {experienciaOtorgada}");
        OnEnemyDestroyed?.Invoke(experienciaOtorgada);
        Destroy(gameObject);
    }
}