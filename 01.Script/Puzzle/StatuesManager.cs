using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatuesManager : MonoBehaviour
{
    public GameObject[] statuses;
    public bool[] _bisCheck;
    public bool _bisOpen;

    public Animator Animator;

    private void Start()
    {
        if (_bisOpen)
        {
            for (int i = 0; i < statuses.Length; i++)
            {
                statuses[i].transform.rotation = Quaternion.Euler(0, statuses[i].GetComponent<Statues>().currect, 0);

            }
            Open();
        }

        for(int i = 0; i < statuses.Length; i++)
        {
            statuses[i].GetComponent<Statues>().n = i;
            statuses[i].GetComponent<Statues>().Manager = this;
        }
    }

    public void Checking()
    {
        
        for(int i = 0; i < _bisCheck.Length; i++)
        {
            if (_bisCheck[i] == false)
            {
                return;
            }
        }
        Open();
    }

    public void Open()
    {
        Animator.SetBool("_bisOpen", true);
    }
}
