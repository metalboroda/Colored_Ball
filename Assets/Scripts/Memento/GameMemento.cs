using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Memento
{
    public class GameMemento
    {
        public Vector3 BallPosition { get; private set; }
        public List<TileMemento> TileStates { get; private set; }
        public List<Vector3> PaintedTilesPositions { get; private set; }

        public GameMemento(Vector3 ballPosition, List<TileMemento> tileStates, List<Vector3> paintedTilesPositions) {
            BallPosition = ballPosition;
            TileStates = tileStates;
            PaintedTilesPositions = paintedTilesPositions;
        }
    }
}