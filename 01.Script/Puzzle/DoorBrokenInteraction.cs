using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorBrokenInteraction : BaseInteraction
{
    public GameObject[] broken;
    public Animator animator;

    public GameObject puzzle;
    [HideInInspector] public int count;

    public override void Interaction()
    {
        GameObject _puzzle = Instantiate(puzzle);
        _puzzle.GetComponent<BrokenDoorPuzzle>().borken = this;
        _puzzle.GetComponent<BrokenDoorPuzzle>().maxCount = broken.Length * 2;
    }

    public void BrokenObject()
    {
        
        for (int i = 0; i < broken[count].transform.childCount; i++)
        {
            broken[count].transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
            
            Destroy(broken[count].transform.GetChild(i).gameObject, 1f);
            
        }
        count++;
        if(count >= broken.Length)
        {
            animator.SetBool("_bisOpen", true);
            GameInstance.instance._gameManger.player._bisStop = false ;
        }
    }


    [SerializeField]
    private float m_roughness;      //거칠기 정도
    [SerializeField]
    private float m_magnitude;      //움직임 범위


    public IEnumerator Shake(float duration)
    {
        Vector3 originalPos = transform.localPosition; 

        float halfDuration = duration / 2f;
        float elapsed = 0f;
        float tick = Random.Range(-10f, 10f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            tick += Time.deltaTime * m_roughness;
            float ping = Mathf.PingPong(elapsed, halfDuration);
            Vector3 offset = new Vector3(
                (Mathf.PerlinNoise(tick, 0) - 0.5f),
                (Mathf.PerlinNoise(0, tick) - 0.5f),
                0f) * m_magnitude * ping;

            GameInstance.instance._gameManger.player.mainCamera.transform.GetChild(0).transform.localPosition = originalPos + offset;

            yield return null;
        }

        transform.localPosition = originalPos; 
    }



}
