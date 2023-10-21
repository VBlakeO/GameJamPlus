using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement m_Instance;

    [Header("===PlayerMovement===")]
    public bool cantMove = false;
    [SerializeField] private  bool cantLook = false;
    [SerializeField] private  bool cantJump = false;

    [Header("Movement")]
    [SerializeField] private float speed = 0.30f;
    private float moveSmoothTime = 0.30f;
    private Vector3 velocity = Vector3.zero;
    public Vector2 CurrentDir {get; set;}     
    private Vector2 targetDir = Vector2.zero;
    private Vector2 currentDirVelocity = Vector2.zero;

    [Header("===Vision===")]
    public float mouseSensitivity;
    private float headPitch = 0.0f;


    [Header("Jump")]
    [SerializeField] private  LayerMask layerMask = 1;
    [SerializeField] private  float jumpForce = 5.0f;
    [SerializeField] private float radius = 0.61f;
    [SerializeField] private float range = 0.55f;
    [SerializeField] private float gravity = -13.0f;
    [SerializeField] private bool isJumping = false;
    private float velocityY = 0.0f;

    [Header("Component")]
    [SerializeField] private Transform head = null;
    [SerializeField] private Camera cameraFps = null;
    private CharacterController controller = null;

    [Header("Slope")]
    private float slopeForce = 5.0f;
    private float slopeForceRayLength = 2.0f;

    private void Awake()
    {
        m_Instance = this;
        controller = GetComponent<CharacterController>();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Time.timeScale == 0)
            return;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded() && !cantJump)
            Jump();

        Movement();
    }

    private void LateUpdate()
    {
        if (Time.timeScale == 0)
            return;

        if (cantLook)
            return;

        Vector2 targetMouseDelta = new(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        headPitch -= targetMouseDelta.y * mouseSensitivity;
        headPitch = Mathf.Clamp(headPitch, -85, 75);

        head.localEulerAngles = Vector3.right * headPitch;
        transform.Rotate(mouseSensitivity * targetMouseDelta.x * Vector3.up);
    }

    private void Movement()
    {
        if (IsGrounded() && !isJumping)
            velocityY = 0;

        velocityY += gravity * Time.deltaTime;

        if (!cantMove)
        {
            targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            targetDir.Normalize();

            CurrentDir = Vector2.SmoothDamp(CurrentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

            velocity = (transform.forward * CurrentDir.y + transform.right * CurrentDir.x) * speed + Vector3.up * velocityY;

            controller.Move(velocity * Time.deltaTime);

            if ((Mathf.Abs(targetDir.x) > 0 || Mathf.Abs(targetDir.y) > 0) && OnSlope())
                controller.Move(Vector3.down * controller.height / 2 * slopeForce * Time.deltaTime);
        }
        else
        {
            velocity = Vector3.up * velocityY;
            
            if (controller.enabled)
                controller.Move(velocity * Time.deltaTime);
        }
    }

    private void Jump()
    {
        isJumping = true;
        velocityY = jumpForce;

        StartCoroutine(BackToGround());
    }

    public void PausePlayer(bool active)
    {
        cantLook = active;
        cantMove = active;
        cantJump = active;
        CurrentDir = Vector2.zero;

        if(active)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    private IEnumerator BackToGround()
    {
        WaitForSeconds wfs = new(0.1f);
        yield return wfs;
        isJumping = false;
    }

    bool OnSlope()
    {
        if (isJumping)
            return false;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, controller.height / 2 * slopeForceRayLength))
            if (hit.normal != Vector3.up)
                return true;

        return false;
    }

    public bool IsGrounded()
    {
        return Physics.SphereCast(transform.position, radius, -transform.up, out _, range, layerMask);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position - transform.up * range, radius);
    }
}