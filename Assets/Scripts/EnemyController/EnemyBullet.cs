using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody2D rigid;
    public Transform target;
    float distance;


    public int type;
    public int damage;
    bool isGround;
    float speed;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        distance = 0;
    }

    private void OnEnable()
    {
        //기본값 초기화
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

    //패링 시 eularAngle의 반전값 리턴
    public Vector3 ReverseRotate()
    {
        return new Vector3(0, 0, transform.eulerAngles.z + 180);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag =="Ground")
        {
            if (type == 1)
            {
                if(isGround)
                    this.gameObject.SetActive(false);
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
    }

    //Trigger 기반 충돌 감지
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") && type == 0)
            this.gameObject.SetActive(false);
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().Damaged(damage);
            this.gameObject.SetActive(false);
        }
    }

    //distance 기반 비활성화 함수
    void ActiveFalse()
    {
        if (distance > 15)
        {
            this.gameObject.SetActive(false);
        }
    }

    //isGround이 true일 때 Chainsaw 전용 함수
    void shoot_1()
    {
        rigid.AddForce(Vector2.right * speed, ForceMode2D.Force);
    }
}
