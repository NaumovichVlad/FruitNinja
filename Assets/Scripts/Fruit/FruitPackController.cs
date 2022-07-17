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
    [SerializeField] private List<GameObject> fruitPrefabs;

    private class FruitState
    {
        public GameObject Fruit;

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
            fruitState.RotationSpeed = GetRandomRotateSpeed();
            fruitState.Fruit = Instantiate(fruit);
            _fruits.Add(fruitState);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       Initialize();
    }

    void FixedUpdate()
    {
        RemoveMissingFruits();
        DestroyMissingFruits();
        MoveAndRotate();
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
