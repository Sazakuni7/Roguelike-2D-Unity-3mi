using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public static event Action<float> OnEnemyDestroyed;
    public event Action<Enemy> OnEnemyDestroyedInstance;

    [SerializeField] private float vidaBase = 16f; // Vida base antes de escalar
    [SerializeField] private float experienciaOtorgada = 10f;
    [SerializeField] private Color colorDaño = Color.red; // Color al recibir daño
    [SerializeField] private float duracionColorDaño = 0.2f; // Duración del cambio de color

    private float vidaActual;
    private bool haMuerto = false;
    private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer original
    private Color colorOriginal; // Color original del enemigo

    public float VidaMaxima { get; private set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }
    }

    private void OnEnable()
    {
        ResetearEstado();
    }

    public void Inicializar(float multiplicadorVida)
    {
        VidaMaxima = vidaBase * multiplicadorVida;
        vidaActual = VidaMaxima;
        haMuerto = false;
    }

    public void RecibirDaño(float daño)
    {
        vidaActual -= daño;

        // Iniciar la corrutina para cambiar el color al recibir daño
        if (spriteRenderer != null)
        {
            StartCoroutine(CambiarColorTemporalmente());
        }

        if (vidaActual <= 0) Morir();
    }

    private IEnumerator CambiarColorTemporalmente()
    {
        // Cambiar al color de daño
        spriteRenderer.color = colorDaño;

        // Esperar un tiempo
        yield return new WaitForSeconds(duracionColorDaño);

        // Restaurar el color original
        spriteRenderer.color = colorOriginal;
    }

    public void EmpujarDesdeJugador(Vector2 posicionJugador, float fuerzaEmpuje)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 direccionEmpuje = (transform.position - (Vector3)posicionJugador).normalized;
            rb.AddForce(direccionEmpuje * fuerzaEmpuje, ForceMode2D.Impulse);
        }
    }

    private void Morir()
    {
        if (haMuerto) return;
        haMuerto = true;

        OnEnemyDestroyed?.Invoke(experienciaOtorgada);
        OnEnemyDestroyedInstance?.Invoke(this);

        GameManager.Instance.DesregistrarEnemigo(this);
        GameUI.Instance.ActualizarEnemigosRestantesUI();

        gameObject.SetActive(false);
    }

    public void ResetearEstado()
    {
        haMuerto = false;
        vidaActual = VidaMaxima;

        // Restaurar el color original al reiniciar el estado
        if (spriteRenderer != null)
        {
            spriteRenderer.color = colorOriginal;
        }
    }
}