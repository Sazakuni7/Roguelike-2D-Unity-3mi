using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private float daño = 2f;
    [SerializeField] private float tiempoDeVida = 5f;

    private Rigidbody2D rb;
    private bool haImpactado = false;
    private Vector2 direccion = Vector2.right; // Dirección inicial (derecha)
    private HashSet<GameObject> enemigosImpactados = new HashSet<GameObject>(); // Para evitar múltiples impactos

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Destruir el proyectil después de un tiempo
        Destroy(gameObject, tiempoDeVida);
    }

    private void Update()
    {
        if (!haImpactado)
        {
            // Movimiento del proyectil
            rb.linearVelocity = direccion * velocidad;
        }
    }

    public void SetDireccion(Vector2 nuevaDireccion)
    {
        direccion = nuevaDireccion;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Colisión detectada con: " + collision.gameObject.name);

        if (!haImpactado && collision.gameObject.CompareTag("Enemy"))
        {
            // Verificar si ya impactó a este enemigo
            if (!enemigosImpactados.Contains(collision.gameObject))
            {
                enemigosImpactados.Add(collision.gameObject);

                // Aplicar daño al enemigo
                Enemy enemigo = collision.gameObject.GetComponent<Enemy>();
                if (enemigo != null)
                {
                    enemigo.RecibirDaño(daño);
                    Debug.Log("Daño aplicado al enemigo: " + daño);
                }

                // Cambiar el estado del proyectil a inactivo
                ConvertirEnInactivo();
            }
        }
    }

    private void ConvertirEnInactivo()
    {
        haImpactado = true;

        // Activar la gravedad
        rb.gravityScale = 1f;

        // Desactivar el trigger para que interactúe con colisiones físicas
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = false;
        }

        // Opcional: Cambiar la apariencia del proyectil para indicar que está inactivo
        GetComponent<SpriteRenderer>().color = Color.gray;
    }
}