using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BlocksController : MonoBehaviour
{
    [Header("Tiles")]
    public RuleTile groundRuleTile;
    public TileBase caveTile;

    [Header("Parámetros de destrucción")]
    [SerializeField] private float blockDestroyTime = 0.1f;

    private Tilemap groundTileMap;
    private Tilemap caveTileMap;

    void Start()
    {
        // Buscar automáticamente los Tilemaps
        Tilemap[] tilemaps = Object.FindObjectsByType<Tilemap>(FindObjectsSortMode.None);
        // Debug.Log($"[BlocksController] Encontrados {allTilemaps.Length} Tilemaps en la escena.");

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

        //  if (groundTileMap == null)
        //  Debug.LogWarning(" No se encontró ningún Tilemap con 'Ground' en su nombre.");

        // if (caveTileMap == null)
        // Debug.LogWarning(" No se encontró ningún Tilemap con 'Cave' en su nombre. Los tiles de fondo no se reemplazarán.");
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
        // else
        //  {
        // Debug.Log($" No se encontró tile en celda {cellPos}, no se destruye nada.");
        //  }
    }

    private IEnumerator DestroyAndReplace(Vector3Int cellPos)
    {
        yield return new WaitForSeconds(blockDestroyTime);

        // Eliminar bloque sólido
        groundTileMap.SetTile(cellPos, null);
        // Debug.Log($" Tile destruido en {cellPos}");

        // Colocar fondo (cave tile)
        if (caveTileMap != null && caveTile != null)
        {
            caveTileMap.SetTile(cellPos, caveTile);
            // Debug.Log($" Tile de cueva colocado en {cellPos}");
        }
        else
        {
            //  if (caveTileMap == null)
            //     Debug.LogWarning(" CaveTileMap es NULL, no se pudo colocar el tile de cueva.");
            //  else if (caveTile == null)
            //    Debug.LogWarning(" CaveTile no asignado en el inspector, no se pudo colocar tile.");
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
