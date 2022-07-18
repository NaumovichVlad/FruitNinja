using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FruitPackController : MonoBehaviour
{
    private readonly List<FruitState> _fruits = new List<FruitState>();

    public float LaunchMaxAngle;
    public float LaunchMinAngle;

    [SerializeField] private MoveController moveController;
    [SerializeField] private float minRotateSpeed;
    [SerializeField] private float maxRotateSpeed;
    [SerializeField] private int initialFruitsCount;
    [SerializeField] private float speed;
    [SerializeField] private int fruitRadius;
    [SerializeField] private List<GameObject> fruitPrefabs;

    public class FruitState
    {
        public GameObject Fruit;

        public float Radius;

        public Vector2 Direction;

        public float RotationSpeed;
    }
    private void Initialize()
    {
        for (var i = 0; i < initialFruitsCount; i++)
        {
            var fruitState = new FruitState();
            var fruit = fruitPrefabs[Random.Range(0, fruitPrefabs.Count)];

            fruit.transform.position = transform.position;
            fruitState.Direction = CalculateRandomDirection();
            fruitState.Radius = fruitRadius;
            fruitState.RotationSpeed = GetRandomRotateSpeed();
            fruitState.Fruit = Instantiate(fruit);
            ShadowController.GetInstance().CreateShadow(fruitState.Fruit);

            _fruits.Add(fruitState);
        }
    }
    
    public List<FruitState> GetFruitStates()
    {
        return _fruits;
    }

    void Start()
    {
        Initialize();
        SwipeDetection.SwipeEvent += OnSwipe;
    }

    void FixedUpdate()
    {
        RemoveMissingFruits();
        DestroyMissingFruits();
        MoveAndRotate();
    }

    private void OnSwipe(Vector2 direction, Vector2 swipePosition)
    {
        for (var i = 0; i < _fruits.Count; i++)
        {
            if (CalculateLength((Vector2)_fruits[i].Fruit.transform.position, swipePosition) < _fruits[i].Radius)
            {
                Destroy(_fruits[i].Fruit);
                _fruits.RemoveAt(i);
                i--;
            }
            
        }
    }

    private float CalculateLength(Vector2 firstVector, Vector2 secondVector)
    {
        return Mathf.Sqrt(Mathf.Pow(firstVector.x - secondVector.x, 2) + Mathf.Pow(firstVector.y - secondVector.y, 2));
    }

    private void DestroyMissingFruits()
    {
        for (var i = 0; i < _fruits.Count; i++)
        {
            Vector3 point = Camera.main.WorldToViewportPoint(_fruits[i].Fruit.transform.position);

            if (point.y < 0f || point.y > 1f || point.x > 1f || point.x < 0f)
            {
                Destroy(_fruits[i].Fruit);
            }
        }
    }

    private void RemoveMissingFruits()
    {
        for (var i = 0; i < _fruits.Count; i++)
        {
            if (_fruits[i].Fruit == null)
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

    private void MoveAndRotate()
    {
        for (var i = 0; i < _fruits.Count; i++)
        {
            moveController.MoveAndRotate(_fruits[i].Fruit, ref _fruits[i].Direction, _fruits[i].RotationSpeed);
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
