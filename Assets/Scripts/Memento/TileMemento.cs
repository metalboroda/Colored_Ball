using UnityEngine;

namespace Assets.Scripts.Memento
{
  public class TileMemento
  {
    public Vector3 Position { get; private set; }
    public bool IsPainted { get; private set; }

    public TileMemento(Vector3 position, bool isPainted) {
      Position = position;
      IsPainted = isPainted;
    }
  }
}