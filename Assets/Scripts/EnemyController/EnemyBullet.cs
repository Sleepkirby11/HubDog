using UnityEditor;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody2D rigid;
    public Transform target;
    Vector2 velocity;
    float distance;


    public int type;
    public int damage;
    bool isGround;
    float speed;

    float current;
    float max;

    private void OnEnable()
    {
        //기본값 초기화
        speed = 10;
        distance = 0;
        current = 0;
        max = 3;
        velocity = Vector2.zero;

        rigid = GetComponent<Rigidbody2D>();
        isGround = false;
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
        {
            GameManager.instance.HitEffectSpawn(this.transform);
            this.gameObject.SetActive(false);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.HitEffectSpawn(this.transform);

            collision.gameObject.GetComponent<Player>().Damaged(damage);
            this.gameObject.SetActive(false);
        }

        //패링 판정 검사
        //Ting 혹은 Shield 태그와 플레이어의 아이템 여부 검사
        if (collision.gameObject.CompareTag("Ting") && GameManager.instance.player.isParrying
            || collision.gameObject.CompareTag("Shield")
            && GameManager.instance.player.AlwaysParryTime > 0)
        {
            //플레이어 사망 시 안전장치
            if (GameManager.instance.player.Hp <= 0)
                return;
            //상시 패링
            if(GameManager.instance.player.AlwaysParryTime > 0)
            {
                GameObject tingEffect = GameManager.instance.pool.Get(6);
                tingEffect.transform.position = collision.gameObject.transform.position;
                tingEffect.transform.eulerAngles = this.gameObject.transform.eulerAngles;
            }
            //일반 패링 성공, EnemyBullet의 type에 따른 분류
            if (type != 2)
            {
                GameObject bullet = GameManager.instance.pool.Get(3);
                bullet.transform.position = transform.position;
                bullet.transform.eulerAngles = ReverseRotate();
            }
            else
                GameManager.instance.player.score++;
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
        if(rigid.linearVelocity == Vector2.zero)
        {
            current += Time.deltaTime;
            if(current > max)
            {
                current = 0;
                this.gameObject.SetActive(false);
            }
        }
    }

    //isGround이 true일 때 Chainsaw 전용 함수
    void shoot_1()
    {
        rigid.AddForce(Vector2.right * speed * 5, ForceMode2D.Force);
    }
}
