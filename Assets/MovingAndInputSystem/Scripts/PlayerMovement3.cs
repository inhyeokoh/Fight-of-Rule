using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement3 : MonoBehaviour
{
    [SerializeField]
    private InputAction mouseClickAction;
    [SerializeField]
    public float playerSpeed = 10f;
    [SerializeField]
    private float rotationSpeed = 3f;

    private Camera mainCamera;
    private Coroutine coroutine;
    private Vector3 targetPosition;

    private const float RAY_DISTANCE = 2f;
    private RaycastHit slopeHit;
    // private int groundLayer = 1 << LayerMask.NameToLayer("Ground"); // 원래 만들어놓은 땅 레이어 쓰면 됨.

    #region #경사 체크 변수
    [Header("경사 지형 검사")]
    [SerializeField, Tooltip("캐릭터가 등반할 수 있는 최대 경사 각도입니다.")]
    float maxSlopeAngle;
    [SerializeField, Tooltip("경사 지형을 체크할 Raycast 발사 시작 지점입니다.")]
    Transform raycastOrigin;
    #endregion

    private CharacterController cc;
    private Rigidbody rb;

    private int groundLayer;

    public ParticleSystem mouseClickEffect;
    private void Awake()
    {
        mainCamera = Camera.main;
        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
        groundLayer = LayerMask.NameToLayer("Ground");
        mouseClickEffect.Stop();
    }

    private void OnEnable()
    {
        mouseClickAction.Enable();
        mouseClickAction.performed += Move;
    }

    private void OnDisable()
    {
        mouseClickAction.performed -= Move;
        mouseClickAction.Disable();
    }

    private void Move(InputAction.CallbackContext context)
    {
        // Layer 처리를 통해 움직일 수 없는 부분을 클릭해서 움직이는 현상을 막아줌.
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray: ray , hitInfo: out RaycastHit hit) && hit.collider && hit.collider.gameObject.layer.CompareTo(groundLayer)
            == 0)
        {
            // coroutine이 여러 개 동시에 실행되는 것을 막는 if문
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(PlayerMoveTowards(hit.point));
            targetPosition = hit.point;

            //파티클 시스템 위치 설정
            mouseClickEffect.transform.position = hit.point;
            mouseClickEffect.Play();
        }
    }
    


    private IEnumerator PlayerMoveTowards(Vector3 target)
    {
        float playerDistanceToFloor = transform.position.y - target.y;
        target.y += playerDistanceToFloor;
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            // Ignores Collisions
            Vector3 destination = Vector3.MoveTowards(transform.position, target, playerSpeed * Time.deltaTime);
            // transform.position = destination;

            // Character Controller
            Vector3 direction = target - transform.position;
            // Vector3 movement = direction.normalized * playerSpeed * Time.deltaTime;
            // cc.Move(movement);

            // Rigidbody
            rb.velocity = direction.normalized * playerSpeed;


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), rotationSpeed * 
                Time.deltaTime);

            yield return null;
        }
        // 목적지에 도착하면 속도를 0으로 바꿈.
        rb.velocity = Vector3.zero * 0f;
    }

    public bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit, RAY_DISTANCE, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlopeAngle;
        }
        return false;
    }

    protected Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 1);
    }
}

/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour
{

    #region #기본 컴포넌트
    public Vector3 direction { get; private set; }
    protected Player player;
    protected Rigidbody rigidBody;
    protected Animator animator;
    protected CapsuleCollider capsuleCollider;
    #endregion

    #region #이동 관련 변수
    protected const float CONVERT_UNIT_VALUE = 0.01f;
    protected const float DEFAULT_CONVERT_MOVESPEED = 3f;
    protected const float DEFAULT_ANIMATION_PLAYSPEED = 0.9f;
    protected float frontGroundHeight;
    #endregion

    #region #경사 체크 변수
    [Header("경사 지형 검사")]
    [SerializeField, Tooltip("캐릭터가 등반할 수 있는 최대 경사 각도입니다.")]
    float maxSlopeAngle;
    [SerializeField, Tooltip("경사 지형을 체크할 Raycast 발사 시작 지점입니다.")]
    Transform raycastOrigin;

    private const float RAY_DISTANCE = 2f;
    private RaycastHit slopeHit;
    #endregion

    #region #바닥 체크 변수
    [Header("땅 체크")]
    [SerializeField, Tooltip("캐릭터가 땅에 붙어 있는지 확인하기 위한 CheckBox 시작 지점입니다.")]
    Transform groundCheck;
    private int groundLayer;
    #endregion

    #region #UNITY_FUNCTIONS
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
    }

    void FixedUpdate()
    {
        Move();
    }

    #endregion


    private float CalculateNextFrameGroundAngle(float moveSpeed)
    {
        var nextFramePlayerPosition = raycastOrigin.position + direction * moveSpeed * Time.fixedDeltaTime;   // 다음 프레임 캐릭터 앞 부분 위치

        if (Physics.Raycast(nextFramePlayerPosition, Vector3.down, out RaycastHit hitInfo, RAY_DISTANCE, groundLayer))
        {
            return Vector3.Angle(Vector3.up, hitInfo.normal);
        }
        return 0f;
    }



    protected void Move()
    {
        float currentMoveSpeed = player.MoveSpeed * CONVERT_UNIT_VALUE;
        float animationPlaySpeed = DEFAULT_ANIMATION_PLAYSPEED + GetAnimationSyncWithMovement(currentMoveSpeed);
        bool isOnSlope = IsOnSlope();
        bool isGrounded = IsGrounded();

        Vector3 velocity = CalculateNextFrameGroundAngle(currentMoveSpeed) < maxSlopeAngle ? direction : Vector3.zero;
        Vector3 gravity = Vector3.down * Mathf.Abs(rigidBody.velocity.y);

        if (isGrounded && isOnSlope)         // 경사로에 있을 때
        {
            velocity = AdjustDirectionToSlope(direction);
            gravity = Vector3.zero;
            rigidBody.useGravity = false;
        }
        else
        {
            rigidBody.useGravity = true;
        }

        LookAt();
        rigidBody.velocity = velocity * currentMoveSpeed + gravity;
        animator.SetFloat("Velocity", animationPlaySpeed);
    }



    public bool IsGrounded()
    {
        Vector3 boxSize = new Vector3(transform.lossyScale.x, 0.4f, transform.lossyScale.z);
        return Physics.CheckBox(groundCheck.position, boxSize, Quaternion.identity, groundLayer);
    }

    public bool IsOnSlope()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out slopeHit, RAY_DISTANCE, groundLayer))
        {
            var angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle != 0f && angle < maxSlopeAngle;
        }
        return false;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        direction = new Vector3(input.x, 0f, input.y);
    }

    protected void LookAt()
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(direction);
            rigidBody.rotation = targetAngle;
        }
    }

    protected float GetAnimationSyncWithMovement(float changedMoveSpeed)
    {
        if (direction == Vector3.zero)
        {
            return -DEFAULT_ANIMATION_PLAYSPEED;
        }

        // (바뀐 이동 속도 - 기본 이동속도) * 0.1f
        return (changedMoveSpeed - DEFAULT_CONVERT_MOVESPEED) * 0.1f;
    }
    protected Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        Vector3 adjustVelocityDirection = Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        return adjustVelocityDirection;
    }
}*/