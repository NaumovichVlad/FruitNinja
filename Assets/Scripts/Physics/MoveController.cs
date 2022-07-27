using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    private readonly List<MovingObject> _movingObjects = new List<MovingObject>();
    private readonly List<Magnetizm> _magnetizms = new List<Magnetizm>();
    private static MoveController moveController;

    private int _freezingTime;
    private float _freezingPower;
    private int _freezingTimer;
    private bool _isFreezed;

    private class Magnetizm
    {
        public float MagnetTime;

        public float MagnetPower;

        public float MagnetTimer;

        public float MagnetRadius;

        public Vector2 MagnetPosition;
    }

    


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

    public void AddFreezing(int freezingTime, float freezingPower)
    {
        _freezingPower = freezingPower;
        _freezingTime = freezingTime;
        _isFreezed = true;
    }

    public void AddMagnetization(Vector2 magnetPosition, int magnetizationTime, float magnetizationPower, float magnetizationRadius)
    {
        _magnetizms.Add(new Magnetizm()
        {
            MagnetPower = magnetizationPower,
            MagnetTime = magnetizationTime,
            MagnetPosition = magnetPosition,
            MagnetRadius = magnetizationRadius,
        });
    }

    private void MoveAndRotate(MovingObject movingObject)
    {
        CheckMissing(movingObject.Instance, movingObject.IsHealth);

        var booster = 1f;
        var magnetVector = Vector2.zero;

        if (_isFreezed)
        {
            booster = _freezingPower;
        }

        if (_magnetizms.Count > 0)
        {
            for (var i = 0; i < _magnetizms.Count; i++)
            {
                magnetVector += 
                    CalculateMagnitizationVector(_magnetizms[i].MagnetPosition, movingObject, _magnetizms[i].MagnetPower, _magnetizms[i].MagnetRadius);

                _magnetizms[i].MagnetTimer += Time.fixedDeltaTime;

                if (_magnetizms[i].MagnetTimer > _magnetizms[i].MagnetTime)
                {
                    movingObject.Direction += 
                        CalculateMagnitizationVector(_magnetizms[i].MagnetPosition, movingObject, _magnetizms[i].MagnetPower, _magnetizms[i].MagnetRadius);
                    _magnetizms.RemoveAt(i--);
                }
            }
        }

        
        movingObject.Instance.transform.Translate((movingObject.Direction + magnetVector) * Time.deltaTime / booster, Space.World);
        movingObject.Instance.transform.Rotate(new Vector3(0, 0, movingObject.RotationSpeed * Time.deltaTime / booster));
        movingObject.Direction += attractiveForce / booster * Time.deltaTime;
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
        if (_isFreezed)
        {
            if (_freezingTimer++ > _freezingTime)
            {
                _isFreezed = false;
                _freezingTimer = 0;
                _freezingTime = 0;
            }
        }


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

    private Vector2 CalculateMagnitizationVector(Vector2 magnetPosition, MovingObject instance, float magnetPower, float magnetRadius)
    {
        var distance = CalculateLength(magnetPosition, instance.Instance.transform.position);

        if (distance < magnetRadius)
        {
            var magnetVector = new Vector2();

            distance = distance < 1 ? 1 : distance;

            magnetPower /= distance;

            magnetVector.x = (magnetPosition.x - instance.Instance.transform.position.x) * magnetPower;
            magnetVector.y = (magnetPosition.y - instance.Instance.transform.position.y) * magnetPower;

            return magnetVector;
        }
        else
        {
            return Vector2.zero;
        }
    }

    public class MovingObject
    {
        public bool IsHealth;

        public GameObject Instance;

        public Vector2 Direction;

        public float RotationSpeed;
    }
}
