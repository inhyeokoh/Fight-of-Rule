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
        // Layer ó���� ���� ������ �� ���� �κ��� Ŭ���ؼ� �����̴� ������ ������.
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray: ray , hitInfo: out RaycastHit hit) && hit.collider && hit.collider.gameObject.layer.CompareTo(groundLayer)
            == 0)
        {
            // coroutine�� ���� �� ���ÿ� ����Ǵ� ���� ���� if��
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(PlayerMoveTowards(hit.point));
            targetPosition = hit.point;

            //��ƼŬ �ý��� ��ġ ����
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
        // �������� �����ϸ� �ӵ��� 0���� �ٲ�.
        rb.velocity = Vector3.zero * 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(targetPosition, 1);
    }
}
