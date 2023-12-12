using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{

    #region #기본 컴포넌트
    public Vector3 direction { get; private set; }
    protected Player player;
    protected Rigidbody rigidBody;
    //애니메이션 protected Animator animator;
    protected CapsuleCollider capsuleCollider;
    private Camera mainCamera;
    private Vector3 targetPosition;
    #endregion

    #region #이동 관련 변수
    protected const float CONVERT_UNIT_VALUE = 0.01f;
    protected const float DEFAULT_CONVERT_MOVESPEED = 3f;
    //애니메이션 protected const float DEFAULT_ANIMATION_PLAYSPEED = 0.9f;
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
        mainCamera = Camera.main;
        rigidBody = GetComponent<Rigidbody>();
        //애니메이션 animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        groundLayer = LayerMask.NameToLayer("Ground");
        // 1 << 있었음
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
        //애니메이션 float animationPlaySpeed = DEFAULT_ANIMATION_PLAYSPEED + GetAnimationSyncWithMovement(currentMoveSpeed);
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

        Debug.Log(Vector2.Distance(transform.position, targetPosition));
        if (Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            rigidBody.velocity = Vector3.zero * 0;
        }
        //애니메이션 animator.SetFloat("Velocity", animationPlaySpeed);
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
        Debug.Log(context);
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray: ray, hitInfo: out RaycastHit hit) && hit.collider && hit.collider.gameObject.layer.CompareTo(groundLayer)
            == 0)
        {
            targetPosition = hit.point;
            targetPosition.y = transform.position.y;
            direction = (targetPosition - transform.position).normalized;
        }

    }

    protected void LookAt()
    {
        if (direction != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(direction);
            rigidBody.rotation = targetAngle;
        }
    }

    /*애니메이션    protected float GetAnimationSyncWithMovement(float changedMoveSpeed)
        {
            if (direction == Vector3.zero)
            {
                return -DEFAULT_ANIMATION_PLAYSPEED;
            }

            // (바뀐 이동 속도 - 기본 이동속도) * 0.1f
            return (changedMoveSpeed - DEFAULT_CONVERT_MOVESPEED) * 0.1f;
        }*/
    protected Vector3 AdjustDirectionToSlope(Vector3 direction)
    {
        Vector3 adjustVelocityDirection = Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
        return adjustVelocityDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 1);
    }
}