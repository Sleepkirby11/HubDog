using UnityEngine;

public class Def : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    Vector2 mouse;
    Transform target;
    float range = 2f;

    float angle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        angle = 0;
        target = GameManager.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        angle = Mathf.Atan2(mouse.x - target.position.x, mouse.y - target.position.y) * Mathf.Rad2Deg;

        this.transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
    }

    
}
