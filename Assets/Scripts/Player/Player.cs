using System;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    public event Action<float, float> OnExperienciaCambiada;
    public event Action<float> OnVidaCambiada;
    public event Action<float> OnDañoCambiado;
    public event Action<int> OnNivelCambiado;

    [Header("Configuración")]
    [SerializeField] private float vida = 100f;
    [SerializeField] private Transform puntoDeDisparo;
    [SerializeField] private float tiempoEntreDisparos = 0.3f;
    [SerializeField] private float velocidad = 5f;

    [Header("Jetpack")]
    [SerializeField] private float fuelMaximo = 3f;
    [SerializeField] private float regeneracionFuelPorSegundo = 1f;
    [SerializeField] private float consumoFuelPorSegundo = 1f;
    [SerializeField] private float fuerzaJetpack = 5f;
    [SerializeField] private float ventanaActivacionJetpack = 0.2f;

    [Header("Progresión del Jugador")]
    [SerializeField] private PlayerProgressionData datosProgresion;

    private float tiempoUltimoSalto;
    private float fuelActual;
    private bool usandoJetpack = false;

    public PlayerProgressionData DatosProgresion => datosProgresion;

    public event Action<float, float> OnFuelCambiado;
    private float tiempoUltimoDisparo;
    private Vector2 direccionDisparo = Vector2.right;
    private Vector3 escalaInicial;
    private Rigidbody2D rb;
    private ObjectPooler projectilePooler;
    public float FuelActual => fuelActual;
    public float FuelMaximo => fuelMaximo;

    // Referencia a Saltar para detectar si está en suelo
    private Saltar saltar;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        saltar = GetComponent<Saltar>();
        escalaInicial = transform.localScale;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        projectilePooler = ObjectPooler.Instance;
        fuelActual = fuelMaximo;
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDestroyed += AgregarExperiencia;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDestroyed -= AgregarExperiencia;
    }

    private void Update()
    {
        // --- Movimiento horizontal ---
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

        direccionDisparo = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        // --- Disparo ---
        if (Input.GetKey(KeyCode.Q) && Time.time >= tiempoUltimoDisparo + tiempoEntreDisparos)
        {
            Disparar();
            tiempoUltimoDisparo = Time.time;
        }

        // --- Jetpack ---
        ManejarJetpack();
    }

    private void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * velocidad, rb.linearVelocity.y);
        }
    }

    // ==================================
    //       SISTEMA DE JETPACK
    // ==================================
    private void ManejarJetpack()
    {
        bool estoyEnSuelo = saltar != null && saltar.EstaEnSuelo();
        bool espacioPresionado = Input.GetKey(KeyCode.Space);

        // Registrar momento del salto
        if (Input.GetKeyDown(KeyCode.Space) && estoyEnSuelo)
        {
            tiempoUltimoSalto = Time.time;
            usandoJetpack = false; // aún no activo jetpack
        }

        // Regenerar fuel en suelo
        if (estoyEnSuelo && fuelActual < fuelMaximo)
        {
            fuelActual += regeneracionFuelPorSegundo * Time.deltaTime;
            fuelActual = Mathf.Min(fuelActual, fuelMaximo);
        }

        // Jetpack activo SOLO después de que ya haya pasado un frame del salto inicial
        if (!estoyEnSuelo &&
            (Time.time - tiempoUltimoSalto > 1.0f) && // pequeña ventana de delay
            espacioPresionado &&
            fuelActual > 0f)
        {
            usandoJetpack = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaJetpack);
            fuelActual -= consumoFuelPorSegundo * Time.deltaTime;
            fuelActual = Mathf.Max(fuelActual, 0f);
        }
        else
        {
            usandoJetpack = false;
        }

        // Notificar a UI
        OnFuelCambiado?.Invoke(fuelActual, fuelMaximo);
    }

    public float GetFuelActual() => fuelActual;
    public float GetFuelMaximo() => fuelMaximo;

    // ==================================
    //       DISPARO Y EXPERIENCIA
    // ==================================
    private void Disparar()
    {
        if (puntoDeDisparo == null || projectilePooler == null)
        {
           // Debug.LogWarning("No se encontró el punto de disparo o el ObjectPooler.");
            return;
        }

        GameObject proyectilGO = projectilePooler.GetPooledObject("Projectile");
        if (proyectilGO != null)
        {
            Projectile proyectil = proyectilGO.GetComponent<Projectile>();
            if (proyectil != null)
            {
                float dañoProyectil = datosProgresion.dañoBase;
                proyectil.Disparar(puntoDeDisparo.position, direccionDisparo, dañoProyectil);
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

    public float GetVida()
    {
        return vida;
    }

    private void AgregarExperiencia(float experiencia)
    {
        int nivelAnterior = datosProgresion.nivel;

        datosProgresion.AgregarExperiencia(experiencia);

        // Actualizar UI
        OnExperienciaCambiada?.Invoke(datosProgresion.experienciaActual, datosProgresion.experienciaNecesaria);
        OnDañoCambiado?.Invoke(datosProgresion.dañoBase);

        // Si subió de nivel
        if (datosProgresion.nivel > nivelAnterior)
        {
            OnNivelCambiado?.Invoke(datosProgresion.nivel);

            // Mejora del jetpack al subir de nivel
            fuelMaximo += 0.5f;
            regeneracionFuelPorSegundo += 0.1f;
            fuelActual = fuelMaximo; // rellena el tanque al subir de nivel

            Debug.Log($"Nivel {datosProgresion.nivel} alcanzado: Fuel máx {fuelMaximo}, Regen {regeneracionFuelPorSegundo}");
        }
    }


    public PlayerProgressionData GetDatosProgresion() => datosProgresion;
    public void SetDatosProgresion(PlayerProgressionData nuevosDatos) => datosProgresion = nuevosDatos;
}
