using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BorderCollie : MonoBehaviour {

    public static BorderCollie Instance { get; private set; }

    [SerializeField] private List<MeshRenderer> meshRenderers;
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform colorbindPrefab;

    private const float MOVE_SPEED = 12f;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;

    private Rigidbody rigidbody;

    private void Awake() {
        if (Instance != null) Debug.LogError("There is more than one BorderCollie instance!");
        Instance = this;

        if (ColorblindMode.Instance == null) {
            Instantiate(colorbindPrefab);
        }

        rigidbody = GetComponent<Rigidbody>();

        foreach (MeshRenderer meshRenderer in meshRenderers) {
            meshRenderer.enabled = false;
        }
    }

    private void Update() {
        HandleInput();
    }

    private void FixedUpdate() {
        HandleMovement();
    }

    private void HandleInput() {
        horizontalInput = GameInput.GetMovementVectorNormalized().x;
        verticalInput = GameInput.GetMovementVectorNormalized().y;
    }

    private void HandleMovement() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rigidbody.AddForce(10f * MOVE_SPEED * moveDirection.normalized, ForceMode.Force);
    }

    public Transform GetTransform() {
        return transform;
    }

}
