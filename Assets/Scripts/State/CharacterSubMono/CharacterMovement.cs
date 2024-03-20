using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : SubMono<PlayerController>
{
    //������ ���ӽð�
    private float avoidTime = 0.5f;

    //���� ���ӽð�
    public float attackTime;

    //���� ���� ī��Ʈ
    private int comboNumber = 0;

    public Transform playerTransform;
    public Vector3 targetPosition;
   
    Coroutine attackCoroutine;
    Rigidbody playerRigidbody;
    Vector3 direction;

    public Rigidbody Rb { get { return playerRigidbody; } }
    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }

    private float rotationSpeed = 3f;

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
        _board.camera.GetComponent<CameraMovement>().target = gameObject;
    }

    /*  public void CharacterTransformRigidBody(GameObject clone)
      {
          playerRigidbody = clone.GetComponent<Rigidbody>();
          playerTransform = clone.GetComponent<Transform>();
      }
  */

    // ĳ���� �̵��ϴ� ��������
    public void Move(int playerSpeed)
    {       
        targetPosition.y += 0;         
        direction = targetPosition - playerTransform.position;                 
        playerRigidbody.velocity = direction.normalized * playerSpeed;        
        playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, Quaternion.LookRotation(direction.normalized), rotationSpeed *
                Time.deltaTime);
    }
     
    // ������ ��������
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

    // ���� ĳ���� �ٶ󺸰��ִ� ����
    public void Direction(Vector3 target)
    {
        target.y += 0;
        direction = target - playerTransform.position;
        playerTransform.LookAt(target);
    }

    //���� ĳ�� �����޺��� ���������
    public void AttackCombo(int number)
    {        
        _board._animationController.ChanageAttackAnimation(comboNumber);
        AttackTime(number);
    }
    //ĳ������ �������� ������Ű������ �޼��忴�µ� ���װ� ���̻���
    public void IsKinematic(bool on)
    {
        playerRigidbody.isKinematic = on;
    }

    //�Ϲ� ������ ������ ���� �޺��� �ʱ�ȭ�ؾ����� Ȯ���ϴ� �޼���
    public void AttackTime(int number)
    {
        if (attackCoroutine != null)
        {         
            StopCoroutine(attackCoroutine);
        }
        attackCoroutine = StartCoroutine("AttackComboTime", number);
       // print("AttackTime����");
       /* StopCoroutine("AttackComboTime");
        StartCoroutine("AttackComboTime", number);*/
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

    //�ִϸ��̼� ���� ������ ���� �ʱ�ȭ
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


}
