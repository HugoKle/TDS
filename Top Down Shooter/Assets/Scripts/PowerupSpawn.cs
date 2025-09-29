using System.Threading.Tasks;
using UnityEngine;

public class PowerupSpawn : MonoBehaviour
{
    [SerializeField] GameObject speed;
    [SerializeField] GameObject health;

    [SerializeField] float maxSpawnTime;
    [SerializeField] float minSpawnTime;

    float spawnTime;
    Vector2 screenBounds;
    Vector2 spawnPos;
    GameObject powerup;

    int upgradeCounter = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    async Task Start()
    {
        await Task.Delay(10000);
        SpawnPowerup();


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnPowerup()
    {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        int randomPowerup = Random.Range(0, 3);

        switch (randomPowerup)
        {
            case 0:
                powerup = health;
                break;
            case 1:
                powerup = speed;
                break;
            case 2:
                powerup = speed;
                break;
        }

        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        spawnPos = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), Random.Range(-screenBounds.y, screenBounds.y));

        Instantiate(powerup, spawnPos, transform.rotation);
        Invoke("SpawnPowerup", spawnTime);
    }
}
