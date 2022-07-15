using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FruitPackController : MonoBehaviour
{
    private readonly List<GameObject> _fruits = new List<GameObject>();

    public float LaunchMaxAngle;
    public float LaunchMinAngle;

    [SerializeField] private MoveController moveController;
    [SerializeField] private int initialFruitsCount;
    [SerializeField] private float speed;

    private void Initialize()
    {
        for (var i = 0; i < initialFruitsCount; i++)
        {
            moveController.Direction = CalculateRandomDirection();
            moveController.gameObject.transform.position = transform.position;
            _fruits.Add(Instantiate(moveController.gameObject));
        }
    }
    // Start is called before the first frame update
    void Start()
    {
       Initialize();
    }

    void Update()
    {
        RemoveMissingFruits();
        DestroyMissingFruits();
    }

    private void DestroyMissingFruits()
    {
        for (var i = 0; i < _fruits.Count; i++)
        {
            Vector3 point = Camera.main.WorldToViewportPoint(_fruits[i].transform.position);

            if (point.y < 0f || point.y > 1f || point.x > 1f || point.x < 0f)
            {
                Destroy(_fruits[i]);
            }
        }
    }

    private void RemoveMissingFruits()
    {
        for (var i = 0; i < _fruits.Count; i++)
        {
            if (_fruits[i] == null)
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

    private Vector2 CalculateRandomDirection()
    {
        var angle = Random.value * (LaunchMaxAngle - LaunchMinAngle) + LaunchMinAngle;

        return new Vector2(speed * Mathf.Cos(angle), speed * Mathf.Abs(Mathf.Sin(angle)));
    }
}
