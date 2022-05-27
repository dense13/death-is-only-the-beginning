using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        // NOTE: collisions are set in the collision matrix
        Destroy(other.gameObject);
    }
}
