using System.Collections.Generic;
using Components;
using UnityEngine;

namespace Infrastructure
{
    public class GameState
    {
        public readonly Dictionary<Vector2Int, int> Cells = new();

        public SignType CurrentSign { get; set; }
        public bool IsGameOver { get; set; }
    }
}