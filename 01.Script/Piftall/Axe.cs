using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    public Animator animator;
    public AudioSource AudioSource;
    public int attack;
    public float delay;

    private void Start()
    {
        StartCoroutine(AnimationStart());
    }

    public IEnumerator AnimationStart()
    {
        yield return new WaitForSeconds(delay);
        animator.SetBool("_bisPlaying", true);
        if(AudioSource != null) AudioSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMoveMent>().Damage(attack);
        }
    }
}
