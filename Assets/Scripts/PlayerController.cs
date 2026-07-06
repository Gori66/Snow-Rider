using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float forwardSpeed = 10f; // Wie stark seitlich basierend auf Blickrichtung
    public float rotationSpeed = 50f;
    public float maxX = 8f; // Grenzen der Piste nach links und rechts
    public float maxAngle = 90f; // Maximale Neigung der Kamera nach links und rechts

    [Header("Jump Settings")]
    public float jumpForce = 8f;
    public float gravity = -20f;
    public float groundY = 2f; // Die Y-Position, auf der der Spieler "steht" (deine bisherige Kamera-Höhe)

    private float currentRotationY = 0f;
    private float verticalVelocity = 0f;
    private bool isGrounded = true;

    void Update()
    {
        HandleSideMovement();
        HandleJump();
    }
    
    void HandleSideMovement()
    {
        float moveInput = 0f;

        // Abfrage für das neue Input System
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            moveInput = -1f;
        }
        else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            moveInput = 1f;
        }

        if (moveInput != 0)
        {
            // Rotation anpassen
            currentRotationY += moveInput * rotationSpeed * Time.deltaTime;
            currentRotationY = Mathf.Clamp(currentRotationY, -maxAngle, maxAngle);
        }

        // Rotation anwenden
        transform.rotation = Quaternion.Euler(0, currentRotationY, 0);

        // Bewegung basierend auf der Blickrichtung
        Vector3 forwardDir = transform.forward;
        float lateralMovement = forwardDir.x * forwardSpeed * Time.deltaTime;
        
        Vector3 newPosition = transform.position + new Vector3(lateralMovement, 0, 0);
        newPosition.x = Mathf.Clamp(newPosition.x, -maxX, maxX);
        transform.position = newPosition;
    }

    void HandleJump()
    {
        // Sprung auslösen mit W oder Pfeil nach oben
        bool jumpPressed = Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame;

        if (jumpPressed && isGrounded)
        {
            verticalVelocity = jumpForce;
            isGrounded = false;
        }

        // Schwerkraft anwenden
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 pos = transform.position;
        pos.y += verticalVelocity * Time.deltaTime;

        // Boden-Kollision prüfen
        if (pos.y <= groundY)
        {
            pos.y = groundY;
            verticalVelocity = 0f;
            isGrounded = true;
        }

        transform.position = pos;
    }
}