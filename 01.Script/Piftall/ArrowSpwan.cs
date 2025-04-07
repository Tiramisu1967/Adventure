using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowSpwan : MonoBehaviour
{
    public float spwanDelay;
    public GameObject pos;
    public GameObject arrow;
    private void Start()
    {
        StartCoroutine(Spwan());
    }

    public IEnumerator Spwan()
    {
        while (true)
        {
            yield return new WaitForSeconds(spwanDelay);
            Instantiate(arrow, pos.transform.position, pos.transform.rotation);
        }
    }
}
