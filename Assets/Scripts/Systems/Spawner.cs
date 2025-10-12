using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject jugadorPrefab;
    public GameObject enemyGroundPrefab;
    public GameObject enemyAirPrefab;

    [Header("Spawn Config")]
    public float spawnInterval = 3f;
    public int maxEnemigosActivos = 6;

    [HideInInspector] public bool playerSpawned = false;

    private List<Vector3> posicionesSpawner = new List<Vector3>();

    private GeneracionProcedural generacion;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void InicializarPosiciones()
    {
        posicionesSpawner.Clear();

        // Buscar todos los SpawnerTiles en la escena
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("SpawnerTile");
        foreach (GameObject tile in tiles)
        {
            posicionesSpawner.Add(tile.transform.position);
        }

        generacion = Object.FindFirstObjectByType<GeneracionProcedural>();

        if (!playerSpawned)
            SpawnJugadorCercanoAlTerreno();

        // Spawnear enemigos cada intervalo
        InvokeRepeating(nameof(SpawnEnemigos), 2f, spawnInterval);
    }

    private void SpawnJugadorCercanoAlTerreno()
    {
        if (jugadorPrefab == null || generacion == null)
        {
           // Debug.LogWarning("No se puede spawnear al jugador: faltan referencias.");
            return;
        }

        int[,] mapa = generacion.GetMap();
        int width = mapa.GetLength(0);
        int height = mapa.GetLength(1);

        List<Vector3> posiblesPos = new List<Vector3>();

        for (int x = 3; x < width - 3; x++)
        {
            for (int y = 1; y < height - 2; y++) // empieza cerca del suelo
            {
                if (mapa[x, y] == 0 && mapa[x, y - 1] == 1)
                    posiblesPos.Add(new Vector3(x, y, 0));
            }
        }

        if (posiblesPos.Count > 0)
        {
            Vector3 spawnPos = posiblesPos[Random.Range(0, posiblesPos.Count)];
            GameObject jugadorGO = Instantiate(jugadorPrefab, spawnPos, Quaternion.identity);
            Jugador jugador = jugadorGO.GetComponent<Jugador>();

            GameManager.Instance.SetJugador(jugador);
            playerSpawned = true;

            // Notificar a todo el sistema que el jugador fue creado
            GameEvents.TriggerPlayerSpawned(jugador);

           // Debug.Log("Jugador spawneado correctamente y notificado a GameUI.");
        }
      /*  else
        {
            Debug.LogWarning("No se encontró una posición válida para spawnear al jugador.");
        }*/
    }

    private void SpawnEnemigos()
    {
        // Evitar spawnear si ya hay max enemigos en GameManager
        if (GameManager.Instance.EnemigosActivos >= maxEnemigosActivos) return;

        int alturaMaxima = generacion.GetMap().GetLength(1);
        int umbralAereo = Mathf.RoundToInt(alturaMaxima * 0.65f);

        // Mezclar posiciones
        Shuffle(posicionesSpawner);

        foreach (Vector3 pos in posicionesSpawner)
        {
            if (GameManager.Instance.EnemigosActivos >= maxEnemigosActivos) break;

            bool esAereo = pos.y > umbralAereo;
            string tagPool = esAereo ? "EnemyAir" : "EnemyGround";

            GameObject enemyGO = ObjectPooler.Instance.GetPooledObject(tagPool);
            if (enemyGO == null)
            {
                // Intentar con el otro tipo si hay pool disponible
                tagPool = esAereo ? "EnemyGround" : "EnemyAir";
                enemyGO = ObjectPooler.Instance.GetPooledObject(tagPool);
            }

            if (enemyGO != null)
            {
                enemyGO.transform.position = pos + Vector3.up * 0.5f;
                enemyGO.SetActive(true);

                Enemy enemyScript = enemyGO.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    // Registrar en GameManager
                    GameManager.Instance.RegistrarEnemigo(enemyScript);

                    // Suscribirse a la muerte del enemigo
                    enemyScript.OnEnemyDestroyedInstance += (e) =>
                    {
                        GameManager.Instance.DesregistrarEnemigo(e);
                        SpawnEnemigos(); // Intentar reemplazar inmediatamente
                        GameUI.Instance.ActualizarEnemigosRestantesUI(); // Actualizar UI
                    };

                    // Asignar jugador a los scripts de persecución
                    EnemyAirPathing chaseAir = enemyGO.GetComponent<EnemyAirPathing>();
                    if (chaseAir != null)
                        chaseAir.SetJugador(GameManager.Instance.GetJugador().transform);

                    EnemyGroundPathing chaseGround = enemyGO.GetComponent<EnemyGroundPathing>();
                    if (chaseGround != null)
                        chaseGround.SetJugador(GameManager.Instance.GetJugador().transform);
                }

                // Actualizar UI al spawnear
                GameUI.Instance.ActualizarEnemigosRestantesUI();
            }
        }
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
