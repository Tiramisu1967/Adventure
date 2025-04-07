using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Quiz : BaseInteraction
{
    public GameObject quizCanvas;
    public string quizText;
    private GameObject _quiz;

    public override void Interaction()
    {
        base.Interaction();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        GameInstance.instance._gameManger.player._bisStop = true;
         _quiz = Instantiate(quizCanvas);
        _quiz.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{quizText}";
    }

    public void QuizExit()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
        GameInstance.instance._gameManger.player._bisStop = true;
        Destroy(_quiz);
    }
}
