using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyDefine;
using static MyEnums;
public class Weapon : GridObject
{
    /*------------
        맴버변수
    --------------*/
    [SerializeField]
    protected float timeBetweenAttacks => weaponData.GetFireRate(); // Attack Speed
    [SerializeField]
    protected float attackRange => weaponData.range;        // Attack Radius
    [SerializeField]
    protected Monster targetEnemy = null;
    [SerializeField]
    private GameObject rangeSprite = null;
    [SerializeField]
    protected float attackCounter = 0f;
    [SerializeField]
    protected bool isAttacking = false;
    protected WeaponData weaponData = null;
    protected Animator anim;
    protected int totalDamage => weaponData.damage * Managers.Upgrade.GetLevel(Grade, Managers.Object.Player);
    [SerializeField] private int currentDamage;

    /*------------
        프로퍼티
    --------------*/

    public WeaponType WeaponType {get; protected set;} = WeaponType.None;
    public UnitGrade Grade  {get; protected set;} = UnitGrade.None;

    /*------------
        초기 설정
    --------------*/
    public virtual void Spawn(WeaponData data)
    {
        // Data Init
        weaponData = data;
        anim = GetComponent<Animator>();
        Grade = weaponData.grade;

        // 초기화
        attackCounter = 0f;
        if (rangeSprite == null)
        {
            var rangeObj = Managers.Resource.Instantiate("AttackRange", transform);
            rangeObj.gameObject.SetActive(false);
            rangeSprite = rangeObj;
        }
        //rangeSprite.transform.localScale = new Vector3(attackRange, attackRange, attackRange) * 2.5f;
        rangeSprite.transform.localScale = new Vector3(attackRange, attackRange, attackRange) * 0.7f;

        currentDamage = totalDamage;
    }
    /*-------------
        업데이트
    ---------------*/
    void Update()
    {
        if (targetEnemy == null)
        {
            Monster closestEnemy = GetClosestEnemyInRange();
            if (closestEnemy != null && Vector3.Distance(transform.position, closestEnemy.transform.position) <= attackRange)
            {
                targetEnemy = closestEnemy;
            }
        }
        else
        {
            if (CURRENT_TIME >= attackCounter)
            {
                isAttacking = true;
                attackCounter = timeBetweenAttacks + CURRENT_TIME; // Reset attack counter
            }

            if (Vector3.Distance(transform.position, targetEnemy.transform.position) > attackRange)
            {
                targetEnemy = null;
            }
        }
    }

    void FixedUpdate()
    {
        if (isAttacking)
        {
            Attack();
            isAttacking = false;
        }
    }
    /*---------------
        공격관련 함수
    -----------------*/
    public void Attack()
    {
        if (targetEnemy != null)
        {
            if (targetEnemy.isDead)
            {
                targetEnemy = null;
                attackCounter = 0;
                return;
            }
            PlayAnim();
            AttackDetail();
            currentDamage = totalDamage;
        }
    }

    protected virtual void AttackDetail()
    {
        Debug.LogError("공격 방식을 설정해주세요.");
    }

    protected virtual void PlayAnim()
    {
        //anim.Play("Slash_1", -1, 0);
    }
    /*------------
    탐색 관련 함수
    --------------*/
    private float getTargetDistance(Monster enemy)
    {
        if (enemy == null)
        {
            enemy = GetClosestEnemyInRange();
            if (enemy == null)
            {
                return 0f;
            }
        }
        return Vector3.Distance(transform.position, enemy.transform.position);
    }

    private List<Monster> GetEnemiesInRange()
    {
        List<Monster> enemiesInRange = new List<Monster>();

        foreach (Monster enemy in Managers.Object.GetMonsters())
        {
            if (enemy.isDead) continue;

            if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }

    private Monster GetClosestEnemyInRange()
    {
        Monster closestEnemy = null;
        float smallestDistance = float.PositiveInfinity;

        foreach (Monster enemy in GetEnemiesInRange())
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closestEnemy = enemy;
            }
        }
        return closestEnemy;
    }
    /*-------------
        헬퍼 함수
    ---------------*/
    public WeaponData GetWeaponData()
    {
        return weaponData;
    }
    protected void LookAtTarget(Transform target, Transform model)
    {
        if(target == null) return;
        if(model == null) return;

        // 타겟의 방향을 계산
        Vector3 directionToTarget = (target.position - model.position).normalized;

        // 타겟을 향한 회전값 계산
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        // 모델의 회전
        model.rotation = targetRotation;

    }
    protected void LookAtPosition(Vector3 pos, Transform model)
    {
        if (model == null) return;

        // 타겟의 방향을 계산
        Vector3 directionToTarget = (pos - model.position).normalized;

        // 타겟을 향한 회전값 계산
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        targetRotation.x = 0f;
        targetRotation.z = 0f;

        // 모델의 회전
        model.rotation = targetRotation;

    }
    /*------------
        인터페이스
    --------------*/
    public override void Dragging()
    {
        rangeSprite.gameObject.SetActive(true);
    }

    public override void Drop()
    {
        rangeSprite.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (weaponData == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
