using UnityEngine;

public class ChaseGround : MonoBehaviour
{
    [SerializeField] float velocidad = 5f;
    [SerializeField] Transform jugador;

    private Rigidbody2D rb;
    private bool jugadorEnRango = false;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    // Persigue al objetivo indicado (jugador).
    // Calcula la direcci�n y ajusta la velocidad en consecuencia.
    private void FixedUpdate()
    {

        if (rb == null || jugador == null) return;

        if (!jugadorEnRango)
        {
            Vector2 dir = (jugador.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(dir.x * velocidad, rb.linearVelocity.y);
        }
        else
            rb.linearVelocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) jugadorEnRango = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) jugadorEnRango = false;
    }
}
