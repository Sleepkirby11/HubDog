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
    BoxCollider2D col;


    public float FireDelay;
    private float currentDelay;
    public float speed;
    public int hp;
    public int maxHp;

    bool isParrying;
    bool isGround;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        currentDelay = 0;
        isParrying = false;
        isGround = true;
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 nextVec = inputVec * speed;
        rigid.linearVelocityX = nextVec.x;

        if(currentDelay < FireDelay)
            currentDelay += Time.deltaTime;

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("shoot") && currentDelay >= FireDelay)
        {
            // Shoot
            currentDelay = 0;
            GameObject bullet = GameManager.instance.pool.Get(0);
            bullet.transform.position = transform.position;
        }

        if(hp <= 0)
        {
            hp = 0;
        }
    }

    public void ActionMove(InputAction.CallbackContext context)
    {
        if(context.started)
        {

            anim.SetBool("isWalk", true);
            inputVec = context.ReadValue<Vector2>();
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

    public void ActionJump(InputAction.CallbackContext context)
    {
        if (context.started && isGround)
        {
            rigid.AddForceY(5, ForceMode2D.Impulse);
            isGround = false;
        }
    }
    
    void OnParry()
    {
        isParrying = true;
    }
    void OffParry()
    {
        isParrying = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }
}
