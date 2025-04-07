using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public struct itemPrefab
    {
        public string name;
        public GameObject prefab;
    }

    public static Inventory instance;
    public int bagLevel;
    public int oxygenLevel;
    public int maxBag;
    public int maxWeight;
    public List<BaseItem> items = new List<BaseItem>();
    public List<BaseItem> currentItems = new List<BaseItem>();
    public List<itemPrefab> itemPrefabs = new List<itemPrefab>();
    public int weight;
    public int select;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    public void GiveItem()
    {
        Debug.Log(currentItems.Count > select);
        if (currentItems.Count > select)

        {
            Vector3 spwan = GameInstance.instance._gameManger.player.head.transform.position;
            Debug.Log(spwan);
            spwan += GameInstance.instance._gameManger.player.head.transform.forward * 1f;
            Debug.Log(spwan);
            GameObject _giveItem = Instantiate(currentItems[select].ItemObject, spwan, Quaternion.identity);
            Debug.Log("_giveItem");
            weight -= currentItems[select].weight;
            ItemDelete(select);
        }
    }

    public void GetItem(BaseItem _Item)
    {
        _Item.ItemObject = itemPrefabs.Find(itemPrefabs => itemPrefabs.name == _Item.itemName).prefab;
        currentItems.Add(_Item);
        weight += _Item.weight;
        if(GameInstance.instance._gameManger != null)
        {
            GameInstance.instance._gameManger.UI.InventoryUISetting();
        }
    }

    

    public void ItemUse()
    {
        if (currentItems[select].type == ItemType.expendables)
        {
            switch (currentItems[select].use)
            {
                case global::ItemUse.Medicine:
                    ItemDelete(select);
                    Medicine();
                    break;
                case global::ItemUse.Oxygen:
                    ItemDelete(select);
                    Oxygen();
                    break;
                case global::ItemUse.Booster:
                    ItemDelete(select);
                    Boost(3);
                    break;
                case global::ItemUse.Strong_Booster:
                    ItemDelete(select);
                    Boost(6);
                    break;
                case global::ItemUse.Compass:
                    Compass();
                    break;
                case global::ItemUse.Deodorant:
                    ItemDelete(select);
                    Deodorant();
                    break;
            }
        }
    }

    public void ItemDelete(int _number){
        weight -= currentItems[_number].weight;
        currentItems.RemoveAt(_number);
        GameInstance.instance._gameManger.UI.InventoryUISetting();
    }

    public void Medicine()
    {
        if(GameInstance.instance._gameManger.player.currentHp >= GameInstance.instance._gameManger.player.maxHp - 25) GameInstance.instance._gameManger.player.currentHp = GameInstance.instance._gameManger.player.maxHp;
        else GameInstance.instance._gameManger.player.currentHp += 25;
    }

    public void Oxygen()
    {
        if (GameInstance.instance._gameManger.player.currentOxygen >= GameInstance.instance._gameManger.player.maxOxygen - 30) GameInstance.instance._gameManger.player.currentOxygen = GameInstance.instance._gameManger.player.maxOxygen;
        else GameInstance.instance._gameManger.player.currentOxygen += 30;
    }

    public void Boost(int _strong)
    {
        GameInstance.instance._gameManger.player.ItemSpeed = _strong;
    }

    GameObject targetTreasure = null;
    float delay = 3f;
    float currentDelay = 0f;

    public void Compass()
    {
        GameObject[] treasures = GameObject.FindGameObjectsWithTag("Treasure");
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;
        Transform compass = GameInstance.instance._gameManger.player.compass.transform;

        foreach (GameObject treasure in treasures)
        {
            float dist = Vector3.Distance(compass.position, treasure.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = treasure;
            }
        }

        if (nearest != null)
        {
            ItemDelete(select);
            targetTreasure = nearest;
            currentDelay = delay; 
        }
    }

    private void Update()
    {
        if (currentDelay > 0 && targetTreasure != null)
        {
            GameInstance.instance._gameManger.player.compass.SetActive(true);
            currentDelay -= Time.deltaTime;

            Transform compass = GameInstance.instance._gameManger.player.compass.transform;
            Vector3 lookPos = targetTreasure.transform.position;
            lookPos.y = compass.position.y; // 수평 회전만

            compass.LookAt(lookPos);
        } else if(GameInstance.instance._gameManger != null)
        {
            GameInstance.instance._gameManger.player.compass.SetActive(false);
        }
    }

    public void Deodorant()
    {
        GameInstance.instance._gameManger.player.targetDuration = 30;
    }
}
