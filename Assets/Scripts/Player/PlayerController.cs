using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float fuerzaEmpuje = 5f; // Fuerza con la que se empuja al enemigo

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody2D enemyRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 direccion = (enemyRb.position - (Vector2)transform.position).normalized;
                enemyRb.linearVelocity += direccion * 3f; // pequeña fuerza de empuje
            }
        }
    }
}
