using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubManager : MonoBehaviour
{
    public Player localplayer;
    public PoolManager localpool;
    public Image[] localLifes;

    public GameObject localMenu;
    public Slider localdelay;

    public Goal localgoal;
    public TMP_Text localgoalText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameManager inst = GameManager.instance;
        if (inst.gameObject == null)
            return;
        inst.player = localplayer;
        inst.pool = localpool;
        inst.Lifes = localLifes;
        inst.Menu = localMenu;
        inst.delay = localdelay;
        inst.goal = localgoal;
        inst.goalText = localgoalText;
    }
}
