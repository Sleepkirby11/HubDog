using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D col;

    public int type;

    //타겟 설정
    Transform target;

    float nextMove;

    private float distanceX;
    private float distanceY;

    public int hp, maxHp;
    public float speed;

    public float maxDelay;
    private float currentDelay;

    public int score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

        distanceX = 0;
        distanceY = 0;
        nextMove = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentDelay < maxDelay)
            currentDelay += Time.fixedDeltaTime;

        //플레이어 인식, 발사 딜레이 설정 조건식
        if(currentDelay >= maxDelay
            && distanceX < 8
            && distanceY < 8
            && target.gameObject.transform.position.y < this.transform.position.y
            && type != 2)
        {
            //Bullet Type 기반 전용 함수 호출
            switch(type)
            {
                case 0:
                    Shoot_0();
                    break;
                case 1:
                    Shoot_1();
                    break;
            }
        }
        if(type == 2)
        {
            FollowMove();
        }
        else
            DistanceMove();
    }

    //생성 시 기본값 부여
    private void OnEnable()
    {
        target = GameManager.instance.player.transform;
        currentDelay = 0;
        hp = maxHp;
    }

    //플레이어 간의 x좌표 거리 기반 이동
    void DistanceMove()
    {
        //거리 계산
        distanceX = Mathf.Abs(target.transform.position.x - this.transform.position.x);
        distanceY = Mathf.Abs(target.transform.position.y - this.transform.position.y);
        rigid.linearVelocityX = nextMove;

        //플레이어 인식 범위 설정
        if (distanceX >= 4 && distanceX < 8 && distanceY < 8)
        {
            //이동 방향 설정
            if (target.transform.position.x - rigid.position.x < 0)
            {
                nextMove = -speed;
            }
            else if(target.transform.position.x - rigid.position.x > 0)
            {
                nextMove = speed;
            }
        }
        //멈추는 범위 조건식 설정
        else if (distanceX <= 0.1f)
        {
            nextMove = 0;
        }
    }

    void FollowMove()
    {
        Vector2 dir = target.position - transform.position;
        rigid.AddForce(dir * speed, ForceMode2D.Force);

        if(currentDelay >= maxDelay)
        {
            Enemy_2_Die();
        }
    }

    //일반 Bullet 전용 함수
    void Shoot_0()
    {
        currentDelay = 0;
        GameObject bullet = GameManager.instance.pool.Get(1);
        bullet.GetComponent<EnemyBullet>().target = target;

        bullet.transform.position = this.transform.position + new Vector3(0, -0.2f, 0);
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, target.position - transform.position);
    }

    //Chainsaw Bullet 전용 함수
    void Shoot_1()
    {
        currentDelay = 0;
        GameObject bullet = GameManager.instance.pool.Get(2);
        bullet.GetComponent<EnemyBullet>().target = target;

        bullet.transform.position = this.transform.position + new Vector3(0, -0.2f, 0);
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, 
            (target.position - transform.position));
        bullet.transform.eulerAngles += new Vector3(0, 0, Random.Range(-20, 20));
    }

    //대미지 계산 함수
    public void Damaged(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }

    //Die
    void Die()
    {
        hp = 0;
        GameManager.instance.UpdateScore(score);

        GameObject particle = GameManager.instance.pool.Get(7);
        particle.gameObject.transform.position = this.transform.position;
        gameObject.SetActive(false);
    }

    void Enemy_2_Die()
    {
        GameManager.instance.HitEffectSpawn(this.transform);
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == 2)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<Player>().Damaged(1);
                Enemy_2_Die();
            }
        }
    }
}
