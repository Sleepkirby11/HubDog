using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    Rigidbody2D rigid;

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

        if (transform.position.y > 10)
        {
            gameObject.SetActive(false);
        }
    }
}
