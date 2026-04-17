using Unity.VisualScripting;
using UnityEngine;

public class Items : MonoBehaviour
{
    //아이템 타입
    //0: Heal
    //1: Attack Speed Up
    //2: Always Parry
    public int type;
    public Sprite[] sprites;
    SpriteRenderer rend;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sprite = sprites[type];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            switch(type)
            {
                case 0:
                    GameManager.instance.player.Heal(1);
                    break;
                case 1:
                    GameManager.instance.player.ATKUp(5);
                    break;
                case 2:
                    GameManager.instance.player.AlwayDef(10);
                    break;
            }

            this.gameObject.SetActive(false);
        }
    }
}
