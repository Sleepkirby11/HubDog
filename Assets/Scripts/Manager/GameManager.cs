using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NUnit.Framework.Internal.Commands;
using System.Collections;
using Unity.Cinemachine;
using JetBrains.Annotations;

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

        
    }



    // Update is called once per frame
    //플레이어 currentDelay 시각화
    void LateUpdate()
    {
        if(instance != null)
        {
            delay.value = player.CurrentDelay / player.FireDelay;
        }
    }

    //HP UI 업데이트
    public void UpdateLifeBar()
    {
        for(int i = 0; i < player.MaxHp; i++)
        {
            Lifes[i].gameObject.SetActive(false);
        }
        for(int i = 0; i < player.Hp; i++)
        {
            Lifes[i].gameObject.SetActive(true);
        }
    }
    public void UpdateScore(int score)
    {
        if (goal == null)
            return;
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

    public void NewStage(int plusStage)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + plusStage);
    }

    //종료
    public void OnButtonExit()
    {
        Application.Quit();
    }

    public void HitEffectSpawn(Transform spawnTsf)
    {
        GameObject hitEffect = GameManager.instance.pool.Get(10);
        hitEffect.transform.position = spawnTsf.position;
    }

    public IEnumerator DieBoth()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1.5f);
        Time.timeScale = 1;

        if (player.Hp <= 0)
        {
            yield return new WaitForSeconds(5f);

            NewStage(0);
        }
        else
        {
            
            yield return new WaitForSeconds(5f);

            NewStage(1);
        }
    }
}
