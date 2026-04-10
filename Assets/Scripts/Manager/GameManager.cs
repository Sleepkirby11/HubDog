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
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        delay.value = player.currentDelay / player.FireDelay;
    }

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


    public void OnButtonExit()
    {
        Application.Quit();
    }
}
