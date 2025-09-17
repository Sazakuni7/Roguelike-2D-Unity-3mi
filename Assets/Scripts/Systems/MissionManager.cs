using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    private Queue<string> misiones = new Queue<string>();

    public void AgregarMision(string mision)
    {
        misiones.Enqueue(mision);
        Debug.Log($"Misi�n agregada: {mision}");
    }

    public void CompletarMision()
    {
        if (misiones.Count > 0)
        {
            string misionCompletada = misiones.Dequeue();
            Debug.Log($"Misi�n completada: {misionCompletada}");
        }
        else
        {
            Debug.Log("No hay misiones pendientes.");
        }
    }

    public void MostrarMisiones()
    {
        Debug.Log("Misiones pendientes:");
        foreach (var mision in misiones)
        {
            Debug.Log(mision);
        }
    }
}