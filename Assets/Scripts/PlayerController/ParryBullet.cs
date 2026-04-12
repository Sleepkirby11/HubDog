using UnityEngine;

public class ParryBullet : MonoBehaviour
{
    Rigidbody2D rigid;
    public int damage;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        rigid.linearVelocity = transform.up * 20;

        if (transform.position.y > 10)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().hp -= damage;
            gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }
}
