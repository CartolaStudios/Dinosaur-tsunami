using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScenarioSpawner : MonoBehaviour
{
    public GameObject[] buildingPrefabs; // Array de prefabs de prédios
    public float spawnMinInterval = 2f; // Intervalo mínimo de spawn
    public float spawnMaxInterval = 5f; // Intervalo máximo de spawn
    public float moveSpeed = 5f; // Velocidade de movimento dos prédios
    public float buildingLifetime = 10f; // Tempo de vida dos prédios

    private float nextSpawnTime;
    public Vector3 rotanteValue;
    void Start()
    {
        ScheduleNextSpawn();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnBuilding();
            ScheduleNextSpawn();
        }
    }

    void ScheduleNextSpawn()
    {
        nextSpawnTime = Time.time + Random.Range(spawnMinInterval, spawnMaxInterval);
    }

    void SpawnBuilding()
    {
        GameObject buildingPrefab = buildingPrefabs[Random.Range(0, buildingPrefabs.Length)];
        GameObject buildingInstance = Instantiate(buildingPrefab, transform.position, Quaternion.identity);
        buildingInstance.transform.rotation = Quaternion.Euler(rotanteValue.x,rotanteValue.y ,rotanteValue.z);

        BuildingMovement movementScript = buildingInstance.AddComponent<BuildingMovement>();
        movementScript.moveSpeed = moveSpeed;
        movementScript.lifetime = buildingLifetime;
    }
}