using System;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    public event Action<float> OnVidaCambiada;

    [Header("Configuración")]
    [SerializeField] private float vida;
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform puntoDeDisparo;
    [SerializeField] private float tiempoEntreDisparos;
    [SerializeField] private float velocidad;

    private float tiempoUltimoDisparo;
    private Vector2 direccionDisparo = Vector2.right;
    private Vector3 escalaInicial;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        escalaInicial = transform.localScale;

        // Resetear velocidad inicial
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void Update()
    {
        // Dirección horizontal
        float moverHorizontal = Input.GetAxis("Horizontal");
        if (moverHorizontal > 0)
        {
            direccionDisparo = Vector2.right;
            transform.localScale = escalaInicial;
        }
        else if (moverHorizontal < 0)
        {
            direccionDisparo = Vector2.left;
            transform.localScale = new Vector3(-escalaInicial.x, escalaInicial.y, escalaInicial.z);
        }

        // Disparo
        if (Input.GetKey(KeyCode.Q) && Time.time >= tiempoUltimoDisparo + tiempoEntreDisparos)
        {
            Disparar();
            tiempoUltimoDisparo = Time.time;
        }
    }

    private void FixedUpdate()
    {
        // Movimiento horizontal
        if (rb != null)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * velocidad, rb.velocity.y);
        }
    }

    private void Disparar()
    {
        if (proyectilPrefab != null && puntoDeDisparo != null)
        {
            GameObject proyectil = Instantiate(proyectilPrefab, puntoDeDisparo.position, Quaternion.identity);
            Projectile p = proyectil.GetComponent<Projectile>();
            if (p != null)
                p.SetDireccion(direccionDisparo);
        }
    }

    public void ModificarVida(float puntos)
    {
        vida += puntos;
        if (vida < 0) vida = 0;

        OnVidaCambiada?.Invoke(vida);

        if (vida <= 0)
        {
            GameEvents.TriggerGameOver();
        }
    }

    public float GetVida() => vida;
}