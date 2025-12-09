using UnityEngine;
using System.Collections.Generic;

public class CrowdManager : MonoBehaviour
{
    [Header("References")]
    public Dummy dummyPrefab;
    public Transform player;
    public Camera mainCamera;

    [Header("Initial Spawn (Spiral)")]
    public int maxEnemies = 100;
    public float innerDeadZoneRadius = 15f;
    public float spiralSpacing = 1.5f;

    [Header("Respawn")]
    public float spawnOutsideExtra = 10f;

    private List<Dummy> enemies = new();

    void Start()
    {
        if (!mainCamera)
            mainCamera = Camera.main;

        SpawnSpiralEnemies();
    }

    void SpawnSpiralEnemies()
    {
        float angle = 0f;
        float radius = innerDeadZoneRadius;

        for (int i = 0; i < maxEnemies; i++)
        {
            Dummy d = Instantiate(dummyPrefab);
            d.Init(player);
            d.OnDeath += OnEnemyDied;

            Vector3 pos = player.position + new Vector3(
                Mathf.Cos(angle) * radius,
                0f,
                Mathf.Sin(angle) * radius
            );

            d.Spawn(pos);
            enemies.Add(d);

            angle += 0.5f;
            radius += spiralSpacing / (2f * Mathf.PI);
        }
    }

    void OnEnemyDied(Dummy d)
    {
        // Респав за пределами экрана
        Vector3 pos = GetSpawnPositionOutsideScreen();
        d.Spawn(pos);
    }

    Vector3 GetSpawnPositionOutsideScreen()
    {
        int side = Random.Range(0, 4);
        Vector3 viewportPos = Vector3.zero;

        if (side == 0) viewportPos = new Vector3(-0.2f, Random.value, 0f);
        if (side == 1) viewportPos = new Vector3(1.2f, Random.value, 0f);
        if (side == 2) viewportPos = new Vector3(Random.value, -0.2f, 0f);
        if (side == 3) viewportPos = new Vector3(Random.value, 1.2f, 0f);

        Vector3 world = mainCamera.ViewportToWorldPoint(
            new Vector3(viewportPos.x, viewportPos.y, mainCamera.transform.position.y)
        );

        world.y = 0f;

        Vector3 dir = (world - player.position).normalized;
        world += dir * spawnOutsideExtra;

        return world;
    }
}
