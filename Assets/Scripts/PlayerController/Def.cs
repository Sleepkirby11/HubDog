using UnityEngine;

public class Def : MonoBehaviour
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;

    Vector2 mouse;
    Transform target;
    float range = 2f;
    float distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //초기화
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        target = GameManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        ShieldTransform();

        //플레이어로부터 반지름 range인 원형 범위 설정 로직
        if (distance > range)
            rigid.transform.position = target.position + (transform.up * range);
        else
            rigid.transform.position = mouse;
    }

    
    void ShieldTransform()
    {
        //마우스 위치에 따라 방패의 회전과 위치를 조정하는 함수
        mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.rotation = Quaternion.FromToRotation(Vector3.up, (mouse - (Vector2)target.position));

        distance = Vector2.Distance(target.position, mouse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger == true
            && collision.gameObject.CompareTag("EnemyBullet")
            && GameManager.instance.player.GetFloat("AlwaysParryTime") > 0)
        {
            GameObject bullet = GameManager.instance.pool.Get(3);
            bullet.transform.position = transform.position;
            bullet.transform.eulerAngles = collision.gameObject.GetComponent<EnemyBullet>().ReverseRotate();
            collision.gameObject.SetActive(false);
        }

    }
}
