using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] float spawnInterval = 2f;

    private float spawnTimer;

    void Update()
    {
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPosition = GetRandomSpawnPosition();
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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