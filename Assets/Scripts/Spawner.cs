using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject enemy = GameManager.instance.pool.Get(3);
            enemy.transform.position = new Vector3(Random.Range(-5, 5), 8, 0);
        }
    }
}
