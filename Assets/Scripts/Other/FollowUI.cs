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
    void FixedUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);

        if(GameManager.instance.player.GetFloat("ATKspeedTime") > 0)
            otherBG.SetActive(true);
        else
            otherBG.SetActive(false);
    }
}
