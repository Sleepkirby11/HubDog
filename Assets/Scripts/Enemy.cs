using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D col;

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
        distance = 0;
        currentDelay = 0;
        nextMove = 0;
    }

    // Update is called once per frame
    void Update()
    {
        currentDelay += Time.deltaTime;

        if(currentDelay >= maxDelay)
        {
            Shoot();
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

        if (distance >= 4)
        {
            //이동 방향 설정
            if (target.transform.position.x - rigid.position.x < 0)
            {
                nextMove = -10f;
            }
            else if(target.transform.position.x - rigid.position.x > 0)
            {
                nextMove = 10f;
            }
        }
        else if (distance <= 0.1f)
        {
            nextMove = 0;
        }
    }

    //Shooting
    void Shoot()
    {
        Vector2 dir = target.position - transform.position;
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        currentDelay = 0;
        GameObject bullet = GameManager.instance.pool.Get(1);
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.Euler(0, 0, rot - 90);
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

            if (hp <= 0)
            {
                Die();
            }
        }
    }
}
