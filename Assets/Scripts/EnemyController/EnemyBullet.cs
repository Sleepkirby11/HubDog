using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody2D rigid;
    Transform target;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        target = GameManager.instance.player.transform;
    }
    // Update is called once per frame
    void Update()
    {
        rigid.transform.position += transform.up * Time.deltaTime * 5;

        if (transform.position.y < -5)
        {
            gameObject.SetActive(false);
        }
    }

    public Vector3 ReverseRotate()
    {
        return new Vector3(0, 0, transform.eulerAngles.z + 180);
    }
}
