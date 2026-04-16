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
            Debug.Log("게임매니저 instance 새로운 할당");
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(this.gameObject);
        UpdateScore(0);
    }


    // Update is called once per frame
    //플레이어 currentDelay 시각화
    void LateUpdate()
    {
        if(delay != null)
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
