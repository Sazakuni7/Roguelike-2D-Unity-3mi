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
        if (!jugadorEnRango)
        {
            direccion = (jugador.position - transform.position).normalized;

            // Mantener velocidad horizontal hacia el jugador
            miRigidbody2D.linearVelocity = new Vector2(direccion.x * velocidad, miRigidbody2D.linearVelocity.y);

            // (opcional) limitar velocidad vertical si no querés que se dispare
            if (miRigidbody2D.linearVelocity.magnitude > 10f)
            {
                miRigidbody2D.linearVelocity = miRigidbody2D.linearVelocity.normalized * 10f;
            }
        }
        else
        {
            miRigidbody2D.linearVelocity = Vector2.zero;
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