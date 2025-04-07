using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : BaseManager
{

    [Header("알림")]
    public GameObject alertPos;
    public GameObject alertObject;
    public bool _bisInteraction;
    public GameObject interactionbox;
    public TextMeshProUGUI interactiontext;
    public string interactionString;
    public GameObject stageAlert;
    public GameObject damageEffort;
    [Space(15)]
    public Image playerHp;
    public Image playerOxygen;
    public Image bag;
    public Image weightIcon;
    public Image notJump;
    public TextMeshProUGUI playTimeText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI bagCountText;
    public TextMeshProUGUI bagWeightText;
    public GameObject quickInventoryPos;
    public GameObject quickInventory;


    [Header("게임 오버 창")]
    public GameObject hpOver;
    public GameObject oxygenOver;
    // Start is called before the first frame update
    void Start()
    {
        InventoryUISetting();
        stageAlert.SetActive(true);
    }

    public void InventoryUISetting()
    {
        int temp = 0;
        for (int i = quickInventoryPos.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(quickInventoryPos.transform.GetChild(i).gameObject);
        }

        while (quickInventoryPos.transform.childCount < Inventory.instance.maxBag)
        {
            GameObject items = Instantiate(quickInventory, quickInventoryPos.transform);
            if (temp < Inventory.instance.currentItems.Count)
            {
                Image image = items.transform.GetChild(1).gameObject.transform.GetComponent<Image>();
                Color color = image.color;
                color.a = 1f;
                items.transform.GetChild(1).gameObject.transform.GetComponent<Image>().color = color;
                items.transform.GetChild(1).gameObject.transform.GetComponent<Image>().sprite = Inventory.instance.currentItems[temp].itemImage;
            } else
            {
                Image image = items.transform.GetChild(1).gameObject.transform.GetComponent<Image>();
                items.transform.GetChild(1).gameObject.transform.GetComponent<Image>().sprite = null;
                Color color = image.color;
                color.a = 0f;
                items.transform.GetChild(1).gameObject.transform.GetComponent<Image>().color = color;
            }
                temp++;
        }
    }

    public void GameOver(bool _bisHp)
    {
        if (_bisHp)
        {
            hpOver.gameObject.SetActive(true);
        }
        else
        {
            oxygenOver.gameObject.SetActive(true);
        }
        StartCoroutine(OverDelay());
    }

    public IEnumerator OverDelay()
    {
        yield return new WaitForSeconds(1.5f);
        gameManger.GameReset();
    }


    void Update()
    {
        playerHp.fillAmount = (float) gameManger.player.currentHp / (float) gameManger.player.maxHp;
        playerOxygen.fillAmount = (float) gameManger.player.currentOxygen / (float) gameManger.player.maxOxygen;
        bag.fillAmount = (float) Inventory.instance.currentItems.Count / (float) Inventory.instance.maxBag;
        moneyText.text = $"{GameInstance.instance.money} G";
        bagCountText.text = $"{Inventory.instance.currentItems.Count} / {Inventory.instance.maxBag}";
        bagWeightText.text = $"{Inventory.instance.weight} / {Inventory.instance.maxWeight}";
        if (Inventory.instance.currentItems.Count  >= Inventory.instance.maxBag)
        {
            weightIcon.gameObject.SetActive(true);
        } else
        {
            weightIcon.gameObject.SetActive(false);
        }
        if (Inventory.instance.maxWeight < Inventory.instance.weight)
        {
            notJump.gameObject.SetActive(true);
        } else
        {
            notJump.gameObject.SetActive(false);
        }
        playTimeText.text = $"{(int)(GameInstance.instance.playTime / 60)} : {GameInstance.instance.playTime % 60:F0}";
        for(int i = 0; i < quickInventoryPos.transform.childCount; i++)
        {
            quickInventoryPos.transform.GetChild(i).GetComponent<Image>().color = Color.white;
        }
        quickInventoryPos.transform.GetChild(Inventory.instance.select).GetComponent<Image>().color = Color.red;

        if (_bisInteraction)
        {
            interactionbox.SetActive(true);
            interactiontext.text = $"{interactionString}";
        } else
        {
            interactionbox.SetActive(false);
        }
    }
    

    public void Alert(string _alertText)
    {
        GameObject _alert = Instantiate(alertObject, alertPos.transform);
        _alert.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = $"{_alertText}";
        Destroy(_alert, 1.5f);
    }

    public IEnumerator DamageEffort()
    {
        damageEffort.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        damageEffort.SetActive(false);
    }

}
