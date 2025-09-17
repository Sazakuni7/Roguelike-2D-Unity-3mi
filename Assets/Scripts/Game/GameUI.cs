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
    [SerializeField] private TMP_Text da�oTmp; // Nueva referencia para el da�o

    private int enemigosRestantes;

    private void OnEnable()
    {
        GameEvents.OnGameOver += MostrarGameOver;
        GameEvents.OnVictory += MostrarVictoria;
        Enemy.OnEnemyDestroyed += ActualizarEnemigosRestantes;

        // Suscribirse al evento de vida, experiencia, nivel y da�o del jugador
        Jugador jugador = FindObjectOfType<Jugador>();
        if (jugador != null)
        {
            jugador.OnVidaCambiada += ActualizarVida;
            jugador.OnExperienciaCambiada += ActualizarExperiencia;
            jugador.OnDa�oCambiado += ActualizarDa�o;

            // Inicializar la UI con los valores actuales
            ActualizarVida(jugador.GetVida());
            ActualizarExperiencia(jugador.DatosProgresion.experienciaActual, jugador.DatosProgresion.experienciaNecesaria);
            ActualizarNivel(jugador.DatosProgresion.nivel);
            ActualizarDa�o(jugador.DatosProgresion.da�oBase);
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

        // Cancelar la suscripci�n al evento de vida, experiencia, nivel y da�o del jugador
        Jugador jugador = FindObjectOfType<Jugador>();
        if (jugador != null)
        {
            jugador.OnVidaCambiada -= ActualizarVida;
            jugador.OnExperienciaCambiada -= ActualizarExperiencia;
            jugador.OnDa�oCambiado -= ActualizarDa�o;
        }
    }

    private void MostrarGameOver()
    {
        if (mensajeMuerteTmp != null)
        {
            mensajeMuerteTmp.gameObject.SetActive(true);
            mensajeMuerteTmp.text = "�Has muerto!";
        }
    }

    private void MostrarVictoria()
    {
        if (mensajeVictoriaTmp != null)
        {
            mensajeVictoriaTmp.gameObject.SetActive(true);
            mensajeVictoriaTmp.text = "�Has ganado!";
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

    private void ActualizarDa�o(float da�o)
    {
        if (da�oTmp != null)
        {
            da�oTmp.text = $"Da�o: {da�o:F0}";
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