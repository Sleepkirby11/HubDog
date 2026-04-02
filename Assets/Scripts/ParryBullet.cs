using UnityEngine;

public class ParryBullet : MonoBehaviour
{
    Rigidbody2D rigid;


    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        rigid.transform.position += transform.up * Time.deltaTime * 20;

        if (transform.position.y > 10)
        {
            gameObject.SetActive(false);
        }
    }


}
