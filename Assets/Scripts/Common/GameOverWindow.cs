using System;
using Components;
using Infrastructure;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Common
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] private TMP_Text _winnerSign;
        [SerializeField] private Button _restart;

        private void OnEnable() => _restart.onClick.AddListener(RestartGame);
        
        private void OnDisable() => _restart.onClick.RemoveListener(RestartGame);

        public void SetWinner(SignType type)
        {
            _winnerSign.text = type switch
            {
                SignType.Cross => "Cross is Winner!",
                SignType.Circle => "Circle is Winner!",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        public void SetDraw() => _winnerSign.text = "Draw";

        public void Show(GameState gameState)
        {
            gameObject.SetActive(true);
            gameState.IsGameOver = true;
        }

        private void RestartGame() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}