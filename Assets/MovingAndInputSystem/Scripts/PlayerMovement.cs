using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{

    #region #�⺻ ������Ʈ
    public Vector3 direction { get; private set; }
    protected Player player;
    protected Rigidbody rigidBody;
    //�ִϸ��̼� protected Animator animator;
    protected CapsuleCollider capsuleCollider;
    private Camera mainCamera;
    private Vector3 targetPosition;
    #endregion

    #region #�̵� ���� ����
    protected const float CONVERT_UNIT_VALUE = 0.01f;
    protected const float DEFAULT_CONVERT_MOVESPEED = 3f;
    //�ִϸ��̼� protected const float DEFAULT_ANIMATION_PLAYSPEED = 0.9f;
    protected float frontGroundHeight;
    #endregion

    #region #��� üũ ����
    [Header("��� ���� �˻�")]
    [SerializeField, Tooltip("ĳ���Ͱ� ����� �� �ִ� �ִ� ��� �����Դϴ�.")]
    float maxSlopeAngle;
    [SerializeField, Tooltip("��� ������ üũ�� Raycast �߻� ���� �����Դϴ�.")]
    Transform raycastOrigin;

    private const float RAY_DISTANCE = 2f;
    private RaycastHit slopeHit;
    #endregion

    #region #�ٴ� üũ ����
    [Header("�� üũ")]
    [SerializeField, Tooltip("ĳ���Ͱ� ���� �پ� �ִ��� Ȯ���ϱ� ���� CheckBox ���� �����Դϴ�.")]
    Transform groundCheck;
    private int groundLayer;
    #endregion

    #region #UNITY_FUNCTIONS
    void Start()
    {
        mainCamera = Camera.main;
        rigidBody = GetComponent<Rigidbody>();
        //�ִϸ��̼� animator = GetComponent<Animator>();
        player = GetComponent<Player>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        groundLayer = LayerMask.NameToLayer("Ground");
        // 1 << �־���
    }

    void FixedUpdate()
    {
        Move();
    }

    #endregion


    private float CalculateNextFrameGroundAngle(float moveSpeed)
    {
        var nextFramePlayerPosition = raycastOrigin.position + direction * moveSpeed * Time.fixedDeltaTime;   // ���� ������ ĳ���� �� �κ� ��ġ

        if (Physics.Raycast(nextFramePlayerPosition, Vector3.down, out RaycastHit hitInfo, RAY_DISTANCE, groundLayer))
        {
            return Vector3.Angle(Vector3.up, hitInfo.normal);
        }
        return 0f;
    }



    protected void Move()
    {
        float currentMoveSpeed = player.MoveSpeed * CONVERT_UNIT_VALUE;
        //�ִϸ��̼� float animationPlaySpeed = DEFAULT_ANIMATION_PLAYSPEED + GetAnimationSyncWithMovement(currentMoveSpeed);
        bool isOnSlope = IsOnSlope();
        bool isGrounded = IsGrounded();

        Vector3 velocity = CalculateNextFrameGroundAngle(currentMoveSpeed) < maxSlopeAngle ? direction : Vector3.zero;
        Vector3 gravity = Vector3.down * Mathf.Abs(rigidBody.velocity.y);

            if (isGrounded && isOnSlope)         // ���ο� ���� ��
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
        //�ִϸ��̼� animator.SetFloat("Velocity", animationPlaySpeed);
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

    /*�ִϸ��̼�    protected float GetAnimationSyncWithMovement(float changedMoveSpeed)
        {
            if (direction == Vector3.zero)
            {
                return -DEFAULT_ANIMATION_PLAYSPEED;
            }

            // (�ٲ� �̵� �ӵ� - �⺻ �̵��ӵ�) * 0.1f
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