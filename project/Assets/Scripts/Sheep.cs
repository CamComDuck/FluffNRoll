using UnityEngine;

public class Sheep : MonoBehaviour {

    private const float BORDER_COLLIE_RADIUS = 20f;
    [SerializeField] private LayerMask borderCollieMask;

    private void Update() {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, BORDER_COLLIE_RADIUS, borderCollieMask);

        Debug.Log(hitColliders.Length > 0);
    }
}
