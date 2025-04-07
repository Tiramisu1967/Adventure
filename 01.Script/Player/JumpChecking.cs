using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpChecking : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            GameInstance.instance._gameManger.player._bisJump = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            GameInstance.instance._gameManger.player._bisJump = false;
        }
    }
}
