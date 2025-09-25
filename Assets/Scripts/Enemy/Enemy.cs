using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    public static event Action<float> OnEnemyDestroyed; // Evento estático con experiencia
    [SerializeField] private float vida = 6f;
    [SerializeField] private float experienciaOtorgada = 10f; // Experiencia que otorga este enemigo

    private bool haMuerto = false;

    // Aplica daño al enemigo. Si la vida llega a 0, se destruye y entrega experiencia al jugador.
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
        if (haMuerto) return; // Evitar múltiples ejecuciones
        haMuerto = true;

        Debug.Log($"Enemigo destruido. Experiencia otorgada: {experienciaOtorgada}");
        OnEnemyDestroyed?.Invoke(experienciaOtorgada);
        Destroy(gameObject);
    }
}