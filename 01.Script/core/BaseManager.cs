using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    protected GameManager gameManger;

    public GameManager GameManager { get { return gameManger; } }
    public virtual void init(GameManager _gameManager)
    {
        gameManger = _gameManager;
    }
}
