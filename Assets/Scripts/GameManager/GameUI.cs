using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Jugador jugador;
    [SerializeField] private TMP_Text vidaTmp;
    [SerializeField] private TMP_Text enemigosRestantesTmp;
    [SerializeField] private TMP_Text mensajeMuerteTmp;
    [SerializeField] private TMP_Text mensajeVictoriaTmp;

    private int enemigosRestantes = 0;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        // Inicializar la UI
        ActualizarVida();
        ActualizarEnemigosRestantes();

        if (mensajeMuerteTmp != null)
            mensajeMuerteTmp.gameObject.SetActive(false);

        if (mensajeVictoriaTmp != null)
            mensajeVictoriaTmp.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Actualizar enemigos restantes
        int cantidad = GameObject.FindGameObjectsWithTag("Enemy").Length;
        SetEnemigosRestantes(cantidad);

        // Actualizar vida
        ActualizarVida();
    }

    public void SetEnemigosRestantes(int cantidad)
    {
        enemigosRestantes = cantidad;
        ActualizarEnemigosRestantes();

        if (enemigosRestantes == 0 && mensajeVictoriaTmp != null)
        {
            mensajeVictoriaTmp.gameObject.SetActive(true);
            mensajeVictoriaTmp.text = "¡Has ganado!";
        }
    }

    private void ActualizarVida()
    {
        if (jugador != null)
        {
            vidaTmp.text = "Vida: " + jugador.GetVida().ToString("F0") + "%";

            if (jugador.GetVida() <= 0 && mensajeMuerteTmp != null)
            {
                mensajeMuerteTmp.gameObject.SetActive(true);
                mensajeMuerteTmp.text = "¡Has muerto!";
                Time.timeScale = 0f; // detener el juego al morir
            }
        }
    }

    private void ActualizarEnemigosRestantes()
    {
        enemigosRestantesTmp.text = "Enemigos Restantes: " + enemigosRestantes;
    }
}
