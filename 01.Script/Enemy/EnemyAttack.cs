using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public BaseEnemy Enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Enemy.animator.GetBool("_bisAttack"))
        {
            other.gameObject.GetComponent<PlayerMoveMent>().Damage(Enemy.attack);
        }
    }
}
