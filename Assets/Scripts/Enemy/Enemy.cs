using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public static event Action OnEnemyDestroyed; // Evento estático
    [SerializeField] private float vida = 6f;

    public void RecibirDaño(float daño)
    {
        vida -= daño;
        if (vida <= 0) Morir();
    }

    private void Morir()
    {
        OnEnemyDestroyed?.Invoke(); // Notificar que el enemigo fue destruido
        Destroy(gameObject);
    }
}