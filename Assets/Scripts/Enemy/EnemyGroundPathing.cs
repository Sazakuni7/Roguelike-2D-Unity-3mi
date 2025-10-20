using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyGroundPathing : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 3f;
    [SerializeField] private float fuerzaSalto = 6f;
    [SerializeField] private float fuerzaSaltoAlta = 9f;
    [SerializeField] private float tiempoEntreSaltos = 0.5f;

    [SerializeField] private float distanciaSuelo = 0.1f;
    [SerializeField] private float distanciaPared = 0.6f;
    [SerializeField] private float tiempoAtasco = 1.2f;
    [SerializeField] private float retrocesoDistancia = 1.2f;
    [SerializeField] private LayerMask capaSuelo;

    [Header("Referencias")]
    [SerializeField] private Transform jugador;
    [SerializeField] private Transform checkSuelo;

    private Rigidbody2D rb;
    private Animator anim;

    private float tiempoUltimoSalto = 0f;
    private bool mirandoDerecha = true;
    private float tiempoQuieto = 0f;
    private Vector2 ultimaPosicion;
    private bool enEvasion = false;
    private Vector2 puntoMaxRetroceso;
    private float temporizadorEvasion = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (jugador == null) return;

        // --- Dirección y distancia hacia jugador ---
        Vector2 dirJugador = (jugador.position - transform.position).normalized;
        float dirX = Mathf.Sign(dirJugador.x);

        // --- Raycast para suelo y pared ---
        bool enSuelo = Physics2D.Raycast(checkSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);
        bool paredDelante = Physics2D.Raycast(transform.position, Vector2.right * dirX, distanciaPared, capaSuelo);

        // --- Detectar atasco ---
        float movHorizontal = Mathf.Abs(transform.position.x - ultimaPosicion.x);
        tiempoQuieto = (movHorizontal < 0.05f) ? tiempoQuieto + Time.fixedDeltaTime : 0f;

        // --- Salto normal con cooldown ---
        if (enSuelo && Time.time >= tiempoUltimoSalto + tiempoEntreSaltos)
        {
            if (paredDelante || Random.value < 0.005f)
            {
                Saltar(fuerzaSalto);
                tiempoUltimoSalto = Time.time;
            }
        }

        // --- Entrar en modo evasión ---
        if (!enEvasion && tiempoQuieto >= tiempoAtasco)
        {
            enEvasion = true;
            temporizadorEvasion = 1.2f;
            puntoMaxRetroceso = transform.position - new Vector3(dirX * retrocesoDistancia, 0f, 0f);

            if (!HaySueloDebajo(puntoMaxRetroceso))
                puntoMaxRetroceso = transform.position;
        }

        // --- Movimiento normal ---
        float velX = dirX * velocidad;
        if (!enEvasion)
        {
            rb.linearVelocity = new Vector2(velX, rb.linearVelocity.y);
        }
        else
        {
            // --- Modo evasión ---
            temporizadorEvasion -= Time.fixedDeltaTime;
            float dirRetroceso = -dirX;
            rb.linearVelocity = new Vector2(dirRetroceso * velocidad * 0.8f, rb.linearVelocity.y);

            if (Vector2.Distance(transform.position, puntoMaxRetroceso) >= retrocesoDistancia * 0.8f && enSuelo)
            {
                Saltar(fuerzaSaltoAlta);
                enEvasion = false;
                tiempoQuieto = 0f;
            }

            if (temporizadorEvasion <= 0)
                enEvasion = false;
        }

        ultimaPosicion = transform.position;

      /*  // --- Rotar sprite ---
        if ((dirX > 0 && !mirandoDerecha) || (dirX < 0 && mirandoDerecha))
        {
            mirandoDerecha = !mirandoDerecha;
            Vector3 escala = transform.localScale;
            escala.x *= -1;
            transform.localScale = escala;
        }*/

        // --- Actualizar Animator ---
        anim.SetBool("isMoving", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
        anim.SetBool("isJumping", !enSuelo);
    }

    private void Saltar(float fuerza)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse);
    }

    private bool HaySueloDebajo(Vector2 punto)
    {
        RaycastHit2D hit = Physics2D.Raycast(punto, Vector2.down, 2f, capaSuelo);
        return hit.collider != null;
    }

    public void SetJugador(Transform jugadorTransform) => jugador = jugadorTransform;
}
