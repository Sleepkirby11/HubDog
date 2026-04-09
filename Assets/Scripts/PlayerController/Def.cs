using UnityEngine;

public class Def : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    Vector2 mouse;
    Transform target;
    float range = 2f;

    float angle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();

        angle = 0;
        target = GameManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        angle = Mathf.Atan2(mouse.x - target.position.x, mouse.y - target.position.y) * Mathf.Rad2Deg;

        rigid.transform.position = target.position + (transform.up * range);

        this.transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
    }

    
    void Glow()
    {
        if (GameManager.instance.player.isParrying)
            spriteRenderer.color = Color.white;
    }
}
