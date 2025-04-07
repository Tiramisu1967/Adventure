using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("�˸�")]
    public GameObject alertPos;
    public GameObject alertObject;

    [Header("����")]
    public AudioSource sound;
    public TextMeshProUGUI money;
    public GameObject mainShop;
    public GameObject upgradeShop;
    public GameObject itemShop;
    public List<BaseItem> items;
    public List<string> itemsExplanation;

    [Header("���� ����")]
    public GameObject explanation;
    public Image itemSprite;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemMoneyText;
    public TextMeshProUGUI itemExplanation;

    [Header("���� ���׷��̵� ����")]
    public Sprite bagSprite;
    public int[] bagPraice;
    public string[] bagExplanation;

    [Header("����� ���׷��̵� ����")]
    public Sprite oxygenSprite;
    public int[] oxygenPraice;
    public string[] oxygenExplanation;

    bool _bisBag;
    bool _bisOxygen;
    bool _bisItem;
    int itemnumber;



    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameInstance.instance._bisTreasure = GameInstance.instance._bisCurrentTreasure;
        GameInstance.instance._bisMapClearChecking = GameInstance.instance._bisCurrentMapClearChecking;
        SaleItem();
    }

    void SaleItem()
    {
        for(int i = Inventory.instance.currentItems.Count - 1; i >= 0; i--)
        {
            if (Inventory.instance.currentItems[i].type == ItemType.forage)
            {
                GameInstance.instance.money += Inventory.instance.currentItems[i].price;
                Inventory.instance.weight -= Inventory.instance.currentItems[i].weight;
                Inventory.instance.currentItems.RemoveAt(i);
            }
        }
        Inventory.instance.items.Clear();
        Inventory.instance.items = new List<BaseItem>(Inventory.instance.currentItems); 
    }


    public void Gameplay()
    {
        Debug.Log("����");
        sound.Play();
        GameInstance.instance.currentStage = 1;
        Inventory.instance.items.Clear();
        Inventory.instance.items = new List<BaseItem>(Inventory.instance.currentItems);
        GameInstance.instance.playerHp = 0;
        GameInstance.instance.playerOxygen = 0;
        SceneManager.LoadScene("Stage 1");
    }
    public void UpgradeVeiw()
    {
        mainShop.SetActive(false);
        itemShop.SetActive(false);
        upgradeShop.SetActive(true);
        sound.Play();
    }
    public void ItemVeiw()
    {
        mainShop.SetActive(false);
        itemShop.SetActive(true);
        upgradeShop.SetActive(false);
        sound.Play();
    }

    public void Close()
    {
        mainShop.SetActive(true);
        itemShop.SetActive(false);
        upgradeShop.SetActive(false);
        explanation.gameObject.SetActive(false);
        sound.Play();
    }

    public void GameExit()
    {
        sound.Play();
        Debug.Log("����");
        Destroy(GameInstance.instance.gameObject);
        Destroy(Inventory.instance.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void BagUpgrade()
    {

        sound.Play();
        _bisBag = true;
        _bisOxygen = false;
        _bisItem = false;
        if(Inventory.instance.bagLevel < 2) itemMoneyText.text = $"{bagPraice[Inventory.instance.bagLevel]}";
        else itemMoneyText.text = $"";
        itemNameText.text = $"����";
        itemSprite.sprite = bagSprite;
        itemExplanation.text = bagExplanation[Inventory.instance.bagLevel];
        explanation.gameObject.SetActive(true);
    }
    public void OxygenUpgrade()
    {

        sound.Play();
        _bisBag = false;
        _bisOxygen = true;
        _bisItem = false;
        if (Inventory.instance.oxygenLevel < 3) itemMoneyText.text = $"{oxygenPraice[Inventory.instance.oxygenLevel]}";
        else itemMoneyText.text = $"";
        switch (Inventory.instance.oxygenLevel)
        {
            case 0:
                itemNameText.text = $"���п� �����";
                break;
            case 1:
                itemNameText.text = $"�߾п� �����";
                break;
            case 2:
                itemNameText.text = $"��п� �����";
                break;
            default:
                itemNameText.text = $"X";
                break;
        }
        itemSprite.sprite = oxygenSprite;
        if(Inventory.instance.oxygenLevel > 3)
        {
            itemExplanation.text = "��� ���׷��̵带 �Ϸ��Ͽ����ϴ�.";
        } else
        {
            itemExplanation.text = oxygenExplanation[Inventory.instance.oxygenLevel];
        }
        explanation.gameObject.SetActive(true);
    }

    public void Explanation(int number)
    {

        sound.Play();
        _bisBag = false;
        _bisOxygen = false;
        _bisItem = true;
        BaseItem _item = items[number];
        itemMoneyText.text = $"{_item.price}";
        itemNameText.text = $"{_item.itemName}";
        itemSprite.sprite = _item.itemImage;
        itemExplanation.text = itemsExplanation[number];
        explanation.gameObject.SetActive(true);
        itemnumber = number;
    }

    public void GetItem()
    {
        Debug.Log("Click");
        if (_bisBag && (GameInstance.instance.money >= bagPraice[Inventory.instance.bagLevel] || GameInstance.instance._bisFreeShop))
        {
            if(!GameInstance.instance._bisFreeShop) GameInstance.instance.money -= bagPraice[Inventory.instance.bagLevel];
            Inventory.instance.bagLevel++;
            Inventory.instance.maxBag += 2;
            BagUpgrade();
            Alert("������ ���������� ��ȭ �Ǿ����ϴ�.");
        } else if(_bisOxygen && (GameInstance.instance.money >= oxygenPraice[Inventory.instance.oxygenLevel] || GameInstance.instance._bisFreeShop))
        {
            if (!GameInstance.instance._bisFreeShop) GameInstance.instance.money -= oxygenPraice[Inventory.instance.oxygenLevel];
            Inventory.instance.oxygenLevel++;
            OxygenUpgrade();
            Alert("������� ���������� ��ȭ �Ǿ����ϴ�.");
        } else if (_bisItem && (GameInstance.instance.money >= items[itemnumber].price || GameInstance.instance._bisFreeShop))
        {
            if(Inventory.instance.currentItems.Count < Inventory.instance.maxBag)
            {
                Inventory.instance.GetItem(items[itemnumber]);
                if (!GameInstance.instance._bisFreeShop) GameInstance.instance.money -= items[itemnumber].price;
                Alert($"{items[itemnumber].itemName}�� \n�����߽��ϴ�.");
                sound.Play();
            } else
            {
                Alert($"���� ������ �����մϴ�.");
                sound.Play();
            }

        }
        else
        {
            Alert("���� �����մϴ�.");
            sound.Play();
        }
    }


    public void Alert(string _alertText)
    {
        GameObject _alert = Instantiate(alertObject, alertPos.transform);
        _alert.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = $"{_alertText}";
        Destroy( _alert, 1.5f );
    }

    private void Update()
    {
        money.text = $"���� �ݾ�: {GameInstance.instance.money} G";
    }
}
