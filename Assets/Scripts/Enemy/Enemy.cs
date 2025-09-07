using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float vida = 6f;

    public void RecibirDaño(float daño)
    {
        vida -= daño;

        if (vida <= 0)
        {
            Morir();
        }
    }

    private void Morir()
    {
        Debug.Log(gameObject.name + " ha sido destruido.");
        Destroy(gameObject);
    }
}