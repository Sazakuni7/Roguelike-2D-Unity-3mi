using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float fuerzaEmpuje = 5f; // Fuerza con la que se empuja al enemigo

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                // Hacer da�o al jugador
               // jugador.ModificarVida(-10); // Ejemplo de da�o al jugador

                // Empujar al enemigo hacia atr�s
                enemy.EmpujarDesdeJugador(transform.position, fuerzaEmpuje * 1.5f); // Aumenta la fuerza de empuje
            }
        }
    }
}