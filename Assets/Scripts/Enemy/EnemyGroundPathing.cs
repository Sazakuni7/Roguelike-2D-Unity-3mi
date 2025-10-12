using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyGroundPathing : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidad = 3f;
    [SerializeField] private float fuerzaSalto = 6f;
    [SerializeField] private float fuerzaSaltoAlta = 9f;
    [SerializeField] private float distanciaSuelo = 0.1f;
    [SerializeField] private float distanciaPared = 0.6f;
    [SerializeField] private float tiempoAtasco = 1.2f;
    [SerializeField] private float retrocesoDistancia = 1.2f;
    [SerializeField] private LayerMask capaSuelo;

    [Header("Referencias")]
    [SerializeField] private Transform jugador;
    [SerializeField] private Transform checkSuelo;

    private Rigidbody2D rb;
    private bool mirandoDerecha = true;
    private float tiempoQuieto = 0f;
    private Vector2 ultimaPosicion;
    private bool enEvasion = false;
    private Vector2 puntoMaxRetroceso;
    private float temporizadorEvasion = 0f;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        if (jugador == null) return;

        Vector2 dirJugador = (jugador.position - transform.position).normalized;
        float distanciaJugador = Vector2.Distance(jugador.position, transform.position);
        float dirX = Mathf.Sign(dirJugador.x);

        bool enSuelo = Physics2D.Raycast(checkSuelo.position, Vector2.down, distanciaSuelo, capaSuelo);
        bool paredDelante = Physics2D.Raycast(transform.position, Vector2.right * dirX, distanciaPared, capaSuelo);

        // --- DETECTAR ATASCO ---
        float movHorizontal = Mathf.Abs(transform.position.x - ultimaPosicion.x);
        if (movHorizontal < 0.05f)
            tiempoQuieto += Time.fixedDeltaTime;
        else
            tiempoQuieto = 0f;

        // --- ENTRAR EN MODO EVASIÓN ---
        if (!enEvasion && tiempoQuieto >= tiempoAtasco)
        {
            enEvasion = true;
            temporizadorEvasion = 1.2f;

            // Punto máximo hacia el que puede retroceder (sin salirse del terreno previo)
            puntoMaxRetroceso = transform.position - new Vector3(dirX * retrocesoDistancia, 0f, 0f);

            // Si no hay suelo en ese punto, cancelar retroceso
            if (!HaySueloDebajo(puntoMaxRetroceso))
                puntoMaxRetroceso = transform.position;
        }

        // --- MOVIMIENTO NORMAL ---
        if (!enEvasion)
        {
            rb.linearVelocity = new Vector2(dirX * velocidad, rb.linearVelocity.y);

            if (enSuelo)
            {
                // Si hay una pared o pequeño obstáculo, saltar
                if (paredDelante)
                    Saltar(fuerzaSalto);
                else if (Random.value < 0.005f) // saltos ocasionales
                    Saltar(fuerzaSalto);
            }
        }
        else
        {
            // --- MODO EVASIÓN ---
            temporizadorEvasion -= Time.fixedDeltaTime;

            // Retroceder un poco
            float dirRetroceso = -dirX;
            rb.linearVelocity = new Vector2(dirRetroceso * velocidad * 0.8f, rb.linearVelocity.y);

            // Si hay suelo y ya retrocedió lo suficiente, saltar alto y salir de evasión
            if (Vector2.Distance(transform.position, puntoMaxRetroceso) >= retrocesoDistancia * 0.8f && enSuelo)
            {
                Saltar(fuerzaSaltoAlta);
                enEvasion = false;
                tiempoQuieto = 0f;
            }

            // Seguridad: si se quedó sin suelo o pasó tiempo
            if (temporizadorEvasion <= 0)
                enEvasion = false;
        }

        ultimaPosicion = transform.position;

        // --- Rotar sprite ---
        if ((dirX > 0 && !mirandoDerecha) || (dirX < 0 && mirandoDerecha))
        {
            mirandoDerecha = !mirandoDerecha;
            Vector3 escala = transform.localScale;
            escala.x *= -1;
            transform.localScale = escala;
        }
    }

    private void Saltar(float fuerza)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // reset vertical
        rb.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse);
    }

    private bool HaySueloDebajo(Vector2 punto)
    {
        RaycastHit2D hit = Physics2D.Raycast(punto, Vector2.down, 2f, capaSuelo);
        return hit.collider != null;
    }

    public void SetJugador(Transform jugadorTransform) => jugador = jugadorTransform;
}
