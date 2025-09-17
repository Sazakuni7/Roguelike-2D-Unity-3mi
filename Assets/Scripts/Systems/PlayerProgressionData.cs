using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProgressionData", menuName = "Game/Player Progression Data")]
public class PlayerProgressionData : ScriptableObject
{
    [Header("Estad�sticas del Jugador")]
    public int nivel;
    public float experienciaActual;
    public float experienciaNecesaria;
    public float vidaMaxima;
    public float da�oBase;

    public void AgregarExperiencia(float experiencia)
    {
        experienciaActual += experiencia;
        Debug.Log($"Experiencia actual: {experienciaActual}/{experienciaNecesaria}");

        if (experienciaActual >= experienciaNecesaria)
        {
            SubirNivel();
        }
    }

    private void SubirNivel()
    {
        nivel++;
        experienciaActual -= experienciaNecesaria;
        experienciaNecesaria *= 1.2f; // Incremento progresivo
        vidaMaxima += 10f; // Incremento de vida
        da�oBase += 2f; // Incremento de da�o

        Debug.Log($"�Nivel aumentado! Nuevo nivel: {nivel}, Experiencia restante: {experienciaActual}, Nueva experiencia necesaria: {experienciaNecesaria}");
    }
}