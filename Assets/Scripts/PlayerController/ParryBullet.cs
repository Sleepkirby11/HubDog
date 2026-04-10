using UnityEngine;

public class ParryBullet : MonoBehaviour
{
    Rigidbody2D rigid;
    public int damage = 5;


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


}
