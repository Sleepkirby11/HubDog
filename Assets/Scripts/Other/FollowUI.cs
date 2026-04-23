using UnityEngine;
using UnityEngine.UI;

public class FollowUI : MonoBehaviour
{

    RectTransform rect;

    public GameObject otherBG;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);

        if (GameManager.instance.player.AlwaysParryTime > 0)
        {
            otherBG.SetActive(true);
            UpdateColor("#FFB400", otherBG);
        }
        else if (GameManager.instance.player.ATKSpeedTime > 0)
        {
            otherBG.SetActive(true);
            UpdateColor("#17FFCB", otherBG);
        }
        else
            otherBG.SetActive(false);
    }
    
    void UpdateColor(string hexCode, GameObject BG)
    {
        if (ColorUtility.TryParseHtmlString(hexCode, out Color color))
        {
            BG.gameObject.GetComponent<Image>().color = color;
        }
    }
}
