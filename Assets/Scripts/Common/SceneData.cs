using UnityEngine;

namespace Common
{
    public class SceneData : MonoBehaviour
    {
        [field: SerializeField] public GameOverWindow GameOverWindow { get; private set; }
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public Transform GridTransform { get; private set; }
    }
}