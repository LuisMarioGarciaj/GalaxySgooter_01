using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public static Spawner instance;

    public float timeBtwSpawn = 1.5f;
    public int wave = 1;
    public int maxEnemiesPerWave = 5;
    public float timeBetweenWaves = 5f;
    public int bossWave = 5; // Onda en la que aparece el jefe
    public GameObject bossPrefab;
    public bool bossSpawned = false;
    private bool spawningWave = true;

    float timer = 0;
    int enemiesSpawned = 0;

    public Transform leftPoint;
    public Transform rightPoint;
    public List<GameObject> enemyPrefabs;
    public int score = 0;
    public Text scoreText;
    public Text waveText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        scoreText.text = "SCORE: " + score;
        waveText.text = "WAVE: " + wave;
    }

    void Update()
    {
        if (!bossSpawned)
        {
            if (spawningWave)
            {
                SpawnEnemiesInWave();
            }
            else
            {
                CheckWaveCompletion();
            }
        }
    }

    // Control del spawn de enemigos por oleada
    void SpawnEnemiesInWave()
    {
        if (timer < timeBtwSpawn)
        {
            timer += Time.deltaTime;
        }
        else if (enemiesSpawned < maxEnemiesPerWave)
        {
            timer = 0;
            float x = Random.Range(leftPoint.position.x, rightPoint.position.x);
            int enemyIndex = Random.Range(0, enemyPrefabs.Count);
            Vector3 newPos = new Vector3(x, transform.position.y, 0);
            Instantiate(enemyPrefabs[enemyIndex], newPos, Quaternion.Euler(0, 0, 180));
            enemiesSpawned++;
        }
        else
        {
            spawningWave = false;
        }
    }

    // Verificar si la oleada ha sido completada y generar el jefe si es necesario
    void CheckWaveCompletion()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !bossSpawned)
        {
            if (wave == bossWave)
            {
                SpawnBoss();
            }
            else
            {
                StartCoroutine(WaitForNextWave());
            }
        }
    }

    // Spawnear al jefe enemigo
    void SpawnBoss()
    {
        bossSpawned = true;
        float x = (leftPoint.position.x + rightPoint.position.x) / 2; // Posicionar al jefe en el centro
        Vector3 newPos = new Vector3(x, transform.position.y, 0);
        Instantiate(bossPrefab, newPos, Quaternion.identity);
    }

    IEnumerator WaitForNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        wave++;
        waveText.text = "WAVE: " + wave;
        enemiesSpawned = 0;
        spawningWave = true;
    }

    public void AddScore(int points)
    {
        score += points;
        scoreText.text = "SCORE: " + score;
    }
}
