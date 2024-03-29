using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool crouching;
    private float crouchTimer;
    private bool lerpCrouch;
    private bool sprinting;
    private float timer;
    private float originalCameraYPosition;

    public Camera firstPersonCamera;
    public float bobbingSpeed = 14f;
    public float bobbingAmount = 0.05f;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 1.2f;

    [SerializeField]
    private GameObject store;
    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        originalCameraYPosition = firstPersonCamera.transform.localPosition.y;
    }
    
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
            {
                controller.height = Mathf.Lerp(controller.height, 1, p);
            }
            else
            {
                controller.height = Mathf.Lerp(controller.height, 2, p);
            }

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }

    public void ProcessMove(Vector2 input)
    {
        if (!store.activeSelf)
        {
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
            playerVelocity.y += gravity * Time.deltaTime;

            if (isGrounded && playerVelocity.y < 0)
                playerVelocity.y = -2f;
            controller.Move(playerVelocity * Time.deltaTime);

            if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
            {
                //Player is moving
                timer += Time.deltaTime * bobbingSpeed;
                firstPersonCamera.transform.localPosition =
                    new Vector3(0, originalCameraYPosition + Mathf.Sin(timer) * bobbingAmount, 0);
            }
            else
            {
                //Idle
                timer = 0;
                firstPersonCamera.transform.localPosition = new Vector3(0,
                    Mathf.Lerp(firstPersonCamera.transform.localPosition.y, originalCameraYPosition,
                        Time.deltaTime * bobbingSpeed), 0);
            }
        }
    }

    public void Jump()
    {
        if (isGrounded && !store.activeSelf)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void Crouch()
    {
        if (!store.activeSelf)
        {
            crouching = !crouching;
            crouchTimer = 0;
            lerpCrouch = true;
        }
    }

    public void Sprint()
    {
        if (!store.activeSelf)
        {
            sprinting = !sprinting;
            if (sprinting)
            {
                speed = 8;
            }
            else
            {
                speed = 5;
            }
        }
    }
}
