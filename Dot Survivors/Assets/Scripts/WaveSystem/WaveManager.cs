using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public WaveConfig[] waves;

    private int currentWaveIndex = 0;
    private float waveTimer = 0f;
    private float spawnTimer = 0f;

    void Start()
    {
        if (waves.Length > 0)
        {
            StartCoroutine(ManageWaves());
        }
        else
        {
            Debug.LogError("No waves configured in WaveManager.");
        }
    }

    IEnumerator ManageWaves()
    {
        while (currentWaveIndex < waves.Length)
        {
            WaveConfig currentWave = waves[currentWaveIndex];

            waveTimer = 0f;
            spawnTimer = 0f;

            while (waveTimer < currentWave.waveDuration)
            {
                waveTimer += Time.deltaTime;
                spawnTimer += Time.deltaTime;

                if (spawnTimer >= currentWave.spawnRate)
                {
                    SpawnEnemy(currentWave);
                    spawnTimer = 0f;
                }

                yield return null;
            }

            currentWaveIndex++;
        }

        Debug.Log("All waves completed.");
    }

void SpawnEnemy(WaveConfig wave)
{
    Vector2 spawnPosition = GetRandomSpawnPosition();
    float totalChance = 0f;

    foreach (var enemy in wave.enemies)
    {
        totalChance += enemy.spawnChance;
    }

    float randomValue = Random.Range(0f, totalChance);
    float cumulativeChance = 0f;

    foreach (var enemy in wave.enemies)
    {
        cumulativeChance += enemy.spawnChance;
        if (randomValue <= cumulativeChance)
        {
            GameObject spawnedEnemy = Instantiate(enemy.enemyPrefab, spawnPosition, Quaternion.identity);

            // Set random scale
            float randomSize = Random.Range(0.7f, 1.3f); // Adjust values as needed
            spawnedEnemy.transform.localScale = new Vector3(randomSize, randomSize, 1);

            break;
        }
    }
}

    Vector2 GetRandomSpawnPosition()
    {
        Camera mainCamera = Camera.main;
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = mainCamera.orthographicSize * 2;
        float cameraWidth = cameraHeight * screenAspect;

        float x = 0;
        float y = 0;

        if (Random.value > 0.5f)
        {
            // Spawn on left or right side
            x = Random.value > 0.5f ? mainCamera.transform.position.x + cameraWidth / 2 + 1 : mainCamera.transform.position.x - cameraWidth / 2 - 1;
            y = Random.Range(mainCamera.transform.position.y - cameraHeight / 2 - 1, mainCamera.transform.position.y + cameraHeight / 2 + 1);
        }
        else
        {
            // Spawn on top or bottom side
            y = Random.value > 0.5f ? mainCamera.transform.position.y + cameraHeight / 2 + 1 : mainCamera.transform.position.y - cameraHeight / 2 - 1;
            x = Random.Range(mainCamera.transform.position.x - cameraWidth / 2 - 1, mainCamera.transform.position.x + cameraWidth / 2 + 1);
        }

        return new Vector2(x, y);
    }
}