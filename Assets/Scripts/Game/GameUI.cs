using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TMP_Text vidaTmp;
    [SerializeField] private TMP_Text enemigosRestantesTmp;
    [SerializeField] private TMP_Text mensajeMuerteTmp;
    [SerializeField] private TMP_Text mensajeVictoriaTmp;

    private int enemigosRestantes;

    private void OnEnable()
    {
        GameEvents.OnGameOver += MostrarGameOver;
        GameEvents.OnVictory += MostrarVictoria;
        Enemy.OnEnemyDestroyed += ActualizarEnemigosRestantes;

        // Suscribirse al evento de vida del jugador
        Jugador jugador = FindObjectOfType<Jugador>();
        if (jugador != null)
        {
            jugador.OnVidaCambiada += ActualizarVida;
            ActualizarVida(jugador.GetVida()); // Inicializar la UI con la vida actual
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

        // Cancelar la suscripción al evento de vida del jugador
        Jugador jugador = FindObjectOfType<Jugador>();
        if (jugador != null)
        {
            jugador.OnVidaCambiada -= ActualizarVida;
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

    private void ActualizarEnemigosRestantes()
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