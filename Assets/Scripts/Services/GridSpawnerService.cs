using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Grid
{
  public class GridSpawnerService
  {
    private Vector3 _gridSize;
    private GameObject _tilePrefab;
    private GameObject _tilesContainer;
    private int _paintedTilesCount;

    public GridSpawnerService(Vector3 gridSize, GameObject tilePrefab, GameObject tilesContainer, int paintedTilesCount) {
      _gridSize = gridSize;
      _tilePrefab = tilePrefab;
      _tilesContainer = tilesContainer;
      _paintedTilesCount = paintedTilesCount;
    }

    public Tile[,,] SpawnGrid(out List<Tile> spawnedTiles, List<Tile> paintedTiles) {
      Tile[,,] grid = new Tile[(int)_gridSize.x, (int)_gridSize.y, (int)_gridSize.z];

      spawnedTiles = new List<Tile>();

      for (int x = 0; x < _gridSize.x; x++) {
        for (int y = 0; y < _gridSize.y; y++) {
          for (int z = 0; z < _gridSize.z; z++) {
            Tile spawnedTile = Object.Instantiate(
                _tilePrefab, new Vector3(x, y, z), Quaternion.identity, _tilesContainer.transform).GetComponent<Tile>();

            spawnedTiles.Add(spawnedTile);
            grid[x, y, z] = spawnedTile;
          }
        }
      }

      PaintConnectedTiles(grid, spawnedTiles, paintedTiles);

      return grid;
    }

    private void PaintConnectedTiles(Tile[,,] grid, List<Tile> spawnedTiles, List<Tile> paintedTiles) {
      int totalTiles = spawnedTiles.Count;
      int tilesToPaint = _paintedTilesCount;

      if (tilesToPaint == 0) return;

      int startX = Random.Range(0, (int)_gridSize.x);
      int startY = Random.Range(0, (int)_gridSize.y);
      int startZ = Random.Range(0, (int)_gridSize.z);

      Tile startTile = grid[startX, startY, startZ];

      startTile.PaintTile();
      paintedTiles.Add(startTile);

      HashSet<Tile> paintedTilesSet = new HashSet<Tile> { startTile };
      List<Tile> frontier = new List<Tile> { startTile };

      while (paintedTilesSet.Count < tilesToPaint && frontier.Count > 0) {
        Tile currentTile = frontier[Random.Range(0, frontier.Count)];

        frontier.Remove(currentTile);

        foreach (Tile neighbor in GetNeighbors(currentTile, grid)) {
          if (!neighbor.IsPainted) {
            neighbor.PaintTile();
            paintedTilesSet.Add(neighbor);
            paintedTiles.Add(neighbor);
            frontier.Add(neighbor);

            if (paintedTilesSet.Count >= tilesToPaint) break;
          }
        }
      }
    }

    private IEnumerable<Tile> GetNeighbors(Tile tile, Tile[,,] grid) {
      Vector3 pos = tile.transform.position;
      List<Tile> neighbors = new List<Tile>();

      int x = Mathf.FloorToInt(pos.x);
      int y = Mathf.FloorToInt(pos.y);
      int z = Mathf.FloorToInt(pos.z);

      if (x > 0) neighbors.Add(grid[x - 1, y, z]);
      if (x < grid.GetLength(0) - 1) neighbors.Add(grid[x + 1, y, z]);
      if (y > 0) neighbors.Add(grid[x, y - 1, z]);
      if (y < grid.GetLength(1) - 1) neighbors.Add(grid[x, y + 1, z]);
      if (z > 0) neighbors.Add(grid[x, y, z - 1]);
      if (z < grid.GetLength(2) - 1) neighbors.Add(grid[x, y, z + 1]);

      return neighbors;
    }
  }
}