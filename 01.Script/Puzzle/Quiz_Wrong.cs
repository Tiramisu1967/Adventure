using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz_Wrong : MonoBehaviour
{
    public GameObject enemy;
    public List<GameObject> enemys;
    public GameObject[] pos;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(wrong());
        }
    }

    public IEnumerator wrong()
    {
        yield return new WaitForSeconds(0.4f);
        GetComponent<Collider>().isTrigger = false;
        StartCoroutine(Spwan());
    }

    public IEnumerator Spwan()
    {
        int i = 20;
        while ( enemys.Count < i)
        {
            Vector3 random = new Vector3(Random.Range(pos[0].transform.position.x, pos[1].transform.position.x), 0, Random.Range(pos[0].transform.position.z, pos[1].transform.position.z));
            GameObject e = Instantiate(enemy, random, Quaternion.identity);
            enemys.Add(e);
            yield return null;
        }

        while (true) {
            enemys.RemoveAll(e => e == null);
            if (enemys.Count < 20)
            {
                Vector3 random = new Vector3(Random.Range(pos[0].transform.position.x, pos[1].transform.position.x), 0, Random.Range(pos[0].transform.position.z, pos[1].transform.position.z));
                GameObject e = Instantiate(enemy, random, Quaternion.identity);
                enemys.Add(e);
            }
            yield return new WaitForSecondsRealtime(1f);
        }
    }
}
