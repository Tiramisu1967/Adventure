using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class BrokenDoorPuzzle : MonoBehaviour
{
    public DoorBrokenInteraction borken;
    public Canvas puzzleCanvas;
    public RectTransform uiPanel;
    public GameObject button;
    public GameObject clickEffect;
    public float maxCount;
    public float currentCount;
    int brokenCount;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(ClickObjectSpwan());
    }


    public void CountUp(GameObject _click)
    {
        currentCount++;
        brokenCount++;
        Vector3 effectPos = _click.transform.position;
        GameObject effort = Instantiate(clickEffect, effectPos, Quaternion.identity, puzzleCanvas.transform);
        Destroy(_click);
        Destroy(effort, 0.5f);
        StartCoroutine(borken.Shake(1f));
        if (brokenCount % 2 == 0 && brokenCount != 0)
        {
            borken.BrokenObject();
        }
    }

    public IEnumerator ClickObjectSpwan()
    {
        GameInstance.instance._gameManger.player._bisStop = true;
        while (currentCount < maxCount)
        {
            yield return new WaitForSecondsRealtime(Random.Range(0.1f, 1f));
            float randomX = Screen.width * Random.Range(0.1f, 0.9f);
            float randomY = Screen.height * Random.Range(0.1f, 0.9f);
            Vector2 randomScreenPos = new Vector2(randomX, randomY);

            // UI 월드 좌표 변환
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(uiPanel, randomScreenPos, puzzleCanvas.worldCamera, out localPoint);

            GameObject _click = Instantiate(button, uiPanel);

            GameObject clickButton = _click;
            clickButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => CountUp(clickButton));
            RectTransform rectTransform = _click.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = localPoint;
            Destroy(_click, Random.Range(1f, 1.5f));
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Destroy(this.gameObject);
    }
}
