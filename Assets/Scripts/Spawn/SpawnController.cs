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

    private int _fruitCount;
    private float _timeStep;
    private Coroutine _spawnCoroutine;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<SpawnZone> spawnZones;
    [SerializeField] private GameObject fruitPrefab;
    [SerializeField] private float minRotateSpeed;
    [SerializeField] private float maxRotateSpeed;
    [SerializeField] private float fruitSpeed;
    [SerializeField] private float minTimeStep;
    [SerializeField] private float maxTimeStep;
    [SerializeField] private int startPackCount;
    [SerializeField] private int maxPackCount;
    [SerializeField] private int scoreForMinStep;
    [SerializeField] private int scoreForMaxPack;

    void Start()
    {
        LosePopUpController.LoseEvent += OnLose;
        LosePopUpController.RestartEvent += Launch;
        _timeStep = maxTimeStep;
        _fruitCount = startPackCount;
        Launch();
    }

    private void OnLose()
    {
        StopCoroutine(_spawnCoroutine);
    }

    void Launch()
    {
        var frequencies = CalculateFrequency();

        _spawnCoroutine = StartCoroutine(SpawnFruits(frequencies));
    }

    IEnumerator SpawnFruits(float[] frequencies)
    {
        yield return new WaitForSeconds(_timeStep);

        var frequency = Random.value * frequencies[frequencies.Length - 1];

        IncreaseSpawnSpeed();

        for (var i = 0; i < spawnZones.Count; i++)
        {
            if (frequencies[i] > frequency)
            {
                var spawnPoint = CreateRandomSpawnInZone(spawnZones[i].SizeInPercent, spawnZones[i].Side);
                var launchMinAngle = ConvertAngle(spawnZones[i].LaunchMinAngle, spawnPoint);
                var launchMaxAngle = ConvertAngle(spawnZones[i].LaunchMaxAngle, spawnPoint);

                for (var j = 0; j < Random.Range(1, _fruitCount + 1); j++)
                {
                    var fruitState = new MoveController.MovingObject();

                    fruitPrefab.transform.position = spawnPoint;
                    fruitState.Direction = CalculateRandomDirection(launchMinAngle, launchMaxAngle);
                    fruitState.RotationSpeed = GetRandomRotateSpeed();
                    fruitState.Instance = Instantiate(fruitPrefab);

                    MoveController.GetInstance().AddMovingObject(fruitState);
                }
               
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

    private float GetRandomRotateSpeed()
    {
        var direction = new int[] { -1, 1 };

        return Random.Range(minRotateSpeed, maxRotateSpeed) * direction[Random.Range(0, 1)];
    }

    private Vector2 CalculateRandomDirection(float minLaunchAngle, float maxLaunchAngle)
    {
        var angle = Random.value * (maxLaunchAngle - minLaunchAngle) + minLaunchAngle;

        return new Vector2(fruitSpeed * Mathf.Cos(Mathf.Deg2Rad * angle), fruitSpeed * Mathf.Sin(Mathf.Deg2Rad * angle));
    }

    private void IncreaseSpawnSpeed()
    {
        var score = ScoreCounterController.GetInstance().GetScore();

        if (score <= scoreForMaxPack)
        {
            var scoreForIncrease = scoreForMaxPack / (maxPackCount - startPackCount);
            _fruitCount =startPackCount + score / scoreForIncrease;
        }

        if (score <= scoreForMinStep)
        {
            _timeStep = maxTimeStep - (float)score / scoreForMinStep * (maxTimeStep - minTimeStep);
        }
    }
}
