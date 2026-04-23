using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    Rigidbody2D rigid;
    public int damage;
    public float speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        rigid.linearVelocity = transform.up * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            GameManager.instance.HitEffectSpawn(this.transform);

            collision.gameObject.GetComponent<Enemy>().Damaged(damage);
            gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Boss"))
        {
            GameManager.instance.HitEffectSpawn(this.transform);

            collision.gameObject.GetComponent<Boss>().Damaged(damage);
            gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Ground"))
        {
            GameManager.instance.HitEffectSpawn(this.transform);

            gameObject.SetActive(false);
        }
    }
}
