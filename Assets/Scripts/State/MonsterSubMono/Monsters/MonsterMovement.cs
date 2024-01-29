using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    public void MoveRotation()
    {
        Vector3 direction = ai.desiredVelocity;
        direction.Set(direction.x, 0f, direction.z);

        Quaternion targetAngle = Quaternion.LookRotation(direction);
        monsterTransform.rotation = Quaternion.Slerp(monsterTransform.rotation, targetAngle, 3f * Time.deltaTime);
    }

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

    public void AddForce(float addforce)
    {
        Vector3 direction = characterPosition.position - monsterTransform.position;
   
        monsterRigidbody.velocity = -(direction.normalized) * addforce;
    }


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

    public void IsKinematic(bool on)
    {
        monsterRigidbody.isKinematic = on;
    }
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
