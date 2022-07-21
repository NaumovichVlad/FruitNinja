using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class FruitPackController : MonoBehaviour
{
    private readonly List<MoveController.MovingObject> _fruits = new List<MoveController.MovingObject>();

    public float LaunchMaxAngle;
    public float LaunchMinAngle;

    public static event OnCut CutEvent;
    public delegate void OnCut(MoveController.MovingObject cutObject, Vector2 cutDirection, float cutSpeed);

    [SerializeField] private float minRotateSpeed;
    [SerializeField] private float maxRotateSpeed;
    [SerializeField] private int initialFruitsCount;
    [SerializeField] private float speed;
    [SerializeField] private GameObject fruitPrefab;

    private void Initialize()
    {
        for (var i = 0; i < initialFruitsCount; i++)
        {
            var fruitState = new MoveController.MovingObject();

            fruitPrefab.transform.position = transform.position;
            fruitState.Direction = CalculateRandomDirection();
            fruitState.RotationSpeed = GetRandomRotateSpeed();
            fruitState.Instance = Instantiate(fruitPrefab);

            MoveController.GetInstance().AddMovingObject(fruitState);
            _fruits.Add(fruitState);
        }
    }

    void Start()
    {
        Initialize();
        SwipeDetection.SwipeEvent += OnSwipe;
    }

    void Update()
    {
        RemoveMissingFruits();
    }

    private void OnSwipe(Vector2 direction, Vector2 swipePosition, float swipeSpeed)
    {
        for (var i = 0; i < _fruits.Count; i++)
        {
            if (_fruits[i].Instance)
            {
                if (CalculateLength((Vector2)_fruits[i].Instance.transform.position, swipePosition) < _fruits[i].Instance.transform.localScale.x)
                {
                    CutEvent(_fruits[i], direction, swipeSpeed);
                    _fruits.RemoveAt(i--);
                }
            }
            
        }
    }

    private float CalculateLength(Vector2 firstVector, Vector2 secondVector)
    {
        return Mathf.Sqrt(Mathf.Pow(firstVector.x - secondVector.x, 2) + Mathf.Pow(firstVector.y - secondVector.y, 2));
    }

    private void RemoveMissingFruits()
    {
        for (var i = 0; i < _fruits.Count; i++)
        {
            if (!_fruits[i].Instance)
            {
                _fruits.RemoveAt(i);
                i--;
            }
        }

        if (!_fruits.Any())
        {
            Destroy(gameObject);
        }
    }
    private float GetRandomRotateSpeed()
    {
        var direction = new int[] { -1, 1 };

        return Random.Range(minRotateSpeed, maxRotateSpeed) * direction[Random.Range(0, 1)];
    }

    private Vector2 CalculateRandomDirection()
    {
        var angle = Random.value * (LaunchMaxAngle - LaunchMinAngle) + LaunchMinAngle;

        return new Vector2(speed * Mathf.Cos(Mathf.Deg2Rad * angle), speed * Mathf.Sin(Mathf.Deg2Rad * angle));
    }
}
