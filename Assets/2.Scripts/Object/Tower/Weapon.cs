using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MyDefine;
public class Weapon : GridObject
{
    [SerializeField]
    private float timeBetweenAttacks => weaponData.fireRate; // Attack Speed
    [SerializeField]
    private float attackRange => weaponData.range;        // Attack Radius
    [SerializeField]
    private Monster targetEnemy = null;
    [SerializeField]
    private GameObject rangeSprite = null;
    [SerializeField]
    private float attackCounter = 0f;
    [SerializeField]
    private bool isAttacking = false;
    private WeaponData weaponData = null;
    private Animator anim;
    [SerializeField] int damage;
    public void Spawn(WeaponData data)
    {
        // Data Init
        weaponData = data;
        anim = GetComponent<Animator>();

        // 초기화
        attackCounter = 0f;
        if (rangeSprite == null)
        {
            var rangeObj = Managers.Resource.Instantiate("AttackRange", transform);
            rangeObj.gameObject.SetActive(false);
            rangeSprite = rangeObj;
        }
        rangeSprite.transform.localScale = new Vector3(attackRange, attackRange, attackRange) * 2.5f;

        damage = weaponData.damage;
    }

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
            anim.Play("Slash_1",-1,0);
            var projectile = Managers.Resource.Instantiate("Bullet", pooling: true);
            projectile.GetComponent<Projectile>().Shoot(targetEnemy, transform.position,weaponData.damage);
        }
    }

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

        foreach (Monster enemy in Managers.Object.monsters)
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

    private void OnDrawGizmos()
    {
        if(weaponData != null)
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public override void DragOn()
    {
        rangeSprite.gameObject.SetActive(true);
    }

    public override void Drop()
    {
        rangeSprite.gameObject.SetActive(false);
    }
}