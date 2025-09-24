using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject enemyFastPrefab;
    [SerializeField] GameObject boss1Prefab;
    [SerializeField] GameObject boss1Prefab2;
    [SerializeField] GameObject boss2Prefab;
    [SerializeField] GameObject boss2Prefab2;
    [SerializeField] float minSpawnTime = 1.0f;
    [SerializeField] float maxSpawnTime = 3.0f;
    [SerializeField] int wave;
    [SerializeField] float waveTime = 15.0f;

    int bossTimer = 0;
    float spawnDistance = 10f;
    Vector2 screenBounds;
    Vector2 spawnPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnEnemy();
        Waves();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnEnemy()
    {
        float spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        int side = Random.Range(0, 4);
        switch (side)
        {
            case 0:
                spawnPos = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), screenBounds.y + spawnDistance);
                break;
            case 1:
                spawnPos = new Vector2(Random.Range(-screenBounds.x, screenBounds.x), -screenBounds.y - spawnDistance);
                break;
            case 2:
                spawnPos = new Vector2(screenBounds.x + spawnDistance, Random.Range(-screenBounds.y, screenBounds.y));
                break;
            case 3:
                spawnPos = new Vector2(-screenBounds.x - spawnDistance, Random.Range(-screenBounds.y, screenBounds.y));
                break;
        }
        if ((bossTimer != 10 || bossTimer !> 19) && wave < 10)
        {
            Instantiate(enemyPrefab, spawnPos, transform.rotation);
            Invoke("SpawnEnemy", spawnTime);
        }

        int enemyRandomizer = Random.Range(0, 2);
        if (wave > 10)
        {
            switch (enemyRandomizer)
            {
                case 0:
                    Instantiate(enemyFastPrefab, spawnPos, transform.rotation);
                    break;
                case 1:
                    {
                        Instantiate(enemyPrefab, spawnPos, transform.rotation);
                        break;
                    }
            }
            Invoke("SpawnEnemy", spawnTime);
        }

        if (bossTimer == 10)
        {
            int boss1Randomizer = Random.Range(0, 2);
            switch (boss1Randomizer)
            {
                case 0:
                    Instantiate(boss1Prefab, spawnPos, transform.rotation);
                    break;
                case 1:
                    Instantiate(boss1Prefab2, spawnPos, transform.rotation);
                    break;
            }
            
            bossTimer += 1;
            Invoke("SpawnEnemy", 20);
        }

        if (bossTimer > 20)
        {
            int boss2Randomizer = Random.Range(0, 2);
            switch (boss2Randomizer)
            {
                case 0:
                    Instantiate(boss2Prefab, spawnPos, transform.rotation);
                    break;
                case 1:
                    Instantiate(boss2Prefab2, spawnPos, transform.rotation);
                    break;
            }
            Invoke("SpawnEnemy", 20);
            bossTimer = 0;
        }


    }
    void Waves()
    {
        if (minSpawnTime > 1)
        {
            minSpawnTime = minSpawnTime - 1;
        }
        if (maxSpawnTime > 2)
        {
            maxSpawnTime = maxSpawnTime - 1;

        }
        wave += 1;
        bossTimer += 1;
        Debug.Log(bossTimer);
        Invoke("Waves", waveTime);
    }
}

