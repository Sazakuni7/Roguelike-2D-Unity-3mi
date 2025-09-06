using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float vida = 5f;

    public void ModificarVida(float puntos)
    {
        vida += puntos;

        // Asegurarnos de que la vida no sea negativa
        if (vida < 0)
        {
            vida = 0;
        }

        Debug.Log(EstasVivo());

        // Verificar si el jugador está vivo
        if (!EstasVivo())
        {
            Morir();
        }
    }

    private bool EstasVivo()
    {
        return vida > 0;
    }

    public float GetVida()
    {
        return vida;
    }

    private void Morir()
    {
        Debug.Log("El jugador ha muerto.");
        // Aquí puedo elegir entre detener el juego o hacer que el jugador desaparezca
       // gameObject.SetActive(false); // Hace que el jugador desaparezca
        // Alternativamente, puedo detener el juego con:
         Time.timeScale = 0f;
    }
}