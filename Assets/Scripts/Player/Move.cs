using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    // Variables a configurar desde el editor
    [Header("Configuracion")]
    [SerializeField] float velocidad;

    // Variables de uso interno en el script
    private float moverHorizontal;
    private Vector2 direccion;

    // Variable para referenciar otro componente del objeto
    private Rigidbody2D miRigidbody2D;
    private SpriteRenderer miSpriteRenderer;

    // Código ejecutado cuando el objeto se activa en el nivel
    private void OnEnable()
    {
        miRigidbody2D = GetComponent<Rigidbody2D>();
        miSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Código ejecutado en cada frame del juego (Intervalo variable)
    private void Update()
    {
        moverHorizontal = Input.GetAxis("Horizontal");
        direccion = new Vector2(moverHorizontal, 0f);
        if (moverHorizontal > 0)
            miSpriteRenderer.flipX = false;
        else if (moverHorizontal < 0)
            miSpriteRenderer.flipX = true;

    }

    private void FixedUpdate()
    {
        // Conservar la velocidad vertical y modificar solo la horizontal
        Vector2 velocidadActual = miRigidbody2D.linearVelocity;
        miRigidbody2D.linearVelocity = new Vector2(direccion.x * velocidad, velocidadActual.y);
    }
}