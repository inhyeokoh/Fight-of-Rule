using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 1);
    }
}
