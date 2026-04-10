using UnityEngine;

public class Def : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    Vector2 mouse;
    Transform target;
    float range = 2f;
    float distance;

    float angle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        angle = 0;
        target = GameManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.x - target.position.x, mouse.y - target.position.y) * Mathf.Rad2Deg;

        this.transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);

        distance = Vector2.Distance(target.position, mouse);

        if (distance > range)
            rigid.transform.position = target.position + (transform.up * range);
        else
            rigid.transform.position = mouse;
    }

    
}
