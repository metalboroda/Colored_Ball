using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Grid
{
  public class GridCoinSpawnerService
  {
    private float _coinPosibility;
    private GameObject _coinPrefab;

    public GridCoinSpawnerService(float coinPosibility, GameObject coinPrefab) {
      _coinPosibility = coinPosibility;
      _coinPrefab = coinPrefab;
    }

    public void SpawnCoins(List<Tile> tiles) {
      foreach (Tile tile in tiles) {
        if (!tile.IsPainted && Random.value <= _coinPosibility) {
          Object.Instantiate(_coinPrefab, tile.transform.position, Quaternion.identity, tile.transform);
        }
      }
    }
  }
}