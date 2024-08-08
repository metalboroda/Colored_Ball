using Assets.Scripts.EventBus;
using Assets.Scripts.Memento;
using Assets.Scripts.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Grid
{
  public class GridManager : MonoBehaviour
  {
    [Header("Settings")]
    [SerializeField] private Vector3 _gridSize;

    [Header("Player Settings")]
    [SerializeField] private float _playerMovementSpeed = 2.5f;

    [Header("Tile Settings")]
    [SerializeField] private int _paintedTilesCount;

    [Header("Coin Settings")]
    [Range(0f, 1f)]
    [SerializeField] private float _coinPosibility;
    [Space]
    [SerializeField] private GameObject _coinPrefab;

    [Header("Objects")]
    [SerializeField] private GameObject _tilePrefab;
    [Space]
    [SerializeField] private GameObject _playerPrefab;

    [Header("References")]
    [SerializeField] private GameObject _tilesContainer;

    private Tile[,,] _grid;
    private List<Tile> _spawnedTiles = new List<Tile>();
    private List<Tile> _paintedTiles = new List<Tile>();

    private GridSpawnerService _gridSpawner;
    private GridPlayerSpawnerService _playerSpawner;
    private GridCoinSpawnerService _coinSpawner;

    private EventBinding<EventStructs.TilePainted> _tilePaintedEvent;
    private EventBinding<EventStructs.NowhereToGo> _nowhereToGoEvent;

    private void Awake() {
      _gridSpawner = new GridSpawnerService(_gridSize, _tilePrefab, _tilesContainer, _paintedTilesCount);
      _playerSpawner = new GridPlayerSpawnerService(_playerMovementSpeed);
      _coinSpawner = new GridCoinSpawnerService(_coinPosibility, _coinPrefab);

      _grid = _gridSpawner.SpawnGrid(out _spawnedTiles, _paintedTiles);
      _coinSpawner.SpawnCoins(_spawnedTiles);
    }

    private void OnEnable() {
      _tilePaintedEvent = new EventBinding<EventStructs.TilePainted>(OnTilePainted);
      _nowhereToGoEvent = new EventBinding<EventStructs.NowhereToGo>(OnNowhereToGo);
    }

    private void OnDisable() {
      _tilePaintedEvent.Remove(OnTilePainted);
      _nowhereToGoEvent.Remove(OnNowhereToGo);
    }

    private void Start() {
      SpawnPlayer();
    }

    public bool IsPositionWithinBounds(Vector3 position) {
      return position.x >= 0 && position.x < _gridSize.x && position.z >= 0 && position.z < _gridSize.z;
    }

    public bool IsTilePainted(Vector3 position) {
      int x = Mathf.FloorToInt(position.x);
      int y = Mathf.FloorToInt(position.y);
      int z = Mathf.FloorToInt(position.z);

      if (x >= 0 && x < _gridSize.x && y >= 0 && y < _gridSize.y && z >= 0 && z < _gridSize.z) {
        return _grid[x, y, z].IsPainted;
      }
      return false;
    }

    public void PaintTileAtPosition(Vector3 position) {
      int x = Mathf.FloorToInt(position.x);
      int y = Mathf.FloorToInt(position.y);
      int z = Mathf.FloorToInt(position.z);

      if (x >= 0 && x < _gridSize.x && y >= 0 && y < _gridSize.y && z >= 0 && z < _gridSize.z) {
        Tile tile = _grid[x, y, z];

        tile.PaintTile();

        if (!_paintedTiles.Contains(tile)) {
          _paintedTiles.Add(tile);
        }
      }
    }

    private void SpawnPlayer() {
      string selectedBallName;

      if (ES3.KeyExists(SettingsHashes.BallName)) {
        selectedBallName = ES3.Load<string>(SettingsHashes.BallName);
      }
      else {
        selectedBallName = "Player_Ball_1";
      }

      _playerSpawner.SpawnPlayer(selectedBallName, _gridSize, transform);
    }

    private void OnTilePainted(EventStructs.TilePainted tilePainted) {
      if (!_paintedTiles.Contains(tilePainted.Tile)) {
        _paintedTiles.Add(tilePainted.Tile);
      }

      if (_paintedTiles.Count == _spawnedTiles.Count)
        EventBus<EventStructs.WinLose>.Raise(new EventStructs.WinLose {
          Win = true
        });
    }

    private void OnNowhereToGo(EventStructs.NowhereToGo nowhereToGo) {
      if (_paintedTiles.Count < _spawnedTiles.Count) {
        EventBus<EventStructs.WinLose>.Raise(new EventStructs.WinLose {
          Win = false
        });
      }
    }

    public new Coroutine StartCoroutine(IEnumerator routine) {
      return base.StartCoroutine(routine);
    }

    public new void StopCoroutine(Coroutine routine) {
      base.StopCoroutine(routine);
    }

    public List<TileMemento> CreateMemento(out List<Vector3> paintedTilesPositions) {
      List<TileMemento> tileMementos = new List<TileMemento>();

      paintedTilesPositions = new List<Vector3>();

      foreach (Tile tile in _spawnedTiles) {
        tileMementos.Add(tile.CreateMemento());

        if (tile.IsPainted) {
          paintedTilesPositions.Add(tile.transform.position);
        }
      }

      return tileMementos;
    }

    public void RestoreMemento(List<TileMemento> mementos, List<Vector3> paintedTilesPositions) {
      _paintedTiles.Clear();

      for (int i = 0; i < mementos.Count; i++) {
        _spawnedTiles[i].RestoreMemento(mementos[i]);

        if (mementos[i].IsPainted) {
          _paintedTiles.Add(_spawnedTiles[i]);
        }
      }

      foreach (var position in paintedTilesPositions) {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);
        int z = Mathf.FloorToInt(position.z);

        if (x >= 0 && x < _gridSize.x && y >= 0 && y < _gridSize.y && z >= 0 && z < _gridSize.z) {
          var tile = _grid[x, y, z];

          if (tile.IsPainted && !_paintedTiles.Contains(tile)) {
            _paintedTiles.Add(tile);
          }
          else if (!tile.IsPainted && _paintedTiles.Contains(tile)) {
            _paintedTiles.Remove(tile);
          }
        }
      }
    }
  }
}