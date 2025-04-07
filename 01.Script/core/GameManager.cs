using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("메니저 세팅")]
    public PlayerMoveMent player;
    public PlayerUIManager UI;
    public RankingManger ranking;

    [Header("맵 세팅")]
    public GameObject[] treasure;
    public GameObject[] door;

    [Header("게임 클리어")]
    public Canvas RankingInputCanvas;
    public TextMeshProUGUI PlayerScore;
    public TextMeshProUGUI PlayTiem;
    public TextMeshProUGUI MoneyScore;
    // Start is called before the first frame update
    void Start()
    {
        GameInstance.instance._gameManger = this;
        if (player != null) player.init(this);
        if (UI != null) UI.init(this);
        if (ranking != null) ranking.init(this);

        for(int i = 0; i  < treasure.Length; i++)
        {
            treasure[i].GetComponent<ChestInteraction>().number = i;
        }

        for(int i = 0; i < GameInstance.instance._bisCurrentTreasure.Count; i++)
        {
            if (GameInstance.instance._bisCurrentTreasure[i].Stage == GameInstance.instance.currentStage && GameInstance.instance._bisCurrentTreasure[i]._bisClear == true)
            {
                Destroy(treasure[GameInstance.instance._bisCurrentTreasure[i].Number]);
            }
        }

        if (GameInstance.instance._bisCurrentMapClearChecking[GameInstance.instance.currentStage])
        {
            for(int i = 0; i< door.Length; i++)
            {
                Destroy((GameObject)door[i]);
            }
        }

    }


    private void Update()
    {
        GameInstance.instance.playTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.F1))
        {
            player.currentHp = player.maxHp;
            player.currentOxygen = player.maxOxygen;
            UI.Alert("산소 및 체력 회복");
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameInstance.instance._bisFreeShop = GameInstance.instance._bisFreeShop ? false : true;
            UI.Alert($"상점 치트 : {GameInstance.instance._bisFreeShop}");
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            UI.Alert($"리셋");
            GameReset();
        }
        if (Input.GetKeyDown(KeyCode.F4) && GameInstance.instance.currentStage < GameInstance.instance._bisCurrentMapClearChecking.Length -1)
        {
            UI.Alert($"다음 스테이지");
            GameInstance.instance.currentStage++;
            SceneMove();
        }
        if (Input.GetKeyDown(KeyCode.F5))
        {
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            if (Time.timeScale == 0) UI.Alert($"시간 정지");
            else UI.Alert($"재생");
        }
    }

    public void GameClear()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SaleItem();

        GameInstance.instance._gameManger.player._bisStop = true;
        Time.timeScale = 0;

        float bonusTime = Mathf.Max(0f, 210f - GameInstance.instance.playTime);
        float totalScore = GameInstance.instance.money + bonusTime;

        PlayerScore.text = $"{(int)totalScore}";
        PlayTiem.text = $"{(GameInstance.instance.playTime / 60):00} : {(GameInstance.instance.playTime % 60):00}";
        MoneyScore.text = $"{GameInstance.instance.money}";

        RankingInputCanvas.gameObject.SetActive(true);
        UI.gameObject.SetActive(true);
        RankingInputCanvas.GetComponent<RankingManger>().playerScore = (int)totalScore;
    }


    void SaleItem()
    {
        for (int i = Inventory.instance.currentItems.Count - 1; i >= 0; i--)
        {
            if (Inventory.instance.currentItems[i].type == ItemType.forage)
            {
                GameInstance.instance.money += Inventory.instance.currentItems[i].price;
                Inventory.instance.weight -= Inventory.instance.currentItems[i].weight;
                Inventory.instance.currentItems.RemoveAt(i);
            }
        }
    }

    public void GameReset()
    {
        GameInstance.instance.currentStage = 1;

        for(int i = 0; i < GameInstance.instance._bisCurrentMapClearChecking.Length; i++)
        {
            GameInstance.instance._bisCurrentMapClearChecking[i] = GameInstance.instance._bisMapClearChecking[i];
        }

        Inventory.instance.currentItems.Clear();
        Inventory.instance.currentItems = new List<BaseItem>(Inventory.instance.items);
        GameInstance.instance._bisCurrentTreasure = new List<GameInstance.PuzzleSystem>(GameInstance.instance._bisTreasure);
        GameInstance.instance.playerHp = 0;
        GameInstance.instance.playerOxygen = 0;
        SceneManager.LoadScene("Stage 1");
    }

    public void SceneMove()
    {
        GameInstance.instance.playerHp = player.currentHp;
        GameInstance.instance.playerOxygen = player.currentOxygen;
        if(GameInstance.instance.currentStage > 0 && GameInstance.instance._bisCurrentMapClearChecking.Length > GameInstance.instance.currentStage)
        {
            GameInstance.instance._bisCurrentMapClearChecking[GameInstance.instance.currentStage - 1] = true;
        }
        Debug.Log(GameInstance.instance.currentStage);
        SceneManager.LoadScene($"Stage {GameInstance.instance.currentStage}");
    }
}
