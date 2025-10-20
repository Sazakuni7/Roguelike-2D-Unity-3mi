using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public static event Action<float> OnEnemyDestroyed;
    public event Action<Enemy> OnEnemyDestroyedInstance;

    [SerializeField] private float vidaBase = 16f; // Vida base antes de escalar
    [SerializeField] private float experienciaOtorgada = 10f;

    private float vidaActual;
    private bool haMuerto = false;

    public float VidaMaxima { get; private set; }

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
        if (vidaActual <= 0) Morir();
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
    }
}
