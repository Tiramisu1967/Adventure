using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastDoor : MonoBehaviour
{
   public GameObject treasure;
    public Animator animator;
    public AudioClip muiscsource;
    private void Update()
    {
        if(treasure == null)
        {
            GameInstance.instance.backgroundMusic.clip = muiscsource;
            GameInstance.instance.backgroundMusic.Play();
            animator.SetBool("_bisOpen", true);
        }
    }
}
