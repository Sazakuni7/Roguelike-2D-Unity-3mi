using UnityEngine;
using UnityEngine.Audio;

public class MenuOpciones : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void cambiarVolumenMusica(float volumen)
    {
      audioMixer.SetFloat("VolumenMusica", volumen);    
    }
    public void cambiarVolumenEfectos(float volumen)
    {
        audioMixer.SetFloat("VolumenEfectos", volumen);
    }
}

