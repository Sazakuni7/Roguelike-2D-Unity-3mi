using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float vida = 5f;
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform puntoDeDisparo; // Punto desde donde se dispara
    [SerializeField] private float tiempoEntreDisparos = 0.5f;

    private float tiempoUltimoDisparo;
    private Vector2 direccionDisparo = Vector2.right; // Dirección inicial (derecha)
    private Vector3 escalaInicial; // Escala inicial del jugador

    private void Awake()
    {
        // Guardar la escala inicial del jugador
        escalaInicial = transform.localScale;
    }

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

    private void Update()
    {
        // Cambiar la dirección del disparo según la entrada del jugador
        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow))) // Izquierda
        {
            direccionDisparo = Vector2.left;
            transform.localScale = new Vector3(-escalaInicial.x, escalaInicial.y, escalaInicial.z); // Voltear el sprite del jugador
        }
        else if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow))) // Derecha
        {
            direccionDisparo = Vector2.right;
            transform.localScale = escalaInicial; // Restaurar el sprite del jugador
        }

        // Disparar si se presiona la tecla Q
        if (Input.GetKey(KeyCode.Q) && Time.time >= tiempoUltimoDisparo + tiempoEntreDisparos)
        {
            Disparar();
            tiempoUltimoDisparo = Time.time;
        }
    }

    private void Disparar()
    {
        // Instanciar el proyectil
        GameObject proyectil = Instantiate(proyectilPrefab, puntoDeDisparo.position, Quaternion.identity);

        // Ajustar la dirección del proyectil
        Projectile scriptProyectil = proyectil.GetComponent<Projectile>();
        if (scriptProyectil != null)
        {
            scriptProyectil.SetDireccion(direccionDisparo);
        }
    }
}