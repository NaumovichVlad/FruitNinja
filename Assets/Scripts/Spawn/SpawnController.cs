using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
    [System.Serializable]
    private class SpawnZone
    {
        public float SizeInPercent;

        public Sides Side;

        public float LaunchMaxAngle;

        public float LaunchMinAngle;

        public float Frequency;
    }

    private enum Sides
    {
        Left,
        Right,
        Bottom
    }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<SpawnZone> spawnZones;
    [SerializeField] private FruitPackController fruitPackController;
    [SerializeField] private float timeStep;

    void Start()
    {
        Launch();
    }

    void Launch()
    {
        var frequencies = CalculateFrequency();
        StartCoroutine(SpawnFruits(frequencies));
    }

    IEnumerator SpawnFruits(float[] frequencies)
    {
        yield return new WaitForSeconds(timeStep);

        var frequency = Random.value * frequencies[frequencies.Length - 1];

        for (var i = 0; i < spawnZones.Count; i++)
        {
            if (frequencies[i] > frequency)
            {
                var spawnPoint = CreateRandomSpawnInZone(spawnZones[i].SizeInPercent, spawnZones[i].Side);
                fruitPackController.gameObject.transform.position = spawnPoint;
                fruitPackController.LaunchMinAngle = ConvertAngle(spawnZones[i].LaunchMinAngle, spawnPoint);
                fruitPackController.LaunchMaxAngle = ConvertAngle(spawnZones[i].LaunchMaxAngle, spawnPoint);
                Instantiate(fruitPackController.gameObject);
                break;
            }
        }
        Launch();
    }
    private Vector2 CreateRandomSpawnInZone(float size, Sides side)
    {
        var spawnPosition = new Vector2();
        float camHeight = mainCamera.orthographicSize * 2;
        float camWidth = mainCamera.aspect * camHeight;

        switch (side)
        {
            case Sides.Bottom:
                var fraction = camWidth * size / 100;
                spawnPosition.y = -camHeight / 2;
                spawnPosition.x = -fraction / 2 + Random.value * fraction;
                break;
            case Sides.Left:
                fraction = camHeight * size / 100;
                spawnPosition.x = -camWidth / 2;
                spawnPosition.y = -camHeight / 2 + Random.value * fraction;
                break;
            case Sides.Right:
                fraction = camHeight * size / 100;
                spawnPosition.x = camWidth / 2;
                spawnPosition.y = -camHeight / 2 + Random.value * fraction;
                break;
        }

        return spawnPosition;
    }

    private float[] CalculateFrequency()
    {
        var frequencies = new float[spawnZones.Count];

        frequencies[0] = spawnZones[0].Frequency;

        for (var i = 1; i < spawnZones.Count; i++)
        {
            frequencies[i] = frequencies[i - 1] + spawnZones[i].Frequency;
        }

        return frequencies;
    }

    private float ConvertAngle(float angle, Vector2 spawnPosition)
    {
        if (spawnPosition.x > 0)
        {
            return 180 - angle;
        }
        return angle;
    }
}
