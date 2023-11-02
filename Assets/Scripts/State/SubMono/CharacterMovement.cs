using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : SubMono<PlayerController>
{
    Rigidbody playerRigidbody;
    Transform playerTransform;

    private float speed;
    private float rotationSpeed = 3f;

    protected override void _Clear()
    {
        
    }

    protected override void _Excute()
    {
       
    }

    protected override void _Init()
    {
        playerRigidbody = _board._playerState.GetComponent<Rigidbody>();
        playerTransform = _board._playerState.GetComponent<Transform>();

        print(playerTransform.gameObject.name);
        print(playerRigidbody.gameObject.name);
    }


    public void Move(float speed)
    {

    }

    private IEnumerator PlayerMoveTowards(Vector3 target, float playerSpeed)
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
            playerRigidbody.velocity = direction.normalized * playerSpeed;


            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction.normalized), rotationSpeed *
                Time.deltaTime);

            yield return null;
        }
        // 목적지에 도착하면 속도를 0으로 바꿈.
        playerRigidbody.velocity = Vector3.zero * 0f;
    }
}
