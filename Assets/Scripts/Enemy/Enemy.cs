using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public static event Action<float> OnEnemyDestroyed; // Evento estático con experiencia
    public event Action<Enemy> OnEnemyDestroyedInstance;
    [SerializeField] private float vida = 6f;
    [SerializeField] private float experienciaOtorgada = 10f; // Experiencia que otorga este enemigo

    private bool haMuerto = false;

    // Aplica daño al enemigo. Si la vida llega a 0, se desactiva y entrega experiencia al jugador.
    public void RecibirDaño(float daño)
    {
        vida -= daño;
        if (vida <= 0) Morir();
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

        Debug.Log($"Enemigo destruido. Experiencia otorgada: {experienciaOtorgada}");
        OnEnemyDestroyed?.Invoke(experienciaOtorgada);
        OnEnemyDestroyedInstance?.Invoke(this);

        // Desregistrar del GameManager
        GameManager.Instance.DesregistrarEnemigo(this);

        // Actualizar UI
        GameUI.Instance.ActualizarEnemigosRestantesUI();

        // Desactivar para reutilizar
        gameObject.SetActive(false);
        ResetearEstado();
    }

    private void ResetearEstado()
    {
        haMuerto = false;
        vida = 6f; // Reiniciar la vida del enemigo (puedes ajustar este valor según sea necesario)
    }
}