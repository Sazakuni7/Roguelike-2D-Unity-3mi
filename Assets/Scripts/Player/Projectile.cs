using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float velocidad;
    [SerializeField] private float tiempoDeVida;

    private float daño; // Ahora el daño se configura dinámicamente
    private Rigidbody2D rb;
    private bool haImpactado = false;
    private Vector2 direccion = Vector2.right;
    private HashSet<GameObject> enemigosImpactados = new HashSet<GameObject>();

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Start() => Destroy(gameObject, tiempoDeVida);

    private void FixedUpdate()
    {
        if (!haImpactado && rb != null)
        {
            rb.linearVelocity = direccion * velocidad; // Usa la dirección configurada
        }
    }

    // Asigna la dirección inicial del proyectil
    public void SetDireccion(Vector2 dir) => direccion = dir;

    public void SetDaño(float nuevoDaño) => daño = nuevoDaño; // Método para configurar el daño


    // Manejo de colisiones: si impacta con un enemigo, 
    // aplica daño (si aún no lo había hecho antes) y se destruye.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!haImpactado && collision.CompareTag("Enemy") && !enemigosImpactados.Contains(collision.gameObject))
        {
            enemigosImpactados.Add(collision.gameObject);
            Enemy enemigo = collision.GetComponent<Enemy>();
            if (enemigo != null) enemigo.RecibirDaño(daño); // Aplica el daño configurado
            ConvertirEnInactivo();
        }
    }

    // Se autodestruye tras cierto tiempo de vida.
    private void ConvertirEnInactivo()
    {
        haImpactado = true;
        if (rb != null) rb.gravityScale = 1f;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.isTrigger = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null) sr.color = Color.gray;
    }
}