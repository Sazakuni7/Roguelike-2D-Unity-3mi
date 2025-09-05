using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] GeneracionProcedural generacion; // referencia al script de generación procedural
    [SerializeField] Tilemap groundTilemap;           // tilemap de suelo
    [SerializeField] GameObject enemigoPrefab;

    [Header("Configuración")]
    [SerializeField] int cantidadEnemigos = 5;

    private List<Vector3> puntosValidos = new List<Vector3>();

    void Start()
    {

        // Obtener lista de puntos válidos del mapa generado
        puntosValidos = ObtenerPuntosValidos();

        // Instanciar enemigos en posiciones aleatorias de la lista
        for (int i = 0; i < cantidadEnemigos; i++)
        {
            if (puntosValidos.Count == 0) break;

            int randomIndex = Random.Range(0, puntosValidos.Count);
            Vector3 spawnPos = puntosValidos[randomIndex];
            Instantiate(enemigoPrefab, spawnPos, Quaternion.identity);

            // opcional: remover la posición para no repetir
            puntosValidos.RemoveAt(randomIndex);
        }
    }

    List<Vector3> ObtenerPuntosValidos()
    {
        List<Vector3> lista = new List<Vector3>();
        int[,] mapa = generacion.GetMap(); // hay que exponer un getter en GeneracionProcedural

        for (int x = 0; x < mapa.GetLength(0); x++)
        {
            for (int y = 0; y < mapa.GetLength(1) - 1; y++)
            {
                // condición: suelo con espacio libre arriba
                if (mapa[x, y] == 1 && mapa[x, y + 1] == 0)
                {
                    Vector3Int cellPos = new Vector3Int(x, y + 1, 0);
                    Vector3 worldPos = groundTilemap.GetCellCenterWorld(cellPos);
                    lista.Add(worldPos);
                }
            }
        }
        return lista;
    }
}