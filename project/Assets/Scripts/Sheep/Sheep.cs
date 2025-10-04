using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour {

    [SerializeField] private LayerMask borderCollieMask;
    [SerializeField] private ParticleSystem standParticles;
    [SerializeField] private List<MeshRenderer> sheepColorMeshRenderers;

    [SerializeField] private SheepSO sheepSO; // Will be set on instantiation

    private const float BORDER_COLLIE_RADIUS = 20f;
    private const float MOVE_SPEED = 30f;
    private const string IS_ROLLING_BOOL = "IsRolling";

    private Rigidbody rigidbody;
    private Animator animator;
    private bool hasStood = false;

    Vector3 directionToRunAway;


    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start() {
        foreach (MeshRenderer meshRenderer in sheepColorMeshRenderers) {
            List<Material> settingMaterials = new() {
                sheepSO.GetMaterial()
            };
            meshRenderer.SetMaterials(settingMaterials);
        }
        var rend = standParticles.GetComponent<ParticleSystemRenderer>();
        rend.material = sheepSO.GetMaterial();
    }

    private void FixedUpdate() {
        Collider[] hitCollidersSmall = Physics.OverlapSphere(transform.position, BORDER_COLLIE_RADIUS, borderCollieMask);
        if (hitCollidersSmall.Length > 0) { // Is colliding with Border Collie to run away
            BorderCollie borderCollie = (BorderCollie)hitCollidersSmall[0].GetComponentInParent<BorderCollie>();
            if (borderCollie == null) Debug.LogError("Sheep hit something not a BorderCollie");

            Vector3 borderColliePosition = borderCollie.transform.position;

            directionToRunAway = transform.position - borderColliePosition;
            directionToRunAway.Normalize();

            rigidbody.AddForce(10f * MOVE_SPEED * directionToRunAway, ForceMode.Force);
            hasStood = false;

        } else { // Not colliding with border collie
            Collider[] hitCollidersLarge = Physics.OverlapSphere(transform.position, BORDER_COLLIE_RADIUS * 2f, borderCollieMask);
            if (hitCollidersLarge.Length == 0) { // Border collie is far away, so stand up
                if (!hasStood) {
                    rigidbody.linearVelocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                    transform.forward = directionToRunAway;
                    hasStood = true;

                }
            }
        }

        animator.SetBool(IS_ROLLING_BOOL, !IsStanding());
    }

    public void ArrivedAtBarn() {
        standParticles.Play();
        
    }

    public bool IsStanding() {
        return hasStood;
    }

    public SheepSO GetSheepSO() {
        return sheepSO;
    }
}
