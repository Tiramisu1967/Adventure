using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    [System.Serializable]
    public struct PuzzleSystem
    {
        public int Stage;
        public int Number;
        public bool _bisClear;
    }

    [HideInInspector] public static GameInstance instance;
    [HideInInspector] public GameManager _gameManger;

    [Header("플레이어")]
    public bool _bisFreeShop;
    public int playerHp;
    public float playerOxygen;
    public int money;

    [Space(10)]
    [Header("플레이 체크")]
    public int currentStage;
    public float playTime;
    public bool[] _bisMapClearChecking;
    public bool[] _bisCurrentMapClearChecking;
    public List<PuzzleSystem> _bisCurrentTreasure = new List<PuzzleSystem>();
    public List<PuzzleSystem> _bisTreasure = new List<PuzzleSystem>();
    public AudioSource backgroundMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        backgroundMusic.Play();
    }
}
