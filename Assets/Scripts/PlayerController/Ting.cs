using UnityEngine;

public class Ting : MonoBehaviour
{
    BoxCollider2D col;
    Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        anim = GetComponent<Animator>();
        this.anim.SetTrigger("Ting");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet") && GameManager.instance.player.isParrying)
        {
            GameObject bullet = GameManager.instance.pool.Get(3);
            bullet.transform.position = transform.position;
            bullet.transform.eulerAngles = collision.gameObject.GetComponent<EnemyBullet>().ReverseRotate();
            collision.gameObject.SetActive(false);
        }

    }
    void Disabled()
    {
        this.gameObject.SetActive(false);
    }
}
