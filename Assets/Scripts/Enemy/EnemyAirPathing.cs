using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyAirPathing : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float velocidadHorizontal = 3f;
    [SerializeField] private float velocidadVertical = 2f;
    [SerializeField] private float distanciaDeteccion = 0.6f;
    [SerializeField] private float tiempoAtascoMax = 1.5f;
    [SerializeField] private float duracionEvasion = 1.2f;
    [SerializeField] private float distanciaMinimaNoEvasion = 2f; // Si está muy cerca, no evade
    [SerializeField] private LayerMask capaObstaculos;

    [Header("Referencias")]
    [SerializeField] private Transform jugador;

    private Rigidbody2D rb;
    private bool mirandoDerecha = true;

    private Vector2 ultimaPosicion;
    private float tiempoAtascado = 0f;
    private bool enEvasion = false;
    private float tiempoEvasionRestante = 0f;
    private Vector2 direccionEvasion;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void FixedUpdate()
    {
        if (jugador == null) return;

        Vector2 direccion = (jugador.position - transform.position).normalized;
        Vector2 origen = transform.position;
        float distanciaAlJugador = Vector2.Distance(transform.position, jugador.position);

        // --- Comprobar si hay una pared u obstáculo entre enemigo y jugador ---
        bool hayObstaculoEntre = Physics2D.Linecast(origen, jugador.position, capaObstaculos);

        // --- Raycast hacia adelante para detección de pared inmediata ---
        bool paredDelante = Physics2D.Raycast(origen, Vector2.right * Mathf.Sign(direccion.x), distanciaDeteccion, capaObstaculos);

        // Movimiento base
        float velX = direccion.x * velocidadHorizontal;
        float velY = direccion.y * velocidadVertical;

        if (paredDelante) velX *= 0.3f; // desacelerar si hay pared

        // --- DETECCIÓN DE ATASCO ---
        float desplazamiento = Vector2.Distance(transform.position, ultimaPosicion);
        if (desplazamiento < 0.05f)
            tiempoAtascado += Time.fixedDeltaTime;
        else
            tiempoAtascado = 0f;

        // --- CONDICIÓN DE EVASIÓN ---
        bool condicionesEvasion =
            (tiempoAtascado > tiempoAtascoMax || paredDelante) &&
            hayObstaculoEntre &&                    // solo si hay algo entre ambos
            distanciaAlJugador > distanciaMinimaNoEvasion &&  // solo si no está pegado al jugador
            !enEvasion;

        if (condicionesEvasion)
        {
            enEvasion = true;
            tiempoEvasionRestante = duracionEvasion;

            // decide dirección evasiva
            bool subir = Random.value > 0.5f || jugador.position.y > transform.position.y;
            float dirVertical = subir ? 1f : -1f;

            direccionEvasion = new Vector2(-Mathf.Sign(direccion.x), dirVertical).normalized;
        }

        // --- EJECUTAR EVASIÓN ---
        if (enEvasion)
        {
            velX = direccionEvasion.x * velocidadHorizontal;
            velY = direccionEvasion.y * velocidadVertical * 1.2f;

            tiempoEvasionRestante -= Time.fixedDeltaTime;
            if (tiempoEvasionRestante <= 0)
                enEvasion = false;
        }

        rb.linearVelocity = new Vector2(velX, velY);
        ultimaPosicion = transform.position;

        // --- Rotar sprite ---
        if ((velX > 0 && !mirandoDerecha) || (velX < 0 && mirandoDerecha))
        {
            mirandoDerecha = !mirandoDerecha;
            Vector3 esc = transform.localScale;
            esc.x *= -1;
            transform.localScale = esc;
        }
    }

    public void SetJugador(Transform jugadorTransform) => jugador = jugadorTransform;
}
