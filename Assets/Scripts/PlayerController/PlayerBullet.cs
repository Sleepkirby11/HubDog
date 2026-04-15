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
        rigid.transform.position += transform.up * Time.deltaTime * speed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Damaged(damage);
            gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }
}
