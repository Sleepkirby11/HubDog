using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NUnit.Framework.Internal.Commands;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public Player player;
    public PoolManager pool;
    public Image[] Lifes;

    public GameObject Menu;
    public Slider delay;

    public Goal goal;
    public TMP_Text goalText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        //싱글톤 null 체크
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(this.gameObject);
        UpdateScore(0);
    }


    // Update is called once per frame
    //플레이어 currentDelay 시각화
    void LateUpdate()
    {
        if(instance != null)
        {
            delay.value = player.GetFloat("currentDelay") / player.GetFloat("fireDelay");
        }
    }

    //HP UI 업데이트
    public void UpdateLifeBar()
    {
        for(int i = 0; i < player.GetInt("maxHp"); i++)
        {
            Lifes[i].gameObject.SetActive(false);
        }
        for(int i = 0; i < player.GetInt("hp"); i++)
        {
            Lifes[i].gameObject.SetActive(true);
        }
    }
    public void UpdateScore(int score)
    {
        if(goal.reQScore > 0)
        {
            player.score += score;
            goalText.text = "처치한 적 " + player.score + " / " + goal.reQScore;
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

    public bool IsPlayerCanGo()
    {
        if (player.score >= goal.reQScore)
            return true;
        else
            return false;
    }

    public void NextStage()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //종료
    public void OnButtonExit()
    {
        Application.Quit();
    }
}
