using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    Rigidbody2D rigid;
    private Vector2 inputVec;
    SpriteRenderer sprite;
    Animator anim;
    CapsuleCollider2D col;

    public GameObject Shield;


    public float FireDelay;
    public float currentDelay;


    public float speed;
    public int hp;
    public int maxHp;

    public bool isParrying;
    private bool isGround;
    public bool isShooting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //초기화
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();

        currentDelay = 0;
        isParrying = false;
        isGround = true;
        hp = maxHp;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 nextVec = inputVec * speed;
        rigid.linearVelocityX = nextVec.x;

            if (currentDelay < FireDelay)
                currentDelay += Time.fixedDeltaTime;
        //패링과 발사 과정을 따로 구분하기 위해 현재 애니메이션 상태 감지 + 발사 텀 체크
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("shoot") && currentDelay >=(FireDelay / 2))
        {
            Shooting();
        }


        //체력을 모두 소진했을 때 사망
        if (hp <= 0)
        {
            isParrying = false;
            Die();
        }
        if(rigid.linearVelocityY < 0)
            isGround = Physics2D.BoxCast(col.bounds.center, col.bounds.size * 0.8f, 0, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));
    }

    //Player Input 시스템을 활용한 플레이어의 움직임 구현
    public void ActionMove(InputAction.CallbackContext context)
    {
        if(context.started)
        {

            anim.SetBool("isWalk", true);
            inputVec = context.ReadValue<Vector2>();

            //스프라이트 방향 전환
            if (inputVec.x > 0)
                sprite.flipX = true;
            else if (inputVec.x < 0)
                sprite.flipX = false;
        }
        if (context.canceled)
        {
            anim.SetBool("isWalk", false);
            inputVec = Vector2.zero;
        }
    }

    //Player Input과 Animation을 활용한 발사 시스템 구현
    public void ActionShooting(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            anim.SetBool("isShooting", true);
        }
        if(context.canceled)
        {
            anim.SetBool("isShooting", false);
        }
    }

    //Player Input을 활용하여 점프 구현
    public void ActionJump(InputAction.CallbackContext context)
    {
        if (context.started && isGround) //isGround = (+)2단 점프 방지
        {
            rigid.AddForceY(10, ForceMode2D.Impulse);
            isGround = false;
        }
        if(context.canceled && rigid.linearVelocityY > 0)
        {
            rigid.linearVelocityY = 0;
        }
    }

    public void ActionMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameManager.instance.ActiveMenu();
        }
    }
    
    //OnParry, OffParry는 각각 Animator에 의해 호출
    void OnParry()
    {
        if (currentDelay >= FireDelay)
        {
            currentDelay = 0;
            isParrying = true;
            GameObject Ting = GameManager.instance.pool.Get(6);
            Ting.transform.position = Shield.transform.position;
            Ting.transform.eulerAngles = Shield.transform.eulerAngles;
        }
    }
    void OffParry()
    {
        isParrying = false;
        if(anim.GetBool("isShooting") == true)
            isShooting = true;
        else
            isShooting = false;
    }

    void Shooting()
    {
        // Shoot
        currentDelay = 0;
        GameObject bullet = GameManager.instance.pool.Get(0);
        bullet.transform.position = Shield.gameObject.transform.position;
        bullet.transform.eulerAngles = Shield.gameObject.transform.eulerAngles;
    }

    void Die()
    {
        hp = 0;
    }
    
    ////점프 후 착지 판정
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
        
    //    if(collision.gameObject.CompareTag("Ground"))
    //    {
    //        foreach(ContactPoint2D contact in collision.contacts)
    //        {
    //            if(contact.normal.y > 0.5f) //접촉 지점의 노멀 벡터가 위쪽을 향할 때만 착지 판정
    //            {
    //                isGround = true;
    //                break;
    //            }
    //        }
    //    }
    //}
    ////플랫폼에서 떨어졌을 때 한번 더 체크
    //private void OnCollisionExit2D(Collision2D collision)
    //{

    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        foreach (ContactPoint2D contact in collision.contacts)
    //        {
    //            if (contact.normal.y > 0.5f) //접촉 지점의 노멀 벡터가 위쪽을 향할 때만
    //            {
    //                isGround = false;
    //                break;
    //            }
    //        }
    //    }
    //}

    //플레이어의 피격 판정(패링 or hp감소)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("EnemyBullet"))
        {
            collision.gameObject.SetActive(false);
            hp--;
            GameManager.instance.UpdateLifeBar();
        }
    }
}
