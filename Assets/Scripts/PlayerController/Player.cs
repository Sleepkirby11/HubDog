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
    public GameObject particle_Getitem;

    private float fireDelay = 0.5f;
    private float currentDelay;

    private float atkSpeedTime;
    private float alwaysParryTime;


    private float speed = 5;
    private int hp;
    private int maxHp = 5;
    public int score;

    public float FireDelay => fireDelay;
    public float CurrentDelay => currentDelay;
    public float ATKSpeedTime => atkSpeedTime;
    public float AlwaysParryTime => alwaysParryTime;
    public int Hp => hp;
    public int MaxHp => maxHp;

    public bool isParrying;
    public bool isShooting;
    public bool isRestrict;
    public bool isCleared;

    private bool isGround;
    private bool isParryJump;
    private bool isCanMove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //초기화
        this.gameObject.SetActive(true);

        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider2D>();
        col.enabled = true;
        rigid.gravityScale = 2;

        currentDelay = 0;
        atkSpeedTime = 0;
        alwaysParryTime = 0;
        isParrying = false;
        isGround = true;
        isParryJump = false;
        isCanMove = true;
        isCleared = false;
        isRestrict = false;

        hp = maxHp;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(hp <= 0)
        {
            return;
        }

        //Player의 이동 로직
        Vector2 nextVec = inputVec * speed;
        if(!isRestrict)
        {
            if (isCanMove)
                rigid.linearVelocityX = nextVec.x;
        }

        //발사 딜레이 계산
        if (currentDelay < FireDelay)
                currentDelay += Time.fixedDeltaTime;

        //아이템에 의해 변한 값 n(초) 카운트다운
        if(atkSpeedTime > 0)
        {
            atkSpeedTime -= Time.fixedDeltaTime;
        }
        else
        {
            atkSpeedTime = 0;
            fireDelay = 0.5f;
        }
        if(alwaysParryTime > 0)
        {
            alwaysParryTime -= Time.fixedDeltaTime;
        }
        else
        {
            alwaysParryTime = 0;
        }


        //패링과 발사 과정을 따로 구분하기 위해 현재 애니메이션 상태 감지 + 발사 텀 체크
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("shoot") && currentDelay >=(FireDelay / 2))
        {
            Shooting();
        }

        GroundCheck();
    }

    //Player Input 시스템을 활용한 플레이어의 움직임 구현
    public void ActionMove(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            isCanMove = true;
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
        if (hp <= 0)
        {
            return;
        }

        if (context.started && isGround && !isRestrict) //isGround = (+)2단 점프 방지
        {
            rigid.linearVelocityY = 0;
            rigid.AddForceY(10, ForceMode2D.Impulse);
            isGround = false;
        }
        if(context.canceled)
        {
            if (rigid.linearVelocityY > 0)
                rigid.linearVelocityY = 0;
        }
    }

    //Player Input을 활용하여 메뉴 활성화 구현
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
        if (currentDelay >= fireDelay)
        {
            //패링 판정 오브젝트 소환
            currentDelay = 0;
            isParrying = true;
            GameObject Ting = GameManager.instance.pool.Get(6);
            Ting.transform.position = Shield.transform.position;
            Ting.transform.eulerAngles = Shield.transform.eulerAngles;

            //방패의 방향에 따른 이단 점프 기능
            Vector2 distance = Ting.transform.position - this.transform.position;
            if(!isGround)
            {
                if(isParryJump)
                {
                    rigid.linearVelocityY = 0;
                    rigid.AddForce(-distance * 5, ForceMode2D.Impulse);
                    isParryJump = false;
                    isCanMove = false;
                }
            }
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

    //총알 발사 함수
    void Shooting()
    {
        // Shoot
        currentDelay = 0;
        GameObject bullet = GameManager.instance.pool.Get(0);
        bullet.transform.position = Shield.gameObject.transform.position;
        bullet.transform.eulerAngles = Shield.gameObject.transform.eulerAngles;
    }

    //피격 함수(hp 감소 및 사망 판정)
    public void Damaged(int damage)
    {
        if(!isCleared)
            hp -= damage;
        GameManager.instance.UpdateLifeBar();
        if (hp <= 0)
        {
            anim.SetTrigger("isDie");
            rigid.linearVelocity = Vector2.zero;
            GameManager.instance.StartCoroutine(GameManager.instance.DieBoth());
            
            GameObject deathHit = GameManager.instance.pool.Get(10);
            deathHit.transform.position = this.transform.position;
            deathHit.transform.localScale = deathHit.transform.localScale * 3;
            GameObject superDeath = GameManager.instance.pool.Get(11);
            superDeath.transform.position = this.transform.position;
        }
    }

    void Died()
    {
        gameObject.SetActive(false);
    }

    //Heal 함수(Hp가 MaxHp를 넘기지 않도록 보정
    public void Heal(int heal)
    {
        ItemParticleRenderer(0);
        hp += heal;
        if(hp > maxHp)
            hp = maxHp;
        GameManager.instance.UpdateLifeBar();
    }

    //공격 속도 UP함수
    public void ATKUp(int speed)
    {
        ItemParticleRenderer(1);
        fireDelay /= 2f;
        atkSpeedTime = speed;
    }
    public void AlwayDef(int second)
    {
        ItemParticleRenderer(2);
        alwaysParryTime = second;
    }

    void ItemParticleRenderer(int type)
    {
        particle_Getitem.gameObject.SetActive(true);
        ParticleSystem.TextureSheetAnimationModule tsa = particle_Getitem.GetComponent<ParticleSystem>().textureSheetAnimation;
        float nomalizedType = type / (float)tsa.spriteCount;

        tsa.enabled = true;
        tsa.startFrame = (new ParticleSystem.MinMaxCurve(nomalizedType));
        tsa.frameOverTime = new ParticleSystem.MinMaxCurve(0);
    }

    //플레이어 착지 판정 검사
    void GroundCheck()
    {
        if (rigid.linearVelocityY < 0 || isRestrict)
            isGround = Physics2D.CapsuleCast
                (col.bounds.center, col.bounds.size, CapsuleDirection2D.Vertical, 0f, Vector2.down, 0.2f, LayerMask.GetMask("Ground"));

        //isGround가 true일 때 isCanMovew와 isParryJump의 상태 동일화
        if (isGround && !isParryJump)
        {
            isParryJump = true;
            isCanMove = true;
        }
    }

    public int GetInt(string name)
    {
        if (name == "hp")
            return hp;
        if(name == "maxHp")
        {
            return maxHp;
        }

        return 0;
    }

    //점프 후 착지 판정 보완
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f) //접촉 지점의 노멀 벡터가 위쪽을 향할 때만 착지 판정
                {
                    isGround = true;
                    break;
                }
            }
        }
    }

}
