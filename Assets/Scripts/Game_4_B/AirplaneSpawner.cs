
using System.Collections.Generic;
using UnityEngine;

public class AirplaneSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Airplane airplanePrefab;
    [SerializeField] private RectTransform movementArea;
    [SerializeField] private RectTransform[] spawnPoints = new RectTransform[3];

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnDelay = 1f;
    [SerializeField] private float maxSpawnDelay = 2.5f;
    [SerializeField] private float airplaneSpeed = 500f;
    [SerializeField] private int maxActiveAirplanes = 2;

    private readonly List<Airplane> activeAirplanes = new();
    public IReadOnlyList<Airplane> ActiveAirplanes => activeAirplanes;
    private float spawnTimer;
    private bool spawning;

    private void Update()
    {
        activeAirplanes.RemoveAll(airplane => airplane == null);

        if (!spawning)
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f &&
            activeAirplanes.Count < maxActiveAirplanes)
        {
            SpawnAirplane();
            spawnTimer = Random.Range(minSpawnDelay, maxSpawnDelay);
        }
    }

    public void StartSpawning()
    {
        spawning = true;
        spawnTimer = 0f;
    }

    public void StopSpawning()
    {
        spawning = false;
    }

    public void ClearAirplanes()
    {
        spawning = false;

        foreach (Airplane airplane in activeAirplanes)
        {
            if (airplane != null)
                Destroy(airplane.gameObject);
        }

        activeAirplanes.Clear();
    }

    private void SpawnAirplane()
    {
        if (airplanePrefab == null ||
            movementArea == null ||
            spawnPoints == null ||
            spawnPoints.Length == 0)
            return;

        List<RectTransform> validPoints = new();

        foreach (RectTransform point in spawnPoints)
        {
            if (point != null)
                validPoints.Add(point);
        }

        if (validPoints.Count == 0)
            return;

        RectTransform selectedPoint =
            validPoints[Random.Range(0, validPoints.Count)];

        Airplane airplane = Instantiate(airplanePrefab, movementArea);

        RectTransform airplaneRect =
            airplane.GetComponent<RectTransform>();

        airplaneRect.position = selectedPoint.position;

        airplane.Initialize(movementArea, airplaneSpeed);
        activeAirplanes.Add(airplane);
    }
}
