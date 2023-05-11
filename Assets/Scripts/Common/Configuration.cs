using UnityEngine;
using Views;

namespace Common
{
    [CreateAssetMenu]
    public class Configuration : ScriptableObject
    {
        [field: SerializeField] public uint LevelHeight { get; private set; }
        [field: SerializeField] public uint LevelWidth { get; private set; }
        [field: SerializeField] public uint ChainLength { get; private set; }
        [field: SerializeField] public CellView CellView { get; private set; }
        [field: SerializeField] public CircleSingView CircleSingView { get; private set; }
        [field: SerializeField] public CrossSingView CrossSingView { get; private set; }
        [field: SerializeField] public Vector2 Offset { get; private set; }
    }
}