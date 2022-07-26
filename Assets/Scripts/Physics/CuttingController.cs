using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingController : MonoBehaviour
{
    [SerializeField] private float boost;
    [SerializeField] private ParticlesController particlesController;
    [SerializeField] private float halfsScatterAngle;

    private static CuttingController cuttingController;

    private void Awake()
    {
        cuttingController = this;
    }

    void Start()
    {
        FruitController.CutEvent += OnCut;
    }

    public static CuttingController GetInstance()
    {
        return cuttingController;
    }

    private void OnCut(MoveController.MovingObject cutObject, List<GameObject> halfs, Sprite particle, Color juiceColor, Vector2 cutDirection, float cutSpeed)
    {
        Cut(cutObject, halfs, cutSpeed);

        particlesController.CreateParticles(cutObject.Instance, particle, juiceColor);
        Destroy(cutObject.Instance);
    }

    public void Cut(MoveController.MovingObject cutObject, List<GameObject> halfs, float cutSpeed)
    {
        int side = -1;

        for (var i = 0; i < cutObject.Instance.transform.childCount; i++)
        {
            var halfInstance = Instantiate(halfs[i], halfs[i].transform.position, halfs[i].transform.rotation);
            var newDirection = new Vector2();

            if (halfInstance.transform.rotation.z < 0)
            {
                side *= -1;
            }

            newDirection.x = (cutObject.Direction.x + Mathf.Cos(Mathf.Deg2Rad * halfsScatterAngle) * side);
            newDirection.y = cutObject.Direction.y + cutSpeed * side;

            MoveController.GetInstance().AddMovingObject(new MoveController.MovingObject()
            {
                Instance = halfInstance,
                RotationSpeed = cutObject.RotationSpeed,
                Direction = newDirection
            });

            ShadowController.GetInstance().AddShadow(halfInstance.transform.GetChild(0).gameObject);
        }
    }
}
