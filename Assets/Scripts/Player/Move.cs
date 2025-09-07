using UnityEngine;

public class Mover : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float velocidad = 5f;

    private Rigidbody2D miRigidbody2D;
    private SpriteRenderer miSpriteRenderer;
    private Vector2 direccion;

    // Activar controles automáticamente
    private bool activo = true;

    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miSpriteRenderer = GetComponent<SpriteRenderer>();

        // Reset inicial de velocidad
        if (miRigidbody2D != null)
        {
            miRigidbody2D.linearVelocity = Vector2.zero;
            miRigidbody2D.angularVelocity = 0f;
        }
    }

    private void Update()
    {
        if (!activo) return;

        float moverHorizontal = Input.GetAxis("Horizontal");
        direccion = new Vector2(moverHorizontal, 0f);

        if (miSpriteRenderer != null)
        {
            if (moverHorizontal > 0) miSpriteRenderer.flipX = false;
            else if (moverHorizontal < 0) miSpriteRenderer.flipX = true;
        }
    }

    private void FixedUpdate()
    {
        if (Time.timeScale != 1f)
            Time.timeScale = 1f;
        if (!activo || miRigidbody2D == null) return;

        Vector2 velocidadActual = miRigidbody2D.linearVelocity;
        miRigidbody2D.linearVelocity = new Vector2(direccion.x * velocidad, velocidadActual.y);
    }
}