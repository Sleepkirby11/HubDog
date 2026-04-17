using UnityEngine;

public class Ting : MonoBehaviour
{
    BoxCollider2D col;
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //초기화
        col = GetComponent<BoxCollider2D>();
    }

    //Enable 시 트리거 작동
    void OnEnable()
    {
        //초기화
        anim = GetComponent<Animator>();
        this.anim.SetTrigger("Ting");
    }

    //패링 여부 확인
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger == true
            && collision.gameObject.CompareTag("EnemyBullet")
            && GameManager.instance.player.isParrying)
        {
            GameObject bullet = GameManager.instance.pool.Get(3);
            bullet.transform.position = transform.position;
            bullet.transform.eulerAngles = collision.gameObject.GetComponent<EnemyBullet>().ReverseRotate();
            collision.gameObject.SetActive(false);
        }

    }

    //Animator에서 Clip의 마지막 프레임에 호출되는 이벤트 함수
    void Disabled()
    {
        this.gameObject.SetActive(false);
    }
}
