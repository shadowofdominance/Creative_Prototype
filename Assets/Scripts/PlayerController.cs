using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private Rigidbody playerRb;
    private GameManager gameManager;

    private InputAction moveAction;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float drag = 5f;
    private int objectCount = 0;
    private float xLimit = 10;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem moveParticles;

    [Header("Tilt Settings")]
    [SerializeField] private float tiltAngle = 15f;
    [SerializeField] private float tiltSpeed = 5f;

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
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float horizontalInput = moveInput.x;

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            playerRb.AddForce(Vector3.right * horizontalInput * moveSpeed, ForceMode.Force);

            // Play movement particles
            if (moveParticles != null && !moveParticles.isPlaying)
            {
                moveParticles.Play();
            }

            // Apply tilt based on movement direction
            float targetTilt = -horizontalInput * tiltAngle;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetTilt);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
        }
        else
        {
            playerRb.linearVelocity = new Vector3(0, playerRb.linearVelocity.y, playerRb.linearVelocity.z);

            // Stop movement particles
            if (moveParticles != null && moveParticles.isPlaying)
            {
                moveParticles.Stop();
            }

            // Return to upright position
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
        }

        if (transform.position.x < -xLimit)
        {
            transform.position = new Vector3(-xLimit, transform.position.y, transform.position.z);
        }
        if (transform.position.x > xLimit)
        {
            transform.position = new Vector3(xLimit, transform.position.y, transform.position.z);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GoodObjects"))
        {
            objectCount++;
            Debug.Log("Good Objects Count: " + objectCount);
            gameManager.UpdateScore(1);
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("BadObjects"))
        {
            Debug.Log("Lost a life!");
            gameManager.UpdateLives(1);
            Destroy(collision.gameObject);
        }
    }
}
