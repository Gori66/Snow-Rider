using UnityEngine;
// Wichtig: Das neue Input System importieren
using UnityEngine.InputSystem; 

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float sideSpeed = 10f;
    public float rotationSpeed = 50f;
    public float maxX = 8f;

    private float currentRotationY = 0f;

    void Update()
    {
        float moveInput = 0f;

        // Abfrage für das neue Input System (Tastatur)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                moveInput = -1f;
            }
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                moveInput = 1f;
            }
        }

        if (moveInput != 0)
        {
            // 1. Bewegung nach links/rechts
            Vector3 newPosition = transform.position + new Vector3(moveInput * sideSpeed * Time.deltaTime, 0, 0);
            newPosition.x = Mathf.Clamp(newPosition.x, -maxX, maxX);
            transform.position = newPosition;

            // 2. Rotation anpassen
            currentRotationY += moveInput * rotationSpeed * Time.deltaTime;
            currentRotationY = Mathf.Clamp(currentRotationY, -35f, 35f);
        }

        // Rotation auf das Objekt anwenden
        transform.rotation = Quaternion.Euler(0, currentRotationY, 0);
    }
}