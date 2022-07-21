using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingController : MonoBehaviour
{

    [SerializeField] MoveController moveController;
    [SerializeField] private float boost;
    void Start()
    {
        FruitPackController.CutEvent += OnCut;
    }

    private void OnCut(MoveController.MovingObject cutObject, Vector2 cutDirection, float cutSpeed)
    {
        int side = -1;
        for (var i = 0; i < cutObject.Instance.transform.childCount; i++)
        {
            var half = cutObject.Instance.transform.GetChild(i);
            var halfInstance = Instantiate(half.gameObject, half.transform.position, half.transform.rotation);

            moveController.AddMovingObject(new MoveController.MovingObject()
            {
                Instance = halfInstance,
                RotationSpeed = cutObject.RotationSpeed,
                Direction = cutDirection + cutObject.Direction
            }) ;

            ShadowController.GetInstance().AddShadow(halfInstance.transform.GetChild(0).gameObject);

            cutDirection.x *= side;
            side *= -1;
        }

        Destroy(cutObject.Instance);
    }
}
