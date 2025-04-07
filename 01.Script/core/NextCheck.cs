using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextCheck : MonoBehaviour
{
    public bool _next;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            if (_next)
            {
                GameInstance.instance.currentStage++;
                Debug.Log(GameInstance.instance.currentStage);
                GameInstance.instance._gameManger.SceneMove();
            } else {
                GameInstance.instance.currentStage--;
                Debug.Log(GameInstance.instance.currentStage);
                GameInstance.instance._gameManger.SceneMove();
            }
        }
    }
}
