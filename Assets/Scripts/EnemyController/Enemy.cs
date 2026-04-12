using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D col;
    LineRenderer line;

    public int type;

    //타겟 설정
    Transform target;

    float nextMove;

    private float distance;

    public int hp, maxHp;

    public float maxDelay;
    private float currentDelay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        line = GetComponent<LineRenderer>();

        distance = 0;
        currentDelay = 0;
        nextMove = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        line.SetPosition(0, this.transform.position);

        line.SetPosition(1, target.transform.position);
        
        currentDelay += Time.fixedDeltaTime; 

        if(currentDelay >= maxDelay && distance >= 4 && distance < 8)
        {
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

        DistanceMove();
    }

    //생성 시 기본값 부여
    private void OnEnable()
    {
        target = GameManager.instance.player.transform;

        hp = maxHp;
    }

    //플레이어 간의 x좌표 거리 기반 이동
    void DistanceMove()
    {
        //거리 계산
        distance = Mathf.Abs(target.transform.position.x - this.transform.position.x);
        rigid.linearVelocityX = nextMove;

        if (distance >= 4 && distance < 8)
        {
            //이동 방향 설정
            if (target.transform.position.x - rigid.position.x < 0)
            {
                nextMove = -5f;
            }
            else if(target.transform.position.x - rigid.position.x > 0)
            {
                nextMove = 5f;
            }
        }
        else if (distance <= 0.1f)
        {
            nextMove = 0;
        }
    }

    //Shooting
    void Shoot_0()
    {
        Vector2 dir = target.position - transform.position;
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        currentDelay = 0;
        GameObject bullet = GameManager.instance.pool.Get(1);
        bullet.GetComponent<EnemyBullet>().target = target;
        bullet.transform.position = line.GetPosition(0) + new Vector3(0, -0.2f, 0);
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, line.GetPosition(1) - transform.position);
    }

    void Shoot_1()
    {
        Vector2 dir = target.position - transform.position;
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        currentDelay = 0;
        GameObject bullet = GameManager.instance.pool.Get(2);
        bullet.transform.position = line.GetPosition(0);
        bullet.GetComponent<EnemyBullet>().target = target;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up, 
            (line.GetPosition(1) - transform.position));
        bullet.transform.eulerAngles += new Vector3(0, 0, Random.Range(-15, 15));
    }

    //Die
    void Die()
    {
        hp = 0;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("PlayerBullet"))
        {
            collision.gameObject.SetActive(false);
            hp--;
        }
        if(collision.CompareTag("ParryBullet"))
        {
            collision.gameObject.SetActive(false);
            hp -= collision.gameObject.GetComponent<ParryBullet>().damage;
        }
        if (hp <= 0)
        {
            Die();
        }
    }
}
