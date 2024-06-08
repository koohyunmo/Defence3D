using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GridObject : MonoBehaviour, IDragging
{
    [SerializeField]
    private float timeBetweenAttacks = 1f; // Attack Speed
    [SerializeField]
    private float attackRange = 1f;        // Attack Radius
    [SerializeField]
    private Enemy targetEnemy = null;
    [SerializeField]
    private GameObject rangeSprite = null;
    private float attackCounter;
    private bool isAttacking = false;

    void Start()
    {
        attackCounter = timeBetweenAttacks; // Initialize attack counter
        rangeSprite.transform.localScale = new Vector3(attackRange, attackRange, attackRange) * 2.5f;
    }

    void Update()
    {
        attackCounter -= Time.deltaTime;

        if (targetEnemy == null)
        {
            Enemy closestEnemy = GetClosestEnemyInRange();
            if (closestEnemy != null && Vector3.Distance(transform.position, closestEnemy.transform.position) <= attackRange)
            {
                targetEnemy = closestEnemy;
            }
        }
        else
        {
            if (attackCounter <= 0f)
            {
                isAttacking = true;
                attackCounter = timeBetweenAttacks; // Reset attack counter
            }
            else
            {
                isAttacking = false;
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
        }
    }

    public void Attack()
    {
        if (targetEnemy != null)
        {
            if(targetEnemy.isDead)
            {
                targetEnemy = null;
                attackCounter = 0;
                return;
            }
            
            var projectile = Managers.Resource.Instantiate("Bullet", pooling: true);
            projectile.GetComponent<Projectile>().Shoot(targetEnemy,transform.position);
        }
    }



    private float getTargetDistance(Enemy enemy)
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

    private List<Enemy> GetEnemiesInRange()
    {
        List<Enemy> enemiesInRange = new List<Enemy>();

        foreach (Enemy enemy in Managers.Object.monsters)
        {
            if(enemy.isDead) continue;

            if (Vector3.Distance(transform.position, enemy.transform.position) <= attackRange)
            {
                enemiesInRange.Add(enemy);
            }
        }
        return enemiesInRange;
    }

    private Enemy GetClosestEnemyInRange()
    {
        Enemy closestEnemy = null;
        float smallestDistance = float.PositiveInfinity;

        foreach (Enemy enemy in GetEnemiesInRange())
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
        Gizmos.DrawWireSphere(transform.position,attackRange);
    }

    public void DragOn()
    {
        rangeSprite.gameObject.SetActive(true);
    }

    public void Drop()
    {
        rangeSprite.gameObject.SetActive(false);
    }
}
