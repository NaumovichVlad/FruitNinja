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
    }

    public static MoveController GetInstance()
    {
        return moveController;
    }

    public void AddMovingObject(MovingObject movingObject)
    {
        _movingObjects.Add(movingObject);
    }
    private void MoveAndRotate(MovingObject movingObject)
    {
        movingObject.Instance.transform.Translate(movingObject.Direction * Time.deltaTime, Space.World);
        movingObject.Instance.transform.Rotate(new Vector3(0, 0, movingObject.RotationSpeed * Time.deltaTime));
        movingObject.Direction += attractiveForce * Time.deltaTime;

        CheckMissing(movingObject.Instance);
    }

    private bool CheckMissing(GameObject instance)
    {
        Vector3 point = Camera.main.WorldToViewportPoint(instance.transform.position);

        if (point.y < 0f || point.y > 1f || point.x > 1f || point.x < 0f)
        {
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

    public class MovingObject
    {
        public GameObject Instance;

        public Vector2 Direction;

        public float RotationSpeed;
    }
}
