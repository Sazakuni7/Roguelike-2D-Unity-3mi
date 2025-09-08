using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float velocidad;
    [SerializeField] private float daño;
    [SerializeField] private float tiempoDeVida;

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
            rb.linearVelocity = direccion * velocidad;
        }
    }

    public void SetDireccion(Vector2 dir) => direccion = dir;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!haImpactado && collision.CompareTag("Enemy") && !enemigosImpactados.Contains(collision.gameObject))
        {
            enemigosImpactados.Add(collision.gameObject);
            Enemy enemigo = collision.GetComponent<Enemy>();
            if (enemigo != null) enemigo.RecibirDaño(daño);
            ConvertirEnInactivo();
        }
    }

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
