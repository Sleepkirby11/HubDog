using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
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

        if (localgoal != null)
            inst.goal = localgoal;
        if (localgoalText != null)
            inst.goalText = localgoalText;
        inst.UpdateScore(0);
    }
}
