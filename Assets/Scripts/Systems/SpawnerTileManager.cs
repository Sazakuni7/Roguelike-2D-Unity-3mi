using UnityEngine;
using System.Collections.Generic;

public class SpawnerTileManager : MonoBehaviour
{
    public static SpawnerTileManager Instance { get; private set; }

    [SerializeField] private int totalSpawnerTiles = 6;
    public int spawnersRestantes;

    private List<GameObject> spawnersActivos = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ActualizarSpawnerList();
    }

    public void ActualizarSpawnerList()
    {
        spawnersActivos.Clear();
        spawnersActivos.AddRange(GameObject.FindGameObjectsWithTag("SpawnerTile"));

        // Quitar duplicados
        spawnersActivos = new List<GameObject>(new HashSet<GameObject>(spawnersActivos));

        // Limitar a totalSpawnerTiles
        if (spawnersActivos.Count > totalSpawnerTiles)
            spawnersActivos = spawnersActivos.GetRange(0, totalSpawnerTiles);

        spawnersRestantes = spawnersActivos.Count;
        totalSpawnerTiles = spawnersActivos.Count; // actualizar total según lo encontrado
        GameUI.Instance.ActualizarSpawnerTilesUI(spawnersRestantes, totalSpawnerTiles);
    }

    public void RegistrarDestruccion(GameObject spawnerTile)
    {
        if (spawnersActivos.Contains(spawnerTile))
        {
            spawnersActivos.Remove(spawnerTile);
            spawnersRestantes = spawnersActivos.Count;

            GameUI.Instance.ActualizarSpawnerTilesUI(spawnersRestantes, totalSpawnerTiles);
            GameManager.Instance.RegistrarSpawnerTileDestruido(spawnersRestantes);

            if (spawnersRestantes <= 0)
            {
                Debug.Log("[SpawnerTileManager] Todos los spawners destruidos.");
                GameManager.Instance.TodosLosSpawnerTilesDestruidos();
            }
        }

    }
}
