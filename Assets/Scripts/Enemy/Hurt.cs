using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herir : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] private int puntos = 5;
    [SerializeField] private float tiempoEntreAtaques = 1.0f; // cooldown entre ataques
    private float tiempoUltimoAtaque = 0f;

    private Animator animator;

    private void Awake()
    {
        // Obtener referencia al Animator
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time - tiempoUltimoAtaque >= tiempoEntreAtaques)
            {
                // Reinicia y dispara trigger para asegurar que se reproduzca siempre
                animator.ResetTrigger("Attack");
                animator.SetTrigger("Attack");

                tiempoUltimoAtaque = Time.time;

                // Aplica daño una sola vez
                Jugador jugador = collision.GetComponent<Jugador>();
                if (jugador != null)
                {
                    jugador.ModificarVida(-puntos);
                    Debug.Log("PUNTOS DE DAÑO REALIZADOS AL JUGADOR: " + puntos);
                }
            }
        }
    }
}

