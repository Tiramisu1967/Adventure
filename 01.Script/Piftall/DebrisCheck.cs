using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class DebrisCheck : MonoBehaviour
{
    public int attack;

    private void Start()
    {
        Destroy(gameObject, 2f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMoveMent>().Damage(attack);
            Destroy(this.gameObject);
        }
    }
}
