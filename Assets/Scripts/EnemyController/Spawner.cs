using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int spawnID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameObject enemy = GameManager.instance.pool.Get(spawnID);
        enemy.transform.position = this.transform.position;
    }

}

