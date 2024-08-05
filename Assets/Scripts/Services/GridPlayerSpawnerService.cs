using Assets.Scripts.Ball;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Assets.Scripts.Grid
{
  public class GridPlayerSpawnerService
  {
    private float _playerMovementSpeed;

    public GridPlayerSpawnerService(float playerMovementSpeed) {
      _playerMovementSpeed = playerMovementSpeed;
    }

    public void SpawnPlayer(string ballName, Vector3 gridSize, Transform parent) {
      string assetPath = $"Assets/Prefab/Ball/{ballName}.prefab";
      Addressables.LoadAssetAsync<GameObject>(assetPath).Completed += handle => OnPlayerSpawned(handle, parent);
    }

    private void OnPlayerSpawned(AsyncOperationHandle<GameObject> handle, Transform parent) {
      if (handle.Status == AsyncOperationStatus.Succeeded) {
        GameObject spawnedPlayerObject = Object.Instantiate(handle.Result, parent);
        BallMovement ballController = spawnedPlayerObject.GetComponent<BallMovement>();

        ballController.SpawnInit(_playerMovementSpeed, spawnedPlayerObject.transform.parent.GetComponent<GridManager>());

        Vector3 spawnPosition = spawnedPlayerObject.transform.position;
        GridManager gridManager = spawnedPlayerObject.transform.parent.GetComponent<GridManager>();

        gridManager.PaintTileAtPosition(spawnPosition);
      }
      else {
        Debug.LogError("Failed to load the player prefab.");
      }
    }
  }
}