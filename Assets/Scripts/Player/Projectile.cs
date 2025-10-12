using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Projectile : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private float velocidad = 10f;
    [SerializeField] private float tiempoDeVida = 2f;
    [SerializeField] private float tiempoMaxInactivo = 4f;

    [Header("Referencia BlocksController (automático si no se asigna)")]
    public BlocksController blocksController;

    private float daño;
    private Rigidbody2D rb;
    private bool haImpactado = false;
    private bool estaInactivo = false;
    private float tiempoInactivo = 0f;
    private Vector2 direccion = Vector2.right;
    private HashSet<GameObject> enemigosImpactados = new HashSet<GameObject>();
    private ObjectPooler pooler;
    private Coroutine vidaCoroutine;

    private Collider2D myCollider;
    private Collider2D jugadorCollider; // referencia temporal al jugador para ignorar colisión

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();

        if (blocksController == null)
            blocksController = Object.FindFirstObjectByType<BlocksController>();
    }

    public void Disparar(Vector3 posicion, Vector2 dir, float dañoProyectil)
    {
        ReiniciarEstado();

        transform.position = posicion;
        direccion = dir.normalized;
        daño = dañoProyectil;
        transform.localScale = new Vector3(direccion.x < 0 ? -1 : 1, 1, 1);

        // Buscar collider del jugador y evitar colisión temporal
        GameObject jugadorGO = GameObject.FindGameObjectWithTag("Player");
        if (jugadorGO != null)
        {
            jugadorCollider = jugadorGO.GetComponent<Collider2D>();
            if (jugadorCollider != null && myCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, jugadorCollider, true);
                StartCoroutine(ReactivarColisionConJugador(0.2f)); // 0.2 segundos de inmunidad
            }
        }

        // Activar y aplicar movimiento
        gameObject.SetActive(true);

        if (rb != null)
        {
            rb.linearVelocity = direccion * velocidad;
            rb.gravityScale = 0f;
        }

        // Comenzar temporizador de vida
        if (vidaCoroutine != null)
            StopCoroutine(vidaCoroutine);
        vidaCoroutine = StartCoroutine(VolverAlPoolDespuesDeVida());
    }

    private IEnumerator ReactivarColisionConJugador(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (jugadorCollider != null && myCollider != null)
        {
            Physics2D.IgnoreCollision(myCollider, jugadorCollider, false);
        }
    }

    private void Update()
    {
        if (estaInactivo)
        {
            tiempoInactivo += Time.deltaTime;
            if (tiempoInactivo >= tiempoMaxInactivo)
                VolverAlPool();
        }
    }

    private IEnumerator VolverAlPoolDespuesDeVida()
    {
        yield return new WaitForSeconds(tiempoDeVida);
        if (!estaInactivo)
            VolverAlPool();
    }

    public void SetPooler(ObjectPooler nuevoPooler) => pooler = nuevoPooler;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (haImpactado) return;

        // Evitar autoimpacto con jugador
        if (collision.CompareTag("Player")) return;

        // Daño a enemigos
        if (collision.CompareTag("Enemy") && !enemigosImpactados.Contains(collision.gameObject))
        {
            enemigosImpactados.Add(collision.gameObject);
            Enemy enemigo = collision.GetComponent<Enemy>();
            if (enemigo != null) enemigo.RecibirDaño(daño);
        }

        // Destruir bloque si choca con suelo
        if (collision.CompareTag("Ground") && blocksController != null)
        {
            Vector3 hitPos = collision.ClosestPoint(rb.position);
            blocksController.DestroyTileAtWorldPosition(hitPos);
        }

        // Convertir en inactivo (gris con gravedad)
        ConvertirEnInactivo();
    }

    private void ConvertirEnInactivo()
    {
        haImpactado = true;
        estaInactivo = true;
        tiempoInactivo = 0f;

        if (rb != null)
        {
            rb.gravityScale = 1f;
            rb.linearVelocity = Vector2.zero;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.isTrigger = false;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.gray;
    }

    private void VolverAlPool()
    {
        if (pooler != null)
            pooler.ReturnToPool(gameObject);
        else
            gameObject.SetActive(false);
    }

    private void ReiniciarEstado()
    {
        haImpactado = false;
        estaInactivo = false;
        tiempoInactivo = 0f;
        enemigosImpactados.Clear();

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = Color.green;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.isTrigger = true;

        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
        }
    }
}
