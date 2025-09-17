using System;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    public event Action<float, float> OnExperienciaCambiada; // Evento para notificar cambios en la experiencia
    public event Action<float> OnVidaCambiada; // Evento para notificar cambios en la vida
    public event Action<float> OnDa�oCambiado; // Evento para notificar cambios en el da�o

    [Header("Configuraci�n")]
    [SerializeField] private float vida;
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform puntoDeDisparo;
    [SerializeField] private float tiempoEntreDisparos;
    [SerializeField] private float velocidad;

    [Header("Progresi�n del Jugador")]
    [SerializeField] private PlayerProgressionData datosProgresion; // Referencia al ScriptableObject
    public PlayerProgressionData DatosProgresion => datosProgresion;

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

    private void OnEnable()
    {
        // Suscribirse al evento de destrucci�n de enemigos
        Enemy.OnEnemyDestroyed += AgregarExperiencia;
    }

    private void OnDisable()
    {
        // Cancelar la suscripci�n al evento de destrucci�n de enemigos
        Enemy.OnEnemyDestroyed -= AgregarExperiencia;
    }

    private void Update()
    {
        // Direccionamiento horizontal
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
            {
                float da�oProyectil = datosProgresion.da�oBase;
                p.SetDa�o(da�oProyectil); // Configura el da�o del proyectil desde la progresi�n
                p.SetDireccion(direccionDisparo); // Configura la direcci�n del proyectil

                // Debug para verificar el da�o y direcci�n del proyectil
                Debug.Log($"Proyectil disparado con da�o: {da�oProyectil}, Direcci�n: {direccionDisparo}");
            }
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

    private void AgregarExperiencia(float experiencia)
    {
        datosProgresion.AgregarExperiencia(experiencia);

        // Notificar a la UI sobre el cambio de experiencia
        OnExperienciaCambiada?.Invoke(datosProgresion.experienciaActual, datosProgresion.experienciaNecesaria);

        // Notificar a la UI sobre el cambio de da�o
        OnDa�oCambiado?.Invoke(datosProgresion.da�oBase);
    }

    public PlayerProgressionData GetDatosProgresion()
    {
        return datosProgresion;
    }

    public void SetDatosProgresion(PlayerProgressionData nuevosDatos)
    {
        datosProgresion = nuevosDatos;
    }
}