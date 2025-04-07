using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public Animator animator;
    public GameObject target;
    [Header("ป๓ลย")]
    public int maxHp;
    public int currentHp;
    public int attack;
    public float moveSpeed;
    public GameObject dropObjcet;
    public bool _bisAi;
    private Rigidbody rb;
    public GameObject[] pos;
    public GameObject currnetObject;
    private int num;
    public AudioSource damageSource;

    private void Start()
    {
        currentHp = maxHp;
        rb = GetComponent<Rigidbody>();
        if(pos.Length > 0)
        {
            currnetObject = pos[1];
            Vector3 targetpos = currnetObject.transform.position;
            targetpos.y = 0;
            this.transform.LookAt(targetpos);
        }
        num = 0;
    }

    public virtual void Move(){
        if(moveSpeed > 0)
        {
            if (!_bisAi)
            {
                animator.SetBool("_bisMove", true);
                Vector3 velocity = transform.forward * moveSpeed;
                velocity.y = rb.velocity.y; 
                rb.velocity = velocity;
                if (Vector3.Distance(this.transform.position, currnetObject.transform.position) < 1.5f)
                {
                    currnetObject = pos[num];
                    Vector3 targetpos = currnetObject.transform.position;
                    targetpos.y = 0;
                    this.transform.LookAt(targetpos);
                    num++;
                    if (num >= pos.Length)
                    {
                        num = 0;
                    }
                }
            }
            else
            {
                if(target != null)
                {
                    animator.SetBool("_bisMove", true);
                    this.transform.LookAt(target.transform.position);
                    Vector3 velocity = transform.forward * moveSpeed;
                    velocity.y = rb.velocity.y;
                    rb.velocity = velocity;
                } else
                {
                    animator.SetBool("_bisMove", false);
                }
            }
        } else if(target != null)
        {
            if(Vector3.Distance(this.transform.position, target.transform.position) < 8f && !animator.GetBool("_bisAttack"))
            {
                StartCoroutine(Attack());
            }
        }
    }

    public void TargetCheck()
    {
        GameObject temp = null;
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            if (p.GetComponent<PlayerMoveMent>()._bisTarget && Vector3.Distance(this.transform.position, p.transform.position) < 25f)
            {
                temp = p;
            }
        }

        target = temp;
    }

    private void Update()
    {
        if (!animator.GetBool("_bisDie") && _bisDamage)
        {
            TargetCheck();
            Move();

        }
    }

    bool _bisDamage = true;
    public void Damage(int _damage)
    {
        if (_bisDamage)
        {
            _bisDamage = false;
            currentHp -= _damage;
            if(damageSource != null) damageSource.Play();
            rb.AddForce(transform.up * 4, ForceMode.Impulse);
            animator.SetBool("_bisDamage", true);
            StartCoroutine(DamageDelay());

        }
    }

    public IEnumerator DamageDelay()
    {
        if (currentHp <= 0)
        {
            animator.SetBool("_bisDie", true);
            yield return new WaitForSeconds(1.5f);
            GetComponent<Collider>().isTrigger = true;
            Destroy(this.gameObject, 1f);
        }
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("_bisDamage", false);
        _bisDamage = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && _bisAttack)
        {
            Debug.Log(other.gameObject.name);
            other.gameObject.GetComponent<PlayerMoveMent>().Damage(attack);
            StartCoroutine(AttackDelay());
        }
    }

    public IEnumerator Attack()
    {
        animator.SetBool("_bisAttack", true);
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("_bisAttack", false);
    }

    bool _bisAttack = true;
    public IEnumerator AttackDelay()
    {
        _bisAttack = false;
        StartCoroutine(Attack());
        yield return new WaitForSeconds(1f);
        _bisAttack = true;
    }

}
