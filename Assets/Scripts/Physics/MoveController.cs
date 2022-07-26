using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private readonly List<MovingObject> _movingObjects = new List<MovingObject>();
    private static MoveController moveController;


    [SerializeField] private Vector2 attractiveForce;

    private void Awake()
    {
        moveController = this;
        BombController.ExplosionEvent += OnExplode;
    }

    private void OnExplode(Vector2 explosionPosition, float explosionPower)
    {
        for (var i =0; i < _movingObjects.Count; i++)
        {
            var length = CalculateLength(explosionPosition, _movingObjects[i].Instance.transform.position);

            if (length < explosionPower)
            {
                var y = _movingObjects[i].Direction.y - explosionPosition.y;
                var x = _movingObjects[i].Direction.x - explosionPosition.x;

                _movingObjects[i].Direction.x += x;
                _movingObjects[i].Direction.y += y;
            }
        }
    }

    public int GetMovingObjectCount()
    {
        return _movingObjects.Count;
    }

    public static MoveController GetInstance()
    {
        return moveController;
    }

    public void AddMovingObject(MovingObject movingObject)
    {
        _movingObjects.Add(movingObject);
    }

    public MovingObject PeekMovingObject(GameObject gameObject)
    {
        for (int i = 0; i < _movingObjects.Count; i++)
        {
            if (_movingObjects[i].Instance.Equals(gameObject))
            {
                var result = _movingObjects[i];
                _movingObjects.RemoveAt(i);
                return result;
            }
        }

        return null;
    }

    private void MoveAndRotate(MovingObject movingObject)
    {
        CheckMissing(movingObject.Instance, movingObject.IsHealth);

        movingObject.Instance.transform.Translate(movingObject.Direction * Time.deltaTime, Space.World);
        movingObject.Instance.transform.Rotate(new Vector3(0, 0, movingObject.RotationSpeed * Time.deltaTime));
        movingObject.Direction += attractiveForce * Time.deltaTime;
    }

    private bool CheckMissing(GameObject instance, bool ishealth)
    {
        Vector3 point = Camera.main.WorldToViewportPoint(instance.transform.position);

        if (point.y < 0f || point.y > 1f || point.x > 1f || point.x < 0f)
        {
            if (ishealth)
            {
                HealthController.GetInstance().RemoveHealth();
            }

            Destroy(instance);
            return true;
        }
        return false;
    }

    private void Update()
    {
        for (var i = 0; i < _movingObjects.Count; i++)
        {
            if (_movingObjects[i].Instance == null)
            {
                _movingObjects.RemoveAt(i--);
            }
            else
            {
                MoveAndRotate(_movingObjects[i]);
            }

        }
    }

    private float CalculateLength(Vector2 firstVector, Vector2 secondVector)
    {
        return Mathf.Sqrt(Mathf.Pow(firstVector.x - secondVector.x, 2) + Mathf.Pow(firstVector.y - secondVector.y, 2));
    }

    public class MovingObject
    {
        public bool IsHealth;

        public GameObject Instance;

        public Vector2 Direction;

        public float RotationSpeed;
    }
}
