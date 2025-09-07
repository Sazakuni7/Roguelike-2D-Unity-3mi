using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Jugador jugador; // Referencia al script del jugador
    [SerializeField] private TMP_Text vidaTmp;
    [SerializeField] private TMP_Text enemigosRestantesTmp;
    [SerializeField] private TMP_Text mensajeMuerteTmp; // Texto para mostrar un mensaje de muerte
    [SerializeField] private TMP_Text mensajeVictoriaTmp; // Texto para mostrar un mensaje de victoria

    private int enemigosRestantes = 0;

    private void Awake()
    {
        Debug.Log("Time.timeScale al inicio: " + Time.timeScale);
        Time.timeScale = 1f;
    }
    private void Start()
    {
        // Inicializar la interfaz
        ActualizarVida();
        ActualizarEnemigosRestantes();

        // Asegurarnos de que los mensajes de muerte y victoria estén ocultos al inicio
        if (mensajeMuerteTmp != null)
        {
            mensajeMuerteTmp.gameObject.SetActive(false);
        }

        if (mensajeVictoriaTmp != null)
        {
            mensajeVictoriaTmp.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        // Actualizar la vida en cada frame
        ActualizarVida();

        // Actualizar enemigos en cada frame
        int cantidad = GameObject.FindGameObjectsWithTag("Enemy").Length;
        SetEnemigosRestantes(cantidad);
    }

    public void SetEnemigosRestantes(int cantidad)
    {
        enemigosRestantes = cantidad;
        ActualizarEnemigosRestantes();

        // Mostrar mensaje de victoria si no quedan enemigos
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

            // Mostrar mensaje de muerte si la vida llega a 0
            if (jugador.GetVida() <= 0 && mensajeMuerteTmp != null)
            {
                mensajeMuerteTmp.gameObject.SetActive(true);
                mensajeMuerteTmp.text = "¡Has muerto!";
            }
        }
    }

    private void ActualizarEnemigosRestantes()
    {
        enemigosRestantesTmp.text = "Enemigos Restantes: " + enemigosRestantes;
    }
}