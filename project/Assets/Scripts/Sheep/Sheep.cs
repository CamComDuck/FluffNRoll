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
    [SerializeField] private AudioClip bounce;

    private SheepSO sheepSO;

    private const float BORDER_COLLIE_RADIUS = 35f;
    private const float MOVE_SPEED = 30f;
    private const string IS_ROLLING_BOOL = "IsRolling";

    private Rigidbody rigidbody;
    private Animator animator;
    private bool hasStood = false;

    private Vector3 directionToRunAway;

    float timeUntilNeutralBa = 0f;
    float timeAtNeutralBa = 0f;
    bool isWaitingToNeutralBa = false;

    float timeUntilScaredBa = 0f;
    float timeAtScaredBa = 0f;
    bool isWaitingToScaredBa = false;

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
            isWaitingToNeutralBa = false;
            isWaitingToScaredBa = true;

        } else { // Not colliding with border collie
            Collider[] hitCollidersLarge = Physics.OverlapSphere(visualPosition, BORDER_COLLIE_RADIUS * 2f, borderCollieMask);
            if (hitCollidersLarge.Length == 0) { // Border collie is far away, so stand up
                if (!hasStood) {
                    rigidbody.linearVelocity = Vector3.zero;
                    rigidbody.angularVelocity = Vector3.zero;
                    if (directionToRunAway != Vector3.zero) {
                        transform.eulerAngles = Vector3.zero;
                    }
                    hasStood = true;
                    isWaitingToScaredBa = false;
                }
            }
        }

        animator.SetBool(IS_ROLLING_BOOL, !IsStanding());
        
    }

    private void Update() {
        if (IsStanding()) {
            if (isWaitingToNeutralBa) {
                timeUntilNeutralBa += Time.deltaTime;
                if (timeUntilNeutralBa >= timeAtNeutralBa) {
                    Vector3 soundPosition = new(transform.position.x + 5, transform.position.y, transform.position.z + 5);
                    AudioSource.PlayClipAtPoint(neutralBa, soundPosition, 1f);
                    isWaitingToNeutralBa = false;
                }
            } else {
                timeUntilNeutralBa = 0;
                timeAtNeutralBa = Random.Range(5, 20);
                isWaitingToNeutralBa = true;
            }
        } else {
            if (isWaitingToScaredBa) {
                timeUntilScaredBa += Time.deltaTime;
                if (timeUntilScaredBa >= timeAtScaredBa) {
                    Vector3 soundPosition = new(transform.position.x + 5, transform.position.y, transform.position.z + 5);
                    AudioSource.PlayClipAtPoint(scaredBa, soundPosition, 1f);
                    isWaitingToScaredBa = false;
                }
            } else {
                timeUntilScaredBa = 0;
                timeAtScaredBa = Random.Range(1.25f, 1.5f);
                isWaitingToScaredBa = true;
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
