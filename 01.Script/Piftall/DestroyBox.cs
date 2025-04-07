using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBox : MonoBehaviour
{
    public float objectDelay;
    public GameObject destoryObject;
    public float temp;

    private void Update()
    {
        if(temp <= 0)
        {
            GameObject Debris = Instantiate(destoryObject, this.transform.position, Quaternion.identity);
            Destroy(Debris, 5f);
            temp = objectDelay;
        } else
        {

            temp -= Time.deltaTime;
        }
    }
}
