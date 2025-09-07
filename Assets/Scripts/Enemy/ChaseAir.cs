using UnityEngine;

public class ChaseAir : MonoBehaviour
{
    [SerializeField] float velocidad = 5f;
    [SerializeField] Transform jugador;

    private Rigidbody2D rb;
    private bool jugadorEnRango = false;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        if (Time.timeScale != 1f)
            Time.timeScale = 1f;

        if (rb == null || jugador == null) return;

        if (!jugadorEnRango)
        {
            Vector2 dir = (jugador.position - transform.position).normalized;
            rb.MovePosition(rb.position + dir * velocidad * Time.fixedDeltaTime);
        }
        else
            rb.linearVelocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            jugadorEnRango = true;
            if (rb != null) rb.linearVelocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) jugadorEnRango = false;
    }
}
