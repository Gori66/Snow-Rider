using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("UI Referenzen")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;

    public void ShowGameOver(int finalScore)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "Punkte: " + finalScore.ToString();
        }
    }

    public void RestartGame()
    {
        // Lädt die aktuelle Szene neu -> kompletter Reset
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}