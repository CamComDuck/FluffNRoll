using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sheep : MonoBehaviour {

    [SerializeField] private LayerMask borderCollieMask;
    [SerializeField] private ParticleSystem standParticles;
    [SerializeField] private List<MeshRenderer> sheepColorMeshRenderers;

    [SerializeField] private AudioClip happyBa;
    [SerializeField] private AudioClip scaredBa;
    [SerializeField] private AudioClip neutralBa;


    private SheepSO sheepSO;

    private const float BORDER_COLLIE_RADIUS = 35f;
    private const float MOVE_SPEED = 30f;
    private const string IS_ROLLING_BOOL = "IsRolling";

    private Rigidbody rigidbody;
    private Animator animator;
    private bool hasStood = false;

    Vector3 directionToRunAway;

    float timeUntilBa = 0f;
    float timeAtBa = 0f;
    bool isWaitingToBa = false;

    private void Awake() {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate() {
        Vector3 visualPosition = new(transform.position.x + 5, transform.position.y, transform.position.z + 5);
        Collider[] hitCollidersSmall = Physics.OverlapSphere(visualPosition, BORDER_COLLIE_RADIUS, borderCollieMask);
        if (hitCollidersSmall.Length > 0) { // Is colliding with Border Collie to run away
            BorderCollie borderCollie = (BorderCollie)hitCollidersSmall[0].GetComponentInParent<BorderCollie>();
            if (borderCollie == null) Debug.LogError("Sheep hit something not a BorderCollie");

            Vector3 borderColliePosition = borderCollie.transform.position;

            directionToRunAway = visualPosition - borderColliePosition;
            directionToRunAway.Normalize();
            directionToRunAway.y = 0;

            rigidbody.AddForce(10f * MOVE_SPEED * directionToRunAway, ForceMode.Force);
            hasStood = false;
            isWaitingToBa = false;

        } else { // Not colliding with border collie
            Collider[] hitCollidersLarge = Physics.OverlapSphere(visualPosition, BORDER_COLLIE_RADIUS * 1.2f, borderCollieMask);
            if (hitCollidersLarge.Length == 0) { // Border collie is far away, so stand up
                if (!hasStood) {
                    rigidbody.linearVelocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                    if (directionToRunAway != Vector3.zero) {
                        transform.eulerAngles = Vector3.zero;
                    }
                    hasStood = true;
                }
            }
        }

        animator.SetBool(IS_ROLLING_BOOL, !IsStanding());
        
    }

    private void Update() {
        if (IsStanding()) {
            if (isWaitingToBa) {
                timeUntilBa += Time.deltaTime;
                if (timeUntilBa >= timeAtBa) {
                    Vector3 soundPosition = new(transform.position.x + 5, transform.position.y, transform.position.z + 5);
                    AudioSource.PlayClipAtPoint(neutralBa, soundPosition, 1f);
                    isWaitingToBa = false;
                }
            } else {
                timeUntilBa = 0;
                timeAtBa = Random.Range(5, 20);
                isWaitingToBa = true;
            }
        }
    }

    public void ArrivedAtBarn() {
        standParticles.Play();
        Vector3 soundPosition = new(transform.position.x + 5, transform.position.y, transform.position.z + 5);
        AudioSource.PlayClipAtPoint(happyBa, soundPosition, 1f);
    }

    public void SetSheepSO(SheepSO newSO) {
        sheepSO = newSO;
        foreach (MeshRenderer meshRenderer in sheepColorMeshRenderers) {
            List<Material> settingMaterials = new() {
                sheepSO.GetMaterial()
            };
            meshRenderer.SetMaterials(settingMaterials);
        }
        var rend = standParticles.GetComponent<ParticleSystemRenderer>();
        rend.material = sheepSO.GetMaterial();
    }

    public bool IsStanding() {
        return hasStood;
    }

    public SheepSO GetSheepSO() {
        return sheepSO;
    }
}
