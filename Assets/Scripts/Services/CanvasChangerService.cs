using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Services
{
  public class CanvasChangerService
  {
    private List<GameObject> _canvases = new();

    public CanvasChangerService(List<GameObject> canvases) {
      _canvases = canvases;
    }

    public void ChangeCanvas(GameObject canvasToEnable) {
      foreach (var canvas in _canvases) {
        canvas.SetActive(false);
      }

      canvasToEnable.SetActive(true);
    }
  }
}