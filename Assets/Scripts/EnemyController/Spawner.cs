using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int spawnID;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //spawnID: Pool에서 가져올 Enemy의 Type
        //4: Enemy Type = 1
        //5: Enemy Type = 2
        GameObject enemy = GameManager.instance.pool.Get(spawnID);
        enemy.transform.position = this.transform.position;
    }

}

