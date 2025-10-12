using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    [Header("Referencias")]
    [SerializeField] private TMP_Text vidaTmp;
    [SerializeField] private TMP_Text enemigosRestantesTmp;
    [SerializeField] private TMP_Text mensajeMuerteTmp;
    [SerializeField] private TMP_Text mensajeVictoriaTmp;
    [SerializeField] private TMP_Text experienciaTmp;
    [SerializeField] private TMP_Text nivelTmp;
    [SerializeField] private TMP_Text dañoTmp;

    [Header("Jetpack Fuel")]
    [SerializeField] private UnityEngine.UI.Image fuelBarFill;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
        GameEvents.OnGameOver += MostrarGameOver;
        GameEvents.OnVictory += MostrarVictoria;
        GameEvents.OnPlayerSpawned += ConectarJugador;

        ActualizarEnemigosRestantesUI();

        Jugador jugadorExistente = Object.FindFirstObjectByType<Jugador>();
        if (jugadorExistente != null) ConectarJugador(jugadorExistente);
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= MostrarGameOver;
        GameEvents.OnVictory -= MostrarVictoria;
    }

   public void ConectarJugador(Jugador jugador)
    {
        if (jugador == null) return;
        jugador.OnVidaCambiada -= ActualizarVida;
        jugador.OnVidaCambiada += ActualizarVida;
        jugador.OnExperienciaCambiada -= ActualizarExperiencia;
        jugador.OnExperienciaCambiada += ActualizarExperiencia;
        jugador.OnDañoCambiado -= ActualizarDaño;
        jugador.OnDañoCambiado += ActualizarDaño;
        jugador.OnNivelCambiado -= ActualizarNivel;
        jugador.OnNivelCambiado += ActualizarNivel;
        jugador.OnFuelCambiado -= ActualizarFuel;
        jugador.OnFuelCambiado += ActualizarFuel;

        ActualizarFuel(jugador.FuelActual, jugador.FuelMaximo);
        ActualizarUIJugador(jugador);
    }

    private void ActualizarUIJugador(Jugador jugador)
    {
        ActualizarVida(jugador.GetVida());
        ActualizarExperiencia(jugador.DatosProgresion.experienciaActual, jugador.DatosProgresion.experienciaNecesaria);
        ActualizarNivel(jugador.DatosProgresion.nivel);
        ActualizarDaño(jugador.DatosProgresion.dañoBase);
    }

    private void MostrarGameOver()
    {
        if (mensajeMuerteTmp != null)
        {
            mensajeMuerteTmp.gameObject.SetActive(true);
            mensajeMuerteTmp.text = "¡Has muerto!";
        }
    }

    private void MostrarVictoria()
    {
        if (mensajeVictoriaTmp != null)
        {
            mensajeVictoriaTmp.gameObject.SetActive(true);
            mensajeVictoriaTmp.text = "¡Has ganado!";
        }
    }

    private void ActualizarVida(float nuevaVida)
    {
        if (vidaTmp != null)
            vidaTmp.text = "Vida: " + nuevaVida.ToString("F0") + "%";
    }

    private void ActualizarExperiencia(float experienciaActual, float experienciaParaNivel2)
    {
        if (experienciaTmp != null)
        {
            float experienciaFaltante = experienciaParaNivel2 - experienciaActual;
            experienciaTmp.text = $"Experiencia: {experienciaActual:F0}/{experienciaParaNivel2:F0} (Faltan {experienciaFaltante:F0})";
        }
    }

    private void ActualizarNivel(int nivel)
    {
        if (nivelTmp != null)
            nivelTmp.text = $"Nivel: {nivel}";
    }

    private void ActualizarDaño(float daño)
    {
        if (dañoTmp != null)
            dañoTmp.text = $"Daño: {daño:F0}";
    }

    private void ActualizarFuel(float fuelActual, float fuelMaximo)
    {
        if (fuelBarFill != null)
            fuelBarFill.fillAmount = fuelActual / fuelMaximo;
    }

    public void ActualizarEnemigosRestantesUI()
    {
        if (enemigosRestantesTmp != null)
        {
            int cantidad = GameManager.Instance != null ? GameManager.Instance.EnemigosActivos : 0;
            enemigosRestantesTmp.text = "Enemigos restantes: " + cantidad;
        }
    }

}