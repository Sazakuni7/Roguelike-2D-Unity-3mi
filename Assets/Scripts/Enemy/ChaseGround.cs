using UnityEngine;

public class ChaseGround : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] float velocidad = 5f;
    [SerializeField] Transform jugador;

    private Rigidbody2D miRigidbody2D;
    private Vector2 direccion;
    private bool jugadorEnRango = false;

    private void Awake()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!jugadorEnRango) // Solo se mueve si no está tocando al jugador
        {
            direccion = (jugador.position - transform.position).normalized;
            miRigidbody2D.AddForce(direccion * velocidad);

            float velocidadMaxima = 10f;
            if (miRigidbody2D.linearVelocity.magnitude > velocidadMaxima)
            {
                miRigidbody2D.linearVelocity = miRigidbody2D.linearVelocity.normalized * velocidadMaxima;
            }
        }
        else
        {
            miRigidbody2D.linearVelocity = Vector2.zero; // detener movimiento
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorEnRango = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorEnRango = false;
        }
    }
}