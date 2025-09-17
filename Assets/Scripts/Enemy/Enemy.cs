using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public static event Action OnEnemyDestroyed; // Evento est�tico
    [SerializeField] private float vida = 6f;

    public void RecibirDa�o(float da�o)
    {
        vida -= da�o;
        if (vida <= 0) Morir();
    }

    private void Morir()
    {
        OnEnemyDestroyed?.Invoke(); // Notificar que el enemigo fue destruido
        Destroy(gameObject);
    }
}