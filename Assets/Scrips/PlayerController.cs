using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private Rigidbody playerRb;

    private InputAction moveAction;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float drag = 5f;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }
    private void Awake()
    {
        moveAction = inputActions.FindAction("Move");
        playerRb = GetComponent<Rigidbody>();
        playerRb.linearDamping = drag;
    }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float horizontalInput = moveInput.x;

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            playerRb.AddForce(Vector3.right * horizontalInput * moveSpeed, ForceMode.Force);
        }
        else
        {
            playerRb.linearVelocity = new Vector3(0, playerRb.linearVelocity.y, playerRb.linearVelocity.z);
        }
    }
}
