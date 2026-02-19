using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputActionAsset inputActions;
    private Rigidbody playerRb;
    private GameManager gameManager;

    private InputAction moveAction;

    [SerializeField] private float moveSpeed;
    private int objectCount = 0;
    private float xLimit = 8;

    [Header("Particle Effects")]
    [SerializeField] private ParticleSystem moveParticles;

    [Header("Tilt Settings")]
    [SerializeField] private float tiltAngle = 15f;
    [SerializeField] private float tiltSpeed = 5f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip movementSound;
    [SerializeField] private AudioClip goodObjectSound;
    [SerializeField] private AudioClip badObjectSound;

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

        // Reset Rigidbody state to prevent carryover issues on restart
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        float horizontalInput = moveInput.x;

        if (Mathf.Abs(horizontalInput) > 0.01f)
        {
            // Play movement particles
            if (moveParticles != null && !moveParticles.isPlaying)
            {
                moveParticles.Play();
            }

            // Play movement sound
            if (audioSource != null && movementSound != null && !audioSource.isPlaying)
            {
                audioSource.clip = movementSound;
                audioSource.loop = true;
                audioSource.Play();
            }

            // Apply tilt based on movement direction
            float targetTilt = -horizontalInput * tiltAngle;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetTilt);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
        }
        else
        {
            // Stop movement particles
            if (moveParticles != null && moveParticles.isPlaying)
            {
                moveParticles.Stop();
            }

            // Stop movement sound
            if (audioSource != null && audioSource.isPlaying && audioSource.clip == movementSound)
            {
                audioSource.Stop();
            }

            // Return to upright position
            Quaternion targetRotation = Quaternion.Euler(0, 0, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * tiltSpeed);
        }
        // Clamp position to boundaries
        if (transform.position.x < -xLimit)
        {
            transform.position = new Vector3(-xLimit, transform.position.y, transform.position.z);
        }
        if (transform.position.x > xLimit)
        {
            transform.position = new Vector3(xLimit, transform.position.y, transform.position.z);
        }
    }

    void FixedUpdate()
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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GoodObjects"))
        {
            objectCount++;
            Debug.Log("Good Objects Count: " + objectCount);
            gameManager.UpdateScore(1);

            // Play good object sound
            if (goodObjectSound != null)
            {
                AudioSource.PlayClipAtPoint(goodObjectSound, transform.position);
            }

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("BadObjects"))
        {
            Debug.Log("Lost a life!");
            gameManager.UpdateLives(1);

            // Play bad object sound
            if (badObjectSound != null)
            {
                AudioSource.PlayClipAtPoint(badObjectSound, transform.position);
            }

            Destroy(collision.gameObject);
        }
    }
}
