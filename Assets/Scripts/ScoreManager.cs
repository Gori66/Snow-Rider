using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("UI Referenz")]
    public TextMeshProUGUI scoreText;

    [Header("Einstellungen")]
    public float scoreMultiplier = 1f; // Wie viele Punkte pro Meter zurückgelegter Strecke

    private float currentScore = 0f;
    private bool isRunning = true;

    void Update()
    {
        if (!isRunning) return;

        // Score erhöht sich proportional zur Basisgeschwindigkeit der Welt
        currentScore += Time.deltaTime * scoreMultiplier;
        UpdateScoreDisplay();
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = Mathf.FloorToInt(currentScore).ToString();
        }
    }

    public void StopScore()
    {
        isRunning = false;
    }

    public int GetFinalScore()
    {
        return Mathf.FloorToInt(currentScore);
    }
}