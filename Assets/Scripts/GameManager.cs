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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
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
        }
        else
            Menu.SetActive(false);
    }


    public void OnButtonExit()
    {
        Application.Quit();
    }
}
