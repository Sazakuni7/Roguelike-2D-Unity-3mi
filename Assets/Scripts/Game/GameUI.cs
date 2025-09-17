using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TMP_Text vidaTmp;
    [SerializeField] private TMP_Text enemigosRestantesTmp;
    [SerializeField] private TMP_Text mensajeMuerteTmp;
    [SerializeField] private TMP_Text mensajeVictoriaTmp;
    [SerializeField] private TMP_Text experienciaTmp; // Nueva referencia para la experiencia
    [SerializeField] private TMP_Text nivelTmp; // Nueva referencia para el nivel
    [SerializeField] private TMP_Text dañoTmp; // Nueva referencia para el daño

    private int enemigosRestantes;

    private void OnEnable()
    {
        GameEvents.OnGameOver += MostrarGameOver;
        GameEvents.OnVictory += MostrarVictoria;
        Enemy.OnEnemyDestroyed += ActualizarEnemigosRestantes;

        // Suscribirse al evento de vida, experiencia, nivel y daño del jugador
        Jugador jugador = FindObjectOfType<Jugador>();
        if (jugador != null)
        {
            jugador.OnVidaCambiada += ActualizarVida;
            jugador.OnExperienciaCambiada += ActualizarExperiencia;
            jugador.OnDañoCambiado += ActualizarDaño;

            // Inicializar la UI con los valores actuales
            ActualizarVida(jugador.GetVida());
            ActualizarExperiencia(jugador.DatosProgresion.experienciaActual, jugador.DatosProgresion.experienciaNecesaria);
            ActualizarNivel(jugador.DatosProgresion.nivel);
            ActualizarDaño(jugador.DatosProgresion.dañoBase);
        }

        // Inicializar el conteo de enemigos
        enemigosRestantes = GameObject.FindGameObjectsWithTag("Enemy").Length;
        ActualizarEnemigosRestantesUI();
    }

    private void OnDisable()
    {
        GameEvents.OnGameOver -= MostrarGameOver;
        GameEvents.OnVictory -= MostrarVictoria;
        Enemy.OnEnemyDestroyed -= ActualizarEnemigosRestantes;

        // Cancelar la suscripción al evento de vida, experiencia, nivel y daño del jugador
        Jugador jugador = FindObjectOfType<Jugador>();
        if (jugador != null)
        {
            jugador.OnVidaCambiada -= ActualizarVida;
            jugador.OnExperienciaCambiada -= ActualizarExperiencia;
            jugador.OnDañoCambiado -= ActualizarDaño;
        }
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
        {
            vidaTmp.text = "Vida: " + nuevaVida.ToString("F0") + "%";
        }
    }

    private void ActualizarExperiencia(float experienciaActual, float experienciaParaNivel2)
    {
        if (experienciaTmp != null)
        {
            float experienciaFaltante = experienciaParaNivel2 - experienciaActual;
            experienciaTmp.text = $"Experiencia: {experienciaActual:F0}/{experienciaParaNivel2:F0} (Faltan {experienciaFaltante:F0})";

            Debug.Log($"Actualizando experiencia en la UI: {experienciaActual}/{experienciaParaNivel2} (Faltan {experienciaFaltante})");
        }
    }

    private void ActualizarNivel(int nivel)
    {
        if (nivelTmp != null)
        {
            nivelTmp.text = $"Nivel: {nivel}";
        }
    }

    private void ActualizarDaño(float daño)
    {
        if (dañoTmp != null)
        {
            dañoTmp.text = $"Daño: {daño:F0}";
        }
    }

    private void ActualizarEnemigosRestantes(float _)
    {
        enemigosRestantes--;
        ActualizarEnemigosRestantesUI();

        if (enemigosRestantes <= 0)
        {
            GameEvents.TriggerVictory();
        }
    }

    private void ActualizarEnemigosRestantesUI()
    {
        if (enemigosRestantesTmp != null)
        {
            enemigosRestantesTmp.text = "Enemigos restantes: " + enemigosRestantes;
        }
    }
}