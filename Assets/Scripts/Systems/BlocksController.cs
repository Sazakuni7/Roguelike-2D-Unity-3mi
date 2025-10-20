using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Tilemaps;

public class BlocksController : MonoBehaviour
{
    [Header("Tiles")]
    public RuleTile groundRuleTile;
    public TileBase caveTile;

    [Header("Parámetros de destrucción")]
    [SerializeField] private float blockDestroyTime = 0.1f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip breakSFX;

    private Tilemap groundTileMap;
    private Tilemap caveTileMap;

    void Start()
    {
        // Buscar automáticamente los Tilemaps
        Tilemap[] tilemaps = Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None);

        foreach (Tilemap tm in tilemaps)
        {
            string nameLower = tm.name.ToLower();

            if (nameLower.Contains("ground"))
            {
                groundTileMap = tm;
                //  Debug.Log("GroundTileMap asignado automáticamente: {tm.name}");
            }
            else if (nameLower.Contains("cave"))
            {
                caveTileMap = tm;
                //Debug.Log("CaveTileMap asignado automáticamente: {tm.name}");
            }
        }
    }

    /// Destruye un bloque del mapa de terreno y coloca un tile de cueva en el fondo.
    public void DestroyTileAtWorldPosition(Vector3 worldPos)
    {
        if (groundTileMap == null)
        {
            // Debug.LogWarning("[BlocksController] No hay GroundTileMap asignado.");
            return;
        }

        Vector3Int cellPos = groundTileMap.WorldToCell(worldPos);
        TileBase currentTile = groundTileMap.GetTile(cellPos);

        if (currentTile != null)
        {
            // Debug.Log($" Destruyendo tile en celda {cellPos}...");
            StartCoroutine(DestroyAndReplace(cellPos));
        }
    }

    private IEnumerator DestroyAndReplace(Vector3Int cellPos)
    {
        yield return new WaitForSeconds(blockDestroyTime);

        // Eliminar bloque sólido
        groundTileMap.SetTile(cellPos, null);

        // Reproducir sonido de ruptura
        if (audioSource != null && breakSFX != null)
        {
            audioSource.PlayOneShot(breakSFX);
        }

        // Colocar fondo (cave tile)
        if (caveTileMap != null && caveTile != null)
        {
            caveTileMap.SetTile(cellPos, caveTile);
            // Debug.Log($" Tile de cueva colocado en {cellPos}");
        }


        //  Buscar y destruir un SpawnerTile asociado (si existe)
        Vector3 worldPos = groundTileMap.CellToWorld(cellPos) + new Vector3(0.5f, 1f, 0f); // posición típica del spawner
        float searchRadius = 0.3f;

        Collider2D spawnerHit = Physics2D.OverlapCircle(worldPos, searchRadius);


        if (spawnerHit != null && spawnerHit.CompareTag("SpawnerTile"))
        {
            SpawnerTileManager.Instance?.RegistrarDestruccion(spawnerHit.gameObject);
            Destroy(spawnerHit.gameObject);
        }
        else
        {
  
            // Alternativa sin collider: buscar por distancia (fallback)
            var spawners = Object.FindObjectsOfType<Transform>();
            foreach (var s in spawners)
            {
                if (s.CompareTag("SpawnerTile") && Vector3.Distance(s.position, worldPos) < 0.5f)
                {
                    SpawnerTileManager.Instance?.RegistrarDestruccion(s.gameObject);
                    Destroy(s.gameObject);
                    break;
                }
            }
        }
    }
}
    /// Coloca un tile en la posición del mundo (opcional)
 /*   public void PlaceTileAtWorldPosition(Vector3 worldPos)
    {
        if (groundTileMap == null || blockTile == null) return;

        Vector3Int cellPos = groundTileMap.WorldToCell(worldPos);
        groundTileMap.SetTile(cellPos, blockTile);
    }*/
