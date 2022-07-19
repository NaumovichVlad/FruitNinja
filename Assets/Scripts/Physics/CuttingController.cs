using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingController : MonoBehaviour
{
    // Start is called before the first frame update

    private readonly List<MoveController.MovingObject> _halfs = new List<MoveController.MovingObject>();

    [SerializeField] MoveController moveController;
    [SerializeField] private float boost;
    void Start()
    {
        FruitPackController.CutEvent += OnCut;
    }

    private void OnCut(MoveController.MovingObject cutObject, Vector2 cutDirection, float cutSpeed)
    {
        const int side = -1;
        for (var i = 0; i < cutObject.Instance.transform.childCount; i++)
        {
            cutDirection.y *= side;

            var half = cutObject.Instance.transform.GetChild(i);
            var halfInstance = Instantiate(half.gameObject, half.transform.position, half.transform.rotation);

            _halfs.Add(new MoveController.MovingObject()
            {
                Instance = halfInstance,
                RotationSpeed = cutObject.RotationSpeed,
                Direction = cutDirection * cutSpeed * boost
            }) ;

            ShadowController.GetInstance().AddShadow(halfInstance.transform.GetChild(0).gameObject);

            cutDirection.x *= side;
            
        }

        Destroy(cutObject.Instance);
    }

    private Vector2 CalculateDirection(Vector2 parentDirection, Vector2 cutDirection)
    {
        var speed = parentDirection.magnitude;
        return Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        for (var i = 0; i < _halfs.Count; i++)
        {
            if (moveController.MoveAndRotate(_halfs[i]))
            {
                _halfs.RemoveAt(i--);
            }
        }
    }
}
