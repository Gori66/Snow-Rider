using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public MapManager mapManager;
    
    private bool isDead = false;

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Hazard"))
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("Game Over! Spieler ist gestorben.");

        // Hier später: Game-Over-UI anzeigen, Zeitlupe, Sound, etc.
        // Fürs Erste stoppen wir einfach die Bewegung:
        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        if (mapManager != null)
        {
            mapManager.enabled = false;
        }
    }
}