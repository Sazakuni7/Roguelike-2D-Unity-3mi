using UnityEngine;

public class ChaseAir : MonoBehaviour
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
        if (!jugadorEnRango) // Solo persigue si no está tocando al jugador
        {
            direccion = (jugador.position - transform.position).normalized;
            miRigidbody2D.MovePosition(miRigidbody2D.position + direccion * (velocidad * Time.fixedDeltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorEnRango = true;
            miRigidbody2D.linearVelocity = Vector2.zero; // detener en seco
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorEnRango = false; // vuelve a perseguir
        }
    }
}