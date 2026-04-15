using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NUnit.Framework.Internal.Commands;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public PoolManager pool;
    public Image[] Lifes;

    public GameObject Menu;
    public Slider delay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //싱글톤 null 체크
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    // Update is called once per frame
    //플레이어 currentDelay 시각화
    void LateUpdate()
    {
        delay.value = player.currentDelay / player.FireDelay;
    }

    //HP UI 업데이트
    public void UpdateLifeBar()
    {
        for(int i = 0; i < player.maxHp; i++)
        {
            Lifes[i].gameObject.SetActive(false);
        }
        for(int i = 0; i < player.hp; i++)
        {
            Lifes[i].gameObject.SetActive(true);
        }
    }

    //일시정지 메뉴
    public void ActiveMenu()
    {
        if(Menu.activeSelf == false)
        {
            Menu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            Menu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    //종료
    public void OnButtonExit()
    {
        Application.Quit();
    }
}
