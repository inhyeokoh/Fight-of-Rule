using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : SubMono<PlayerController>
{
    //구르기 지속시간
    private float avoidTime = 0.5f;

    //공격 지속시간
    public float attackTime;

    //현재 공격 카운트
    private int comboNumber = 0;

    public Transform playerTransform;
    public Vector3 targetPosition;

    public Coroutine _moveTowardsNpcCoroutine;
    public Coroutine MoveTowardsNpcCoroutine { get { return _moveTowardsNpcCoroutine; } }
    Coroutine attackCoroutine;
    Rigidbody playerRigidbody;
    Vector3 direction;

    public Rigidbody Rb { get { return playerRigidbody; } }
    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }

    private float rotationSpeed = 6f;

    protected override void _Clear()
    {
        
    }

    protected override void _Excute()
    {

    }



    protected override void _Init()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
        playerTransform = gameObject.GetComponent<Transform>();
        //_board.camera.GetComponent<CameraMovement>().target = gameObject;
    }

    /*  public void CharacterTransformRigidBody(GameObject clone)
      {
          playerRigidbody = clone.GetComponent<Rigidbody>();
          playerTransform = clone.GetComponent<Transform>();
      }
  */

    // 캐릭을 이동하는 물리엔진
    public void Move(int playerSpeed)
    {       
        targetPosition.y += 0;         
        direction = targetPosition - playerTransform.position;                 
        playerRigidbody.velocity = direction.normalized * playerSpeed;        
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, Quaternion.LookRotation(direction.normalized), rotationSpeed *
                Time.deltaTime);
    }
     
    // 구르기 물리엔진
    public void Avoid(float playerSpeed)
    { 
        if (avoidTime > 0)
        {
            avoidTime -= Time.deltaTime;
            playerRigidbody.velocity = direction.normalized * (playerSpeed * 2.5f);
        }
        else
        {
            _board._playerState.IsAvoid = false;
            playerRigidbody.velocity = Vector3.zero;
            avoidTime = 0.5f;                
        } 

    }

    // 현재 캐릭이 바라보고있는 방향
    public void Direction(Vector3 target)
    {
        target.y += 0;
        direction = target - playerTransform.position;
        playerTransform.LookAt(target);
    }

    //현재 캐릭 공격콤보가 몇번쨰인지
    public void AttackCombo(int number)
    {        
        _board._animationController.ChanageAttackAnimation(comboNumber);
        AttackTime(number);
    }
    //캐릭터의 움직임을 고정시키기위한 메서드였는데 버그가 많이생김
    public void IsKinematic(bool on)
    {
        playerRigidbody.isKinematic = on;
    }

    //일반 공격을 했을때 공격 콤보를 초기화해야할지 확인하는 메서드
    public void AttackTime(int number)
    {
        if (attackCoroutine != null)
        {         
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine("AttackComboTime", number);
       // print("AttackTime써짐");
       /* StopCoroutine("AttackComboTime");
        StartCoroutine("AttackComboTime", number);*/
    }

    public void StopNpcMoveCoroutine()
    {
        if (_moveTowardsNpcCoroutine != null)
        {
            StopCoroutine(_moveTowardsNpcCoroutine);
            _moveTowardsNpcCoroutine = null;
        }
    }

    public void StartNpcMoveCoroutne(GameObject hitObject, float dialogDist)
    {
        _moveTowardsNpcCoroutine = StartCoroutine(MoveTowardsNpc(hitObject, dialogDist));
    }


    public void Hit()
    {

    }
    public void Fall()
    {

    }
    public void Stop()
    {
        playerRigidbody.velocity = Vector3.zero;
    }   

    //애니메이션 공격 순서와 공격 초기화
    IEnumerator AttackComboTime(int number)
    {
        attackTime = 1.4f;
        if (comboNumber < number)
        {
            comboNumber++;   
            while (attackTime > 0)
            {

                attackTime -= Time.deltaTime;
                yield return null;
            }
           // print(attackTime);   
            comboNumber = 0;
        }
        else
        {
            comboNumber = 0;
        }      
    }

    public IEnumerator MoveTowardsNpc(GameObject hitObject, float dialogDist)
    {
        Vector3 npcPosition = hitObject.transform.position;
        // NPC와의 방향 벡터 계산
        Vector3 direction = (npcPosition - playerTransform.position).normalized;
        // NPC로부터 dialogDist만큼 떨어진 지점 계산
        Vector3 targetPosition = npcPosition - direction * dialogDist;

        while (Vector3.Distance(playerTransform.position, targetPosition) > 1f)
        {         
            TargetPosition = new Vector3(targetPosition.x, playerTransform.position.y, targetPosition.z);
           _board._playerState.ChangeState((int)Enum_CharacterState.Move);
            // 플레이어가 목표 지점에 도달할 때까지 이동
            yield return null;
        }

        Npc npc = hitObject.GetComponent<Npc>();
        _board._interaction.InteractingNpcID = npc.NpcID;
        TargetPosition = playerTransform.position;
        //_playerState.ChangeState((int)Enum_CharacterState.Idle);
        npc.StartInteract();
    }
}
