using UnityEngine;

public class SledShatter : MonoBehaviour
{
    [Header("Explosions-Einstellungen")]
    public float explosionForce = 8f;
    public float explosionRadius = 5f;
    public float upwardModifier = 2f;
    public float torqueForce = 5f;

    void Start()
    {
        // Für jedes Kind-Fragment: Rigidbody + Collider hinzufügen und Explosion anwenden
        foreach (Transform fragment in transform)
        {
            Rigidbody rb = fragment.gameObject.GetComponent<Rigidbody>();

            // Explosion anwenden - Ursprung ist ungefähr die Mitte des Schlittens
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardModifier);

            // Zusätzliche zufällige Rotation, damit es chaotischer aussieht
            rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
        }
    }
}