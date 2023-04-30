using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Create a static instance of the PlayerController
    public static PlayerController instance;

    public float m_Speed;                       // Movement speed
    public float m_JumpHeight;             // Height of jumps 
    public float m_gravityScale = 10f;     // Used for more controlled jumps   
    public float bounceForce = 8f;      // Player bounces off the enemy head 

    private Vector3 inputDirection;      // Player input direction

    // Reference to the Unity Character Controller
    public CharacterController controller;

    // Reference to the Camera
    new private Camera camera;

    // Child player model
    public GameObject playerModel;
    public float rotateSpeed;

    public Animator animator;

    // Variables to control player being knocked back by damage
    public float knockBackLength = .5f;
    private float knockbackCounter;
    public Vector2 knockbackPower;  // Only an x & y value
    public bool knockedBack;

    private void Awake()
    {
        // Set the PlayerController to this instance
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // If the player is not being thrown back from damage
        if (!knockedBack)
        {
            float yStore = inputDirection.y;  // Local variable to smoothe the fall
            //  Add the input direction to the current position to calculate where to move
            inputDirection = (transform.forward * Input.GetAxisRaw("Vertical"))
                + (transform.right * Input.GetAxisRaw("Horizontal"));
            inputDirection.Normalize();
            inputDirection = inputDirection * m_Speed;
            inputDirection.y = yStore;

            // Apply jump if button is pressed & player is grounded
            if (controller.isGrounded)
            {
                inputDirection.y = Physics.gravity.y;
                if (Input.GetButtonDown("Jump"))
                {
                    // Apply the jump force
                    inputDirection.y = m_JumpHeight;
                }
            }
            // Adjust for gravity as the character controller doesn't do that (-9.81 mps^2)
            inputDirection.y += Physics.gravity.y * Time.deltaTime * m_gravityScale;

            // Tell the character controller to move the player by the amount
            // adjusted for framerate
            controller.Move(inputDirection * Time.deltaTime);

            // Rotate the player to face where the camera is looking
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                transform.rotation = Quaternion.Euler(0f, camera.transform.rotation.eulerAngles.y, 0f);
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(inputDirection.x,
                    0f, inputDirection.z));
                // Rotate the child not the parent to maintain transform position
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation,
                    newRotation, rotateSpeed * Time.deltaTime);
            }
        }

        // Move the player back when damage has occurred 
        if (knockedBack)
        {
            knockbackCounter -= Time.deltaTime;

            float yStore = inputDirection.y;
            inputDirection = playerModel.transform.forward * -knockbackPower.x;
            inputDirection.y = yStore;

            if (controller.isGrounded)
            {
                inputDirection.y = 0f;
            }

            inputDirection.y += Physics.gravity.y * Time.deltaTime * m_gravityScale;

            controller.Move(inputDirection * Time.deltaTime);

            if (knockbackCounter <= 0)
            {
                knockedBack = false;
            }
        }

        // Set the float and boolean to facilitate animations
        animator.SetFloat("Speed", Mathf.Abs(inputDirection.x) + Mathf.Abs(inputDirection.z));
        // Character Controller isGrounded tells us when player is on the ground
        animator.SetBool("Grounded", controller.isGrounded);
    }

    // Player does a little bounce when killing an enemy
    public void Bounce()
    {
        inputDirection.y = bounceForce;
        controller.Move(inputDirection * Time.deltaTime);
    }

    // Method to knock player back from damage
    public void Knockback()
    {
        knockedBack = true;
        knockbackCounter = knockBackLength;
        inputDirection.y = knockbackPower.y;
        controller.Move(inputDirection * Time.deltaTime);
    }
}
