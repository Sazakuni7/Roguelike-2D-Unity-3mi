using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject jugadorPrefab;
    public GameObject enemyGroundPrefab;
    public GameObject enemyAirPrefab;
    public GameObject particleEffectPrefab;

    [Header("Spawn Config")]
    public float spawnInterval = 3f;
    public int maxEnemigosActivos = 6;

    [HideInInspector] public bool playerSpawned = false;
    public bool puedeSpawnear = true;
    private bool evitarSpawnJugador = false;

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

        GameObject[] tiles = GameObject.FindGameObjectsWithTag("SpawnerTile");
        foreach (GameObject tile in tiles)
        {
            posicionesSpawner.Add(tile.transform.position);

            if (particleEffectPrefab != null)
            {
                GameObject particleEffect = Instantiate(particleEffectPrefab, tile.transform.position, Quaternion.identity, tile.transform);
                ParticleSystem ps = particleEffect.GetComponent<ParticleSystem>();
                if (ps != null) ps.transform.SetParent(tile.transform);
            }
        }

        generacion = Object.FindFirstObjectByType<GeneracionProcedural>();

        if (!playerSpawned && !evitarSpawnJugador)
            SpawnJugadorCercanoAlTerreno();

        SpawnerTileManager.Instance?.ActualizarSpawnerList();

        InvokeRepeating(nameof(SpawnEnemigos), 2f, spawnInterval);
    }

    private void SpawnJugadorCercanoAlTerreno()
    {
        if (jugadorPrefab == null || generacion == null) return;

        int[,] mapa = generacion.GetMap();
        int width = mapa.GetLength(0);
        int height = mapa.GetLength(1);

        List<Vector3> posiblesPos = new List<Vector3>();

        for (int x = 3; x < width - 3; x++)
        {
            for (int y = 1; y < height - 2; y++)
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

            GameEvents.TriggerPlayerSpawned(jugador);
        }
    }

    public Vector3 GetSpawnPosicionJugador()
    {
        if (generacion == null) generacion = Object.FindFirstObjectByType<GeneracionProcedural>();

        int[,] mapa = generacion.GetMap();
        int width = mapa.GetLength(0);
        int height = mapa.GetLength(1);

        List<Vector3> posiblesPos = new List<Vector3>();

        for (int x = 3; x < width - 3; x++)
        {
            for (int y = 1; y < height - 2; y++)
            {
                if (mapa[x, y] == 0 && mapa[x, y - 1] == 1)
                    posiblesPos.Add(new Vector3(x, y, 0));
            }
        }

        if (posiblesPos.Count > 0)
            return posiblesPos[Random.Range(0, posiblesPos.Count)];

        // Si no hay posiciones válidas, devolvemos Vector3.zero
        return Vector3.zero;
    }

    private void SpawnEnemigos()
    {
        if (!puedeSpawnear || GameManager.Instance.EnemigosActivos >= maxEnemigosActivos) return;

        int alturaMaxima = generacion.GetMap().GetLength(1);
        int umbralAereo = Mathf.RoundToInt(alturaMaxima * 0.65f);

        Shuffle(posicionesSpawner);

        foreach (Vector3 pos in posicionesSpawner)
        {
            if (GameManager.Instance.EnemigosActivos >= maxEnemigosActivos) break;

            bool esAereo = pos.y > umbralAereo;
            string tagPool = esAereo ? "EnemyAir" : "EnemyGround";

            GameObject enemyGO = ObjectPooler.Instance.GetPooledObject(tagPool);
            if (enemyGO == null)
            {
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
                    // Escalar vida según nivel
                    float multiplicadorVida = 1f + (GameManager.Instance.NivelJuego - 1) * 0.5f;
                    enemyScript.Inicializar(multiplicadorVida);

                    GameManager.Instance.RegistrarEnemigo(enemyScript);

                    enemyScript.OnEnemyDestroyedInstance += (e) =>
                    {
                        GameManager.Instance.DesregistrarEnemigo(e);
                        SpawnEnemigos();
                        GameUI.Instance.ActualizarEnemigosRestantesUI();
                    };

                    EnemyAirPathing chaseAir = enemyGO.GetComponent<EnemyAirPathing>();
                    if (chaseAir != null) chaseAir.SetJugador(GameManager.Instance.GetJugador().transform);

                    EnemyGroundPathing chaseGround = enemyGO.GetComponent<EnemyGroundPathing>();
                    if (chaseGround != null) chaseGround.SetJugador(GameManager.Instance.GetJugador().transform);
                }

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
