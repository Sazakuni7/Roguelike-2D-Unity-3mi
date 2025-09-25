using System;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    public event Action<float, float> OnExperienciaCambiada; // Evento para notificar cambios en la experiencia
    public event Action<float> OnVidaCambiada; // Evento para notificar cambios en la vida
    public event Action<float> OnDañoCambiado; // Evento para notificar cambios en el da�o
    public event Action<int> OnNivelCambiado; // Evento para notificar cambios en el nivel del jugador

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
            rb.linearVelocity = Vector2.zero;
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
            rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * velocidad, rb.linearVelocity.y);
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
                float dañoProyectil = datosProgresion.dañoBase;
                p.SetDaño(dañoProyectil); // Configura el da�o del proyectil desde la progresi�n
                p.SetDireccion(direccionDisparo); // Configura la direcci�n del proyectil

                // Debug para verificar el da�o y direcci�n del proyectil
                Debug.Log($"Proyectil disparado con da�o: {dañoProyectil}, Direcci�n: {direccionDisparo}");
            }
        }
    }

    // L�gica de da�o recibido por el jugador. 
    // Si la vida llega a cero, notifica la muerte.
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

    // M�todo principal para manejar la progresi�n: agrega experiencia, 
    // recalcula nivel/da�o y notifica a la UI si hubo cambios
    private void AgregarExperiencia(float experiencia)
    {
        int nivelAnterior = datosProgresion.nivel;
        datosProgresion.AgregarExperiencia(experiencia);

        OnExperienciaCambiada?.Invoke(datosProgresion.experienciaActual, datosProgresion.experienciaNecesaria);
        OnDañoCambiado?.Invoke(datosProgresion.dañoBase);

        if (datosProgresion.nivel > nivelAnterior)
            OnNivelCambiado?.Invoke(datosProgresion.nivel);
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