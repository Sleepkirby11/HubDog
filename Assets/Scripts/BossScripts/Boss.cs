using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    Rigidbody2D rigid;
    BoxCollider2D col;
    SpriteRenderer sprite;

    Animator anim;

    int phase;
    public int type;

    //타겟 설정
    public Transform target;

    public Transform[] firePoints;

    public int hp, maxHp;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        col.enabled = true;

        hp = maxHp;
        phase = 1;

    }

    void UpdatePattern()
    {
        //랜덤 타입 지정, 호출
        type = Random.Range(0, 2);

        switch (type)
        {
            case 0:
                anim.SetTrigger("Pattern_0");
                if (phase == 2)
                {
                    for (int i = 1; i <= 3; i += 2)
                    {
                        GameObject enemy_3 = GameManager.instance.pool.Get(12);

                        Vector3 firePoint = firePoints[i].position;
                        enemy_3.transform.position = firePoint;
                    }
                }
                break;
            case 1:
                anim.SetTrigger("Pattern_1");
                break;
        }
    }

    //패턴_1
    void Shoot_0()
    {
        GameObject bullet = GameManager.instance.pool.Get(2);
        bullet.GetComponent<EnemyBullet>().target = target;

        bullet.transform.position = this.transform.position;
        bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up,
            (target.position - transform.position));
        bullet.transform.eulerAngles += new Vector3(0, 0, Random.Range(-20, 20));
    }

    void MovePos()
    {
        rigid.MovePosition(new Vector2(Random.Range(-6f, 6f), this.transform.position.y));
    }
    void MoveCenter()
    {
        rigid.MovePosition(new Vector2(0, this.transform.position.y));
    }

    void Pattern_1()
    {
        if (GameManager.instance.player.Hp > 0)
        {
            if (GameManager.instance.player.isRestrict)
            {
                GameManager.instance.player.isRestrict = false;
                StartCoroutine(pat_1_finish(GameManager.instance.player.score));
            }
            else
            {
                GameManager.instance.player.isRestrict = true;
                GameManager.instance.player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                GameManager.instance.player.transform.position = Vector2.zero;

                if (phase != 2)
                    StartCoroutine(pat_1());
                else
                    StartCoroutine(pat_1_phase2());
            }
        }
    }



    //패턴_2
    IEnumerator pat_1()
    {
        GameObject item = GameManager.instance.pool.Get(9);
        item.GetComponent<Items>().type = 2;

        item.transform.position = target.position;
        for (int i = 0; i < 9; i++)
        {

            yield return new WaitForSeconds(0.35f);
            GameObject bullet = GameManager.instance.pool.Get(8);
            bullet.GetComponent<EnemyBullet>().target = target;

            Vector3 firePoint = firePoints[Random.Range(0, firePoints.Length)].position;
            bullet.transform.position = firePoint;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up,
            (target.position - firePoint));
        }
    }
    IEnumerator pat_1_phase2()
    {
        GameObject item = GameManager.instance.pool.Get(9);
        item.GetComponent<Items>().type = 2;

        item.transform.position = target.position;
        for (int i = 0; i < 14; i++)
        {

            yield return new WaitForSeconds(0.25f);
            GameObject bullet = GameManager.instance.pool.Get(8);
            bullet.GetComponent<EnemyBullet>().target = target;

            Vector3 firePoint = firePoints[Random.Range(0, firePoints.Length)].position;
            bullet.transform.position = firePoint;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up,
            (target.position - firePoint));
        }
    }



    public IEnumerator pat_1_finish(int score)
    {
        for (int i = 0; i < score; i++)
        {
            yield return new WaitForSeconds(0.1f);
            GameObject bullet = GameManager.instance.pool.Get(1);
            bullet.GetComponent<EnemyBullet>().target = target;
            bullet.transform.rotation = Quaternion.FromToRotation(Vector3.up,
            (target.position - transform.position));
            bullet.transform.position = firePoints[2].position;
        }
        GameManager.instance.player.score = 0;
    }

    //패턴_3
    void Shoot_2()
    {

    }

    //대미지 계산 함수
    public void Damaged(int damage)
    {
        hp -= damage;
        if (hp <= maxHp / 2)
        {
            phase = 2;
        }

        if (hp <= 0)
        {
            Die();
        }
    }

    //Die
    void Die()
    {
        hp = 0;
        anim.SetTrigger("isDie");
        GameManager.instance.player.isCleared = true;
        GameManager.instance.StartCoroutine(GameManager.instance.DieBoth());

        GameObject deathHit = GameManager.instance.pool.Get(10);
        deathHit.transform.position = this.transform.position;
        deathHit.transform.localScale = deathHit.transform.localScale * 3;
        GameObject superDeath = GameManager.instance.pool.Get(11);
        superDeath.transform.position = this.transform.position;
    }

    void ActiveFalse()
    {
        gameObject.SetActive(false);
    }
}
