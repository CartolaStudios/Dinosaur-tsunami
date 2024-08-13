using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public GameObject[] obstacles; // Carros, lixos, ônibus, tanques, aviões
    public GameObject[] humanPrefabs; // Diferentes tipos de humanos
    public GameObject coinPrefab;
    public GameObject powerUpPrefab; // Power-ups como Fire, BigHead, BigDino
    public BreakableElement[] breakableElements; // Elementos quebráveis com probabilidade configurável
    public float initialSpawnInterval = 2.0f; // Intervalo inicial entre spawns
    private float spawnInterval;
    private float timer;
    private float timerPowerUp;
    private float elapsedTime;
    public float humanSpacing = 1.0f; // Espaçamento entre os humanos

    // Probabilidades iniciais
    public float baseHumanProbability = 0.5f;
    public float baseCoinProbability = 0.3f;
    public float baseObstacleProbability = 0.2f;

    public DinoManager dm;

    private void Start()
    {
        spawnInterval = initialSpawnInterval;
        timerPowerUp = 30.0f; // Define o intervalo de 30 segundos para o PowerUp
    }

    void Update()
    {
        timer += Time.deltaTime;
        elapsedTime += Time.deltaTime;
        timerPowerUp -= Time.deltaTime;

        // Aumenta a dificuldade ao longo do tempo
        if (elapsedTime % 30 == 0)
        {
            spawnInterval = Mathf.Max(0.5f, initialSpawnInterval - elapsedTime / 300.0f);
        }

        if (timerPowerUp <= 0)
        {
            SpawnPowerUp();
            timerPowerUp = 30.0f;
        }

        AdjustProbabilities();

        if (timer >= spawnInterval && dm.dinos.Count > 0)
        {
            SpawnRandomElement();
            timer = 0;
        }
    }

    void AdjustProbabilities()
    {
        int dinoCount = dm.dinos.Count;

        // Ajuste baseado no número de dinossauros
        if (dinoCount > 5)
        {
            float adjustmentFactor = Mathf.Min(1.0f, (dinoCount - 5) / 35.0f); // Ajusta para um valor entre 0 e 1 baseado no número de dinossauros
            baseHumanProbability = Mathf.Max(0.1f, 0.5f - dinoCount * 30 / 100 - adjustmentFactor * 0.4f); // Diminui até 0.1
        }
        else
        {
            baseHumanProbability = 0.5f;
        }
     
        // Ajuste baseado no tempo de jogo
        float timeFactor = Mathf.Min(1.0f, elapsedTime / 600.0f); // Ajusta para um valor entre 0 e 1 baseado no tempo de jogo
        baseHumanProbability = Mathf.Max(0.0f, baseHumanProbability - timeFactor * 0.5f); // Diminui até 0.0

            baseObstacleProbability = 1.0f - baseHumanProbability - baseCoinProbability+dinoCount*1/100;
        // Ajuste das outras probabilidades
        // Ajusta a probabilidade de obstáculos
    }

    float CalculateBreakableElementProbability()
    {
        float totalProbability = 0.0f;
        foreach (var element in breakableElements)
        {
            totalProbability += element.spawnProbability;
        }
        return totalProbability;
    }

    void SpawnRandomElement()
    {
        float rand = Random.value;
        if (rand < baseObstacleProbability)
        {
            SpawnObstacle();
        }
        else if (rand < baseObstacleProbability + baseHumanProbability)
        {
            SpawnHuman();
        }
        else if (rand < baseObstacleProbability + baseHumanProbability + CalculateBreakableElementProbability())
        {
            SpawnBreakableElement();
        }
        else
        {
            SpawnCoin();
        }
    }

    void SpawnObstacle()
    {
        int obstacleCount = Random.Range(1, 4); // 1 a 3 obstáculos
        float spacing = Random.Range(1.0f, 3.0f); // Espaçamento entre 1m e 3m

        for (int i = 0; i < obstacleCount; i++)
        {
            int randomIndex = Random.Range(0, obstacles.Length);
            Instantiate(obstacles[randomIndex], transform.position, Quaternion.identity);
        }
    }

    void SpawnHuman()
    {
        int humanCount = dm.dinos.Count > 5 ? 1 : Random.Range(1, 4); // 1 a 3 humanos se dinossauros <= 5, senão 1 humano
        for (int i = 0; i < humanCount; i++)
        {
            int randomIndex = Random.Range(0, humanPrefabs.Length);
            Vector3 spawnPosition = new Vector3(10 + i * humanSpacing, Random.Range(-2, 2), transform.position.z); // Ajuste no espaçamento horizontal
            Instantiate(humanPrefabs[randomIndex], spawnPosition, Quaternion.identity);
        }
    }

    void SpawnCoin()
    {
        Vector3 startPosition = new Vector3(10, Random.Range(-2, 2), transform.position.z);
        int pattern = Random.Range(0, 5); // 5 padrões de moedas

        for (int i = 0; i < 5; i++)
        {
            Vector3 spawnPosition = startPosition;

            if (pattern == 0) // Linear de cima para baixo
                spawnPosition.y -= i;
            else if (pattern == 1) // Linear de baixo para cima
                spawnPosition.y += i;
            else if (pattern == 2) // Linha reta embaixo
                spawnPosition.y = -2;
            else if (pattern == 3) // Linha reta em cima
                spawnPosition.y = 2;
            else if (pattern == 4) // Zig-zag
                spawnPosition.y += (i % 2 == 0) ? 1 : -1;

            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }

    void SpawnPowerUp()
    {
        Vector3 spawnPosition = new Vector3(10, 0, transform.position.z); // Sempre na altura do meio
        Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);
    }

    void SpawnBreakableElement()
    {
        float rand = Random.value;
        float cumulativeProbability = 0.0f;

        foreach (var element in breakableElements)
        {
            cumulativeProbability += element.spawnProbability;

            if (rand < cumulativeProbability)
            {
                Instantiate(element.gameObject, transform.position, Quaternion.identity);
                break;
            }
        }
    }
}

[System.Serializable]
public class BreakableElement
{
    public GameObject gameObject;
    public float spawnProbability; // Probabilidade de spawn desse elemento
}