using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//몬스터들의 물리엔진들을 관리하는 클래스
public class MonsterMovement : SubMono<MonsterController>
{
    [SerializeField]
    private Transform monsterTransform;
    [SerializeField]
    private Rigidbody monsterRigidbody;
    [SerializeField]
    private NavMeshAgent ai;
    [SerializeField]
    private float hitCoolTime;

    private float rotationSpeed = 3f;
    public float delay;
    private float timeZero = 0;

    public Vector3 spawn;

    
  
    public Transform characterPosition;

    public float attackSpeed;
    public float abliltyDelay;

    protected override void _Clear()
    {

    }

    protected override void _Excute()
    {
       
    }

   
    protected override void _Init()
    {
        ai = gameObject.GetComponent<NavMeshAgent>();
        ai.updateRotation = false;
        monsterTransform = gameObject.GetComponent<Transform>();
        monsterRigidbody = gameObject.GetComponent<Rigidbody>();
        spawn = gameObject.transform.position;
    }

    // 몬스터들을 움직이게하는 메서드
    public void Move(int speed)
    {
        ai.isStopped = false;
        ai.speed = speed;
        ai.acceleration = speed;
        

        /*Vector3 direction = characterPosition.position - monsterTransform.position;
        monsterTransform.rotation = Quaternion.Slerp(monsterTransform.rotation, Quaternion.LookRotation(direction.normalized), rotationSpeed *
                Time.deltaTime);
    
        Vector3 newRotation = monsterTransform.rotation.eulerAngles;
        newRotation.z = 0;
        newRotation.x = 0;
        monsterTransform.rotation = Quaternion.Euler(newRotation);*/


        ai.SetDestination(characterPosition.position);
        MoveRotation();

        //monsterRigidbody.velocity = (direction.normalized) * speed;
    }

    // 무브했을때 회전하는 메서드
    public void MoveRotation()
    {
        Vector3 direction = ai.desiredVelocity;
        direction.Set(direction.x, 0f, direction.z);
      
        if (direction != Vector3.zero)
        {
            Quaternion targetAngle = Quaternion.LookRotation(direction);
            monsterTransform.rotation = Quaternion.Slerp(monsterTransform.rotation, targetAngle, 3f * Time.deltaTime);
        }
        else
        {
            return;
        }
    }


    // 공격이나 가만히 있을때 회전
    public void Rotation()
    {
        Vector3 direction = characterPosition.position - monsterTransform.position;
        monsterTransform.rotation = Quaternion.Slerp(monsterTransform.rotation, Quaternion.LookRotation(direction.normalized), 1.5f *
                Time.deltaTime);

        Vector3 newRotation = monsterTransform.rotation.eulerAngles;
        newRotation.z = 0;
        newRotation.x = 0;
        monsterTransform.rotation = Quaternion.Euler(newRotation);
    }

    //몬스터의 속도가 빠를때 회전을 조절하기 위한 메서드
    public void Slerp()
    {
        Vector3 direction = characterPosition.position - monsterTransform.position;
        monsterTransform.rotation = Quaternion.Slerp(monsterTransform.rotation, Quaternion.LookRotation(direction.normalized), rotationSpeed *
                Time.deltaTime);

        Vector3 newRotation = monsterTransform.rotation.eulerAngles;
        newRotation.z = 0;
        newRotation.x = 0;
        monsterTransform.rotation = Quaternion.Euler(newRotation);
    }


    //다시 자기 위치로 돌아가기 위한 메서드
    public void Return(int speed)
    {
        ai.isStopped = false;
        ai.speed = speed;

        ai.SetDestination(spawn);
        MoveRotation();

        //monsterRigidbody.velocity = (direction.normalized) * speed;
        /* Vector3 direction = spawn - monsterTransform.position;

         monsterTransform.rotation = Quaternion.Slerp(monsterTransform.rotation, Quaternion.LookRotation(direction.normalized), rotationSpeed *
                 Time.deltaTime);


         monsterRigidbody.velocity = (direction.normalized) * speed;*/
    }

    // 데미지를 받았을때 밀려나기위한 메서드
    public void AddForce(float addforce)
    {
        Vector3 direction = characterPosition.position - monsterTransform.position;
   
        monsterRigidbody.velocity = -(direction.normalized) * addforce;
    }

    // 공격할때 물리엔진
    public void Attack(float attackSpeed)
    {
        Stop();
        LookAt();

        if (_board._monsterState.IsAttack)
        {            
            this.attackSpeed = attackSpeed;
            _board._monsterState.IsAttack = false;
            StartCoroutine("AttackSpeed");
        }
        else
        {
            return;
        }  
    }

    public void Dead()
    {
        StopAllCoroutines();
    }

    // 원래는 캐릭들이 밀려나지 않기위한 메서드였는데 이걸 체크해주면 트리거가 두번 체크되는 버그가 생겨서 일단 철회
    public void IsKinematic(bool on)
    {
        monsterRigidbody.isKinematic = on;
    }

    // 몬스터의 스킬을 쓰게할지 못쓰게할지 체크
    public void Ablilty(float abliltyDelay)
    {
        Stop();        
        //LookAt();

        if (_board._monsterState.IsAbliltyDelay)
        {
            this.abliltyDelay = abliltyDelay;
            _board._monsterState.IsAbliltyDelay= false;
            StartCoroutine("AbliltyDelay");
        }
        else
        {
            return;
        }
    }

    public void LookAt()
    {
        monsterTransform.LookAt(characterPosition);
        Vector3 newRotation = monsterTransform.rotation.eulerAngles;
        newRotation.z = 0;
        newRotation.x = 0;
        monsterTransform.rotation = Quaternion.Euler(newRotation);
    }

    public void Delay(float delay)
    {
        if (_board._monsterState.IsDelay)
        {
            this.delay = delay;
            _board._monsterState.IsDelay = false;
            StartCoroutine("DelayCheck");
        }
        else
        {
            return;
        }
    }
    
    public void HitDelay(float hitTime)
    {
        hitCoolTime = hitTime;

        StopCoroutine("HitCoolTime");
        StartCoroutine("HitCoolTime");
    }

    public void Stop()
    {
        ai.isStopped = true;
        ai.velocity = Vector3.zero;
    }

    IEnumerator DelayCheck()
    {
        while (delay > timeZero && !_board._monsterState.IsHDeadCheck)
        {
            delay -= Time.deltaTime;
            yield return null;
        }

        _board._monsterState.IsDelay = true;
    }
    
    IEnumerator HitCoolTime()
    {
        while (hitCoolTime > timeZero && !_board._monsterState.IsHDeadCheck)
        {
            hitCoolTime -= Time.deltaTime;
            yield return null;
        }

        _board._monsterState.IsHitCheck = false;
    }
    IEnumerator AttackSpeed()
    {
        while (attackSpeed > timeZero && !_board._monsterState.IsHDeadCheck)
        {
            attackSpeed -= Time.deltaTime;
            yield return null;
        }

        _board._monsterState.IsAttack = true;
    }

    IEnumerator AbliltyDelay()
    {       
        while (abliltyDelay > timeZero && !_board._monsterState.IsHDeadCheck)
        {
            abliltyDelay -= Time.deltaTime;
            yield return null;
        }

        _board._monsterState.IsAbliltyDelay = true;
    }

}
