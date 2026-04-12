using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody2D rigid;
    public Transform target;
    float distance;


    public int type;
    bool isGround;
    float speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        distance = 0;
    }

    private void OnEnable()
    {
        isGround = false;
        speed = 10;
        rigid.gravityScale = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isGround)
            rigid.linearVelocity = transform.up * speed;

        distance = Vector2.Distance(target.position, this.transform.position);
        switch(type)
        {
            case 1:
                if(isGround)
                    shoot_1();
                break;
        }
        ActiveFalse();
    }

    public Vector3 ReverseRotate()
    {
        return new Vector3(0, 0, transform.eulerAngles.z + 180);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag =="Ground")
        {
            if (type == 0)
                gameObject.SetActive(false);
            else if (type == 1)
            {
                if(isGround)
                    gameObject.SetActive(false);
                else
                    isGround = true;
                rigid.gravityScale = 5;
                //이동 방향 설정
                if (target.transform.position.x - rigid.position.x < 0)
                {
                    speed = -10;
                }
                else if (target.transform.position.x - rigid.position.x > 0)
                {
                    speed = 10;
                }
            }
        }
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().hp -= 1;
            GameManager.instance.UpdateLifeBar();
            gameObject.SetActive(false);
        }
    }
    void ActiveFalse()
    {
        if (distance > 15)
        {
            gameObject.SetActive(false);
        }
    }
    void shoot_1()
    {
        rigid.AddForce(Vector2.right * speed, ForceMode2D.Force);
    }
}
