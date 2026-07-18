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
            if (rb == null)
            {
                rb = fragment.gameObject.AddComponent<Rigidbody>();
            }

            // Sicherstellen, dass hier ECHTE Physik greift (nicht kinematisch!)
            rb.isKinematic = false;

            Collider col = fragment.gameObject.GetComponent<Collider>();
            if (col == null)
            {
                fragment.gameObject.AddComponent<BoxCollider>();
            }

            // Explosion anwenden - Ursprung ist ungefähr die Mitte des Schlittens
            rb.AddExplosionForce(explosionForce, transform.position, explosionRadius, upwardModifier);

            // Zusätzliche zufällige Rotation, damit es chaotischer aussieht
            rb.AddTorque(Random.insideUnitSphere * torqueForce, ForceMode.Impulse);
        }
        
        Destroy(gameObject, 5f); // Löscht das gesamte Fragment-Objekt nach 5 Sekunden
    }
}