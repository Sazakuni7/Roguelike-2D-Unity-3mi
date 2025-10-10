using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spawner : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] GeneracionProcedural generacion;
    [SerializeField] Tilemap groundTilemap;
    [SerializeField] GameObject enemigoTerrestrePrefab;
    [SerializeField] GameObject enemigoAereoPrefab;
    [SerializeField] GameObject jugadorPrefab; // Prefab del jugador

    [Header("Configuración")]
    [SerializeField] int cantidadEnemigosTerrestres = 5;
    [SerializeField] int cantidadEnemigosAereos = 3;
    [SerializeField] float distanciaMinimaJugadorEnemigos = 5f; // Distancia mínima entre jugador y enemigos

    private List<Vector3> puntosValidosTerrestres = new List<Vector3>();
    private List<Vector3> puntosValidosAereos = new List<Vector3>();

    void Start()
    {
        // Obtener lista de puntos válidos del mapa generado
        puntosValidosTerrestres = ObtenerPuntosValidos(1); // Suelo
        puntosValidosAereos = ObtenerPuntosValidos(2); // Cuevas

        // Instanciar enemigos terrestres
        SpawnEnemigos(enemigoTerrestrePrefab, puntosValidosTerrestres, cantidadEnemigosTerrestres);

        // Instanciar enemigos aéreos
        SpawnEnemigos(enemigoAereoPrefab, puntosValidosAereos, cantidadEnemigosAereos);

        // Instanciar jugador
        SpawnJugador();
    }

    void SpawnEnemigos(GameObject prefab, List<Vector3> puntos, int cantidad)
    {
        for (int i = 0; i < cantidad; i++)
        {
            if (puntos.Count == 0) break;

            int randomIndex = Random.Range(0, puntos.Count);
            Vector3 spawnPos = puntos[randomIndex];
            Instantiate(prefab, spawnPos, Quaternion.identity);

            // Remover la posición para no repetir
            puntos.RemoveAt(randomIndex);
        }
    }

    void SpawnJugador()
    {
        Vector3 spawnPos = Vector3.zero;
        bool posicionValida = false;

        foreach (Vector3 punto in puntosValidosTerrestres)
        {
            Collider2D[] colisiones = Physics2D.OverlapCircleAll(punto, distanciaMinimaJugadorEnemigos);
            bool hayEnemigosCerca = false;

            foreach (Collider2D colision in colisiones)
            {
                if (colision.CompareTag("Enemy"))
                {
                    hayEnemigosCerca = true;
                    break;
                }
            }

            if (!hayEnemigosCerca)
            {
                spawnPos = punto;
                posicionValida = true;
                break;
            }
        }

        if (posicionValida)
        {
            GameObject jugador = Instantiate(jugadorPrefab, spawnPos, Quaternion.identity);

            // Asignar el jugador como objetivo a los enemigos
            ChaseGround[] enemigosTerrestres = FindObjectsOfType<ChaseGround>();
            foreach (var enemigo in enemigosTerrestres)
            {
                enemigo.SetJugador(jugador.transform);
            }

            ChaseAir[] enemigosAereos = FindObjectsOfType<ChaseAir>();
            foreach (var enemigo in enemigosAereos)
            {
                enemigo.SetJugador(jugador.transform);
            }

            // Configurar la cámara
            CinemachineCamera cinemachineCam = FindObjectOfType<CinemachineCamera>();
            if (cinemachineCam != null)
            {
                cinemachineCam.Follow = jugador.transform;
            }
            else
            {
                Debug.LogError("No se encontró una CinemachineCamera en la escena.");
            }
        }
        else
        {
            Debug.LogError("No se encontró un lugar válido para spawnear al jugador.");
        }
    }

    void OnDrawGizmos()
    {
        if (puntosValidosTerrestres != null)
        {
            Gizmos.color = Color.green;
            foreach (Vector3 punto in puntosValidosTerrestres)
            {
                Gizmos.DrawSphere(punto, 0.2f);
            }
        }

        if (puntosValidosAereos != null)
        {
            Gizmos.color = Color.blue;
            foreach (Vector3 punto in puntosValidosAereos)
            {
                Gizmos.DrawSphere(punto, 0.2f);
            }
        }
    }

    List<Vector3> ObtenerPuntosValidos(int tipoTerreno)
    {
        List<Vector3> lista = new List<Vector3>();
        int[,] mapa = generacion.GetMap();

        for (int x = 0; x < mapa.GetLength(0); x++)
        {
            for (int y = 0; y < mapa.GetLength(1) - 1; y++)
            {
                if (mapa[x, y] == tipoTerreno && mapa[x, y + 1] == 0)
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