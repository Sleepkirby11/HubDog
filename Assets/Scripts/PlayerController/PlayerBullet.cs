using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    Rigidbody2D rigid;
    public int damage;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnEnable()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-15, 15));
    }
    // Update is called once per frame
    void Update()
    {
        rigid.transform.position += transform.up * Time.deltaTime * 10;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().hp -= damage;
            gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
        }
    }
}
