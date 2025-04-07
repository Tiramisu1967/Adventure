using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LockPicPuzzle : MonoBehaviour
{
    public ChestInteraction treasure;
    public GameObject hitZone;
    private GameObject hitArea;
    private Vector3 arrowPos;
    public GameObject pos;
    public GameObject arrowObject;
    public RectTransform left;
    public RectTransform right;
    public Image countBar;
    public TextMeshProUGUI countText;
    public float moveSpeed;
    public float maxCount;
    public float currentCount;

    private void Start()
    {
        HitAreaSpwan();
        GameInstance.instance._gameManger.player._bisStop = true;
    }

    public void HitAreaSpwan()
    {
        float X = Random.Range(left.anchoredPosition.x, right.anchoredPosition.x);
        Vector3 RandomArea = new Vector3(X, 0, 1);
        hitArea = Instantiate(hitZone, pos.transform);
        hitArea.transform.GetComponent<RectTransform>().localPosition = RandomArea;
        arrowPos = arrowObject.GetComponent<RectTransform>().localPosition; 
    }

    public bool _bisLeft;

    private void Update()
    {
        HitBoxCheck();
        if(currentCount <= 0)
        {
            currentCount = 0;
            countBar.fillAmount = 0f;
            countText.text = "0%";
        }
        else
        {
            countBar.fillAmount = (currentCount / maxCount);
            countText.text =$"{((currentCount / maxCount) * 100):f0} %";
        }

        if (_bisLeft)
        {
            arrowPos.x -= Time.deltaTime * moveSpeed * 100;
            arrowObject.GetComponent<RectTransform>().localPosition = arrowPos;
            if(arrowPos.x < left.anchoredPosition.x)
            {
                arrowObject.GetComponent<RectTransform>().localPosition = left.anchoredPosition;
                _bisLeft = false;
            }
        } else
        {
            arrowPos.x += Time.deltaTime * moveSpeed * 100;
            arrowObject.GetComponent<RectTransform>().localPosition = arrowPos;
            if (arrowPos.x > right.anchoredPosition.x)
            {
                arrowObject.GetComponent<RectTransform>().localPosition = right.anchoredPosition;
                _bisLeft = true;
            }
        }
    }

    public void HitBoxCheck()
    {
        if (Input.GetMouseButtonDown(0))
        {
            float moving = arrowObject.GetComponent<RectTransform>().anchoredPosition.x;
            float middleLine = hitArea.GetComponent<RectTransform>().anchoredPosition.x;
            float middleArea = hitArea.GetComponent<RectTransform>().rect.width / 2;

            if (moving >= middleLine - middleArea && moving <= middleLine + middleArea)
            {
                currentCount++;
                Destroy(hitArea);
                if (currentCount < maxCount)
                {
                    HitAreaSpwan();
                }
                else
                {
                    treasure.Open();
                    GameInstance.instance._gameManger.player._bisStop = false;
                    Destroy(this.gameObject);
                }
            }
            else if (currentCount > 0)
            {
                currentCount--;
            }
        }
    }
}
