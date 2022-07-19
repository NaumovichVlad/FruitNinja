using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{

    [SerializeField] private Vector2 attractiveForce;

    public bool MoveAndRotate(MovingObject movingObject)
    {
        movingObject.Instance.transform.Translate(movingObject.Direction * Time.deltaTime, Space.World);
        movingObject.Instance.transform.Rotate(new Vector3(0, 0, movingObject.RotationSpeed * Time.deltaTime));
        movingObject.Direction += attractiveForce * Time.deltaTime;

        return CheckMissing(movingObject.Instance);
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

    public class MovingObject
    {
        public GameObject Instance;

        public Vector2 Direction;

        public float RotationSpeed;
    }
}
