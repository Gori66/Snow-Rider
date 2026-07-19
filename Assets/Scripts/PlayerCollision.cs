using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("Referenzen")]
    public MapManager mapManager;
    public GameOverManager gameOverManager;
    public ScoreManager scoreManager;
    
    [Header("Shatter-Effekt")]
    public GameObject sledFragmentsPrefab;
    public GameObject sledVisual; // das normale, sichtbare Schlitten-Modell (zum Ausblenden)

    [Header("Third-Person Crash-Kamera")]
    public Transform cameraTransform; // Main Camera
    public Vector3 crashCameraLocalOffset = new Vector3(0f, 3f, -6f); // hinter & über dem Schlitten
    public float crashCameraLocalRotationX = 15f; // leicht nach unten geneigt
    public float cameraTransitionSpeed = 3f;
    public float crashViewDuration = 1f; // wie lange man den Crash von außen sieht, bevor Game Over erscheint
    
    private bool isDead = false;

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Hazard"))
        {
            StartCoroutine(DieSequence());
        }
    }

    IEnumerator DieSequence()
    {
        isDead = true;

        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null) controller.enabled = false;

        if (mapManager != null) mapManager.enabled = false;
        if (scoreManager != null) scoreManager.StopScore();

        // Normales Schlitten-Modell ausblenden
        if (sledVisual != null) sledVisual.SetActive(false);

        // Fragmente an der aktuellen Position/Rotation spawnen
        if (sledFragmentsPrefab != null)
        {
            Instantiate(sledFragmentsPrefab, transform.position, transform.rotation);
        }

        // Kamera sanft zur Third-Person-Crash-Position bewegen
        float elapsed = 0f;
        Vector3 startLocalPos = cameraTransform.localPosition;
        Quaternion startLocalRot = cameraTransform.localRotation;

        // Zielrotation: leicht nach unten geneigt, damit man den Schlitten/die Trümmer gut sieht
        Quaternion targetLocalRot = Quaternion.Euler(crashCameraLocalRotationX, 0f, 0f);

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime * cameraTransitionSpeed;
            cameraTransform.localPosition = Vector3.Lerp(startLocalPos, crashCameraLocalOffset, elapsed);
            cameraTransform.localRotation = Quaternion.Lerp(startLocalRot, targetLocalRot, elapsed);
            // Warten bis zum nächsten Frame
            yield return null;
        }

        // Kurz in der Third-Person-Ansicht verweilen, damit man die Explosion sehen kann
        yield return new WaitForSeconds(crashViewDuration);

        // Game Over UI anzeigen
        int finalScore = scoreManager != null ? scoreManager.GetFinalScore() : 0;
        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver(finalScore);
        }
    }
}