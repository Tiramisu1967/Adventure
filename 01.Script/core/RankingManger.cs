using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RankingManger : BaseManager
{
   
    public AudioSource AudioSource;
    public Animator animator;
    
    public GameObject mainCanvas;
    public GameObject rankingCanvas;
    public GameObject helpCanvas;

    #region Ranking
    [System.Serializable]
    public class Ranking
    {
        public int Score;
        public string Name;
    }

    [Space(15)]
    [Header("랭킹 입력 및 출력")]
    public string path;
    public int playerScore;
    public TMP_InputField nameFied;
    public TextMeshProUGUI[] rankingNameText;
    public TextMeshProUGUI[] rankingScoreText;
    public List<Ranking> rankList = new List<Ranking>();

    public void RankingInit()
    {
        string exePath = Application.dataPath;
#if UNITY_EDITOR
        path = Path.Combine(exePath, "ranking.txt");
#else
        path = Path.Combine(Directory.GetParent(exePath).FullName, "ranking.txt");
#endif
    }

    private void RankingVeiw()
    {
        rankList.Clear();
        RankingSetting();
        for (int i = 0; i < 5; i++)
        {
            rankingNameText[i].text = $"{rankList[i].Name}";
            rankingScoreText[i].text = $"{rankList[i].Score}";
        }
    }

    public void RankingSet()
    {
        AudioSource.Play();
        if (nameFied.text != null)
        {
            rankList.Clear();
            Ranking player = new Ranking();
            player.Name = nameFied.text;
            player.Score = playerScore;
            rankList.Add(player);
            RankingSetting();
            RankSave();
            Time.timeScale = 1;
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void RankingSetting()
    {
        RankingInit();
        RankingLoad();
    }

    public void RankingLoad()
    {
        if (!File.Exists(path))
        {
            RankSave();
        }
        string[] lines = File.ReadAllLines(path);
        foreach (string line in lines)
        {
            string[] paths = line.Split(",");
            if(paths.Length == 2)
            {
                string name = paths[0];
                if (int.TryParse(paths[1], out int score))
                {
                    Ranking _ranking = new Ranking();
                    _ranking.Name = name;
                    _ranking.Score = score;
                    rankList.Add(_ranking);
                }
            }
        }
        rankList.OrderByDescending(x => x.Score).Take(5).ToList();
    }

    public void RankSave()
    {
        List<string> lines = new List<string>();
        for (int i = 0; i < 5; i++)
        {
            if (rankList.Count > i)
            {
                lines.Add($"{rankList[i].Name},{rankList[i].Score}");
            } else
            {
                lines.Add("none,0");
            }
        }
        File.WriteAllLines(path, lines);
    }
    #endregion


    public void GameStart()
    {
        if (GameInstance.instance != null) GameInstance.instance.currentStage = 1;
        AudioSource.Play();
        mainCanvas.gameObject.SetActive(false);
        animator.SetBool("_bisStart", true);
        StartCoroutine(delay());
    }

    public IEnumerator delay()
    {
        yield return new WaitForSeconds(5.3f);
        SceneManager.LoadScene("Stage 1");
    }

    public void Rank()
    {
        AudioSource.Play();
        mainCanvas.gameObject.SetActive(false);
        helpCanvas.gameObject.SetActive(false);
        rankingCanvas.gameObject.SetActive(true);
        RankingVeiw();
    }

    public void Help()
    {
        AudioSource.Play();
        mainCanvas.gameObject.SetActive(false);
        helpCanvas.gameObject.SetActive(true);
        rankingCanvas.gameObject.SetActive(false);
    }

    public void Close()
    {
        AudioSource.Play();
        mainCanvas.gameObject.SetActive(true);
        helpCanvas.gameObject.SetActive(false);
        rankingCanvas.gameObject.SetActive(false);
    }

    public void GameExit()
    {
        AudioSource.Play();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
