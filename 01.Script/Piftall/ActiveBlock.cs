using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBlock : MonoBehaviour
{
    public Animator animator;
    public float viewDelay;
    public float closeDelay;

    public GameObject activeBlock;

    private void Start()
    {
        StartCoroutine(Veiw());
    }

    public IEnumerator Veiw()
    {
        activeBlock.SetActive(true);
        yield return new WaitForSeconds(viewDelay - 2f);
        Debug.Log("애니매이션 실행");
        animator.SetBool("_isPlaying", true);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("애니매이션 종료");
        animator.SetBool("_isPlaying", false);
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(Close());
    }

    public IEnumerator Close()
    {
        activeBlock.SetActive(false);
        yield return new WaitForSeconds(closeDelay);
        StartCoroutine(Veiw());
    }
}
