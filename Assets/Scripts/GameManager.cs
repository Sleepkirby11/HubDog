using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public PoolManager pool;
    public TMP_Text hpbar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        hpbar.text = "HP: " + player.hp + "/" + player.maxHp;
    }

}
