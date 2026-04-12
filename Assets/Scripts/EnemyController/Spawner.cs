using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            GameObject enemy = GameManager.instance.pool.Get(4);
            enemy.transform.position = new Vector3(Random.Range(-5, 5), transform.position.y, 0);
        }
    }
}

