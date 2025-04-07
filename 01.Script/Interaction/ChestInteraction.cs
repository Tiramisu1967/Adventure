using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameInstance;

public class ChestInteraction : BaseInteraction
{
    public int number;
    public float maxCount;
    public float moveSpeed;
    public GameObject puzzleObject;
    public GameObject treasure;
    public override void Interaction()
    {
        base.Interaction();

        if (puzzleObject != null)
        {
            GameObject _puzzle = Instantiate(puzzleObject);
            _puzzle.GetComponent<LockPicPuzzle>().treasure = this;
            _puzzle.GetComponent<LockPicPuzzle>().maxCount = maxCount;
            _puzzle.GetComponent<LockPicPuzzle>().moveSpeed = moveSpeed;
        } else
        {
            Open();
        }
    }

    public void Open()
    {
        PuzzleSystem puzzleSystem = new PuzzleSystem();
        puzzleSystem._bisClear = true;
        puzzleSystem.Stage = GameInstance.instance.currentStage;
        puzzleSystem.Number = number;
        GameInstance.instance._bisCurrentTreasure.Add(puzzleSystem);
        Instantiate(treasure, this.transform.position, Quaternion.identity);
        ChestDestroy();
    }

    public void ChestDestroy()
    {
        Destroy(gameObject);
    }

}
