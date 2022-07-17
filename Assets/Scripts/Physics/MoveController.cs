using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{

    [SerializeField] private Vector2 attractiveForce;

    public void MoveAndRotate(GameObject fruit, ref Vector2 direction, float rotationSpeed)
    {
        fruit.transform.Translate(direction, Space.World);
        fruit.transform.Rotate(new Vector3(0, 0, rotationSpeed));
        direction += attractiveForce;
    }

    
}
