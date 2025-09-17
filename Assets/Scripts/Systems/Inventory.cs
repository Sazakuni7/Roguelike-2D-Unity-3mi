using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> objetos = new Dictionary<string, int>();

    public void AgregarObjeto(string nombreObjeto, int cantidad = 1)
    {
        if (objetos.ContainsKey(nombreObjeto))
        {
            objetos[nombreObjeto] += cantidad;
        }
        else
        {
            objetos[nombreObjeto] = cantidad;
        }

        Debug.Log($"Objeto agregado: {nombreObjeto} (Cantidad: {objetos[nombreObjeto]})");
    }

    public void EliminarObjeto(string nombreObjeto, int cantidad = 1)
    {
        if (objetos.ContainsKey(nombreObjeto))
        {
            objetos[nombreObjeto] -= cantidad;
            if (objetos[nombreObjeto] <= 0)
            {
                objetos.Remove(nombreObjeto);
            }

            Debug.Log($"Objeto eliminado: {nombreObjeto}");
        }
    }

    public void MostrarInventario()
    {
        Debug.Log("Inventario:");
        foreach (var objeto in objetos)
        {
            Debug.Log($"{objeto.Key}: {objeto.Value}");
        }
    }
}