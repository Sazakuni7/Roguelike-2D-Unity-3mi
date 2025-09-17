using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (enemy != null)
            {
                // L�gica de colisi�n con enemigos (si es necesario)
            }
        }

        if (collision.gameObject.CompareTag("Victory"))
        {
            GameEvents.TriggerVictory();
        }
    }
}