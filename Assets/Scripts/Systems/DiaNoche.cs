using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GeneradorDiaNoche : MonoBehaviour
{
    [SerializeField] private Camera camara;
    [SerializeField] private Color nocheColor;
    [SerializeField] private Light2D luz2D;


    [SerializeField][Range(1, 128)] private int duracionDia;
    [SerializeField][Range(1, 24)] private int dias;

    private Color diaColor;


    void Start()
    {
        diaColor = camara.backgroundColor;
        StartCoroutine(CambiarColor(duracionDia));
    }

    IEnumerator CambiarColor(float tiempo)
    {
        Color colorDestinoFondo = camara.backgroundColor == diaColor ? nocheColor : diaColor;
        Color colorDestinoLuz = luz2D.color != Color.white ? Color.white : nocheColor;
        float duracionCiclo = tiempo * 0.6f;
        float duracionCambio = tiempo * 0.4f;

        for (int i = 0; i < dias; i++)
        {
            yield return new WaitForSeconds(duracionCiclo);

            float tiempoTranscurrido = 0;

            while (tiempoTranscurrido < duracionCambio)
            {
                tiempoTranscurrido += Time.deltaTime;
                float t = tiempoTranscurrido / duracionCambio;

                float smoothT = Mathf.SmoothStep(0f, 1f, t);

                camara.backgroundColor = Color.Lerp(camara.backgroundColor, colorDestinoFondo, smoothT);
                luz2D.color = Color.Lerp(luz2D.color, colorDestinoLuz, smoothT);

                yield return null;
            }

            colorDestinoLuz = luz2D.color != Color.white ? Color.white : nocheColor;
            colorDestinoFondo = camara.backgroundColor == diaColor ? nocheColor : diaColor;

        }
    }
}