using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingController : MonoBehaviour
{
    [SerializeField] private float boost;
    [SerializeField] private ParticlesController particlesController;
    [SerializeField] private float halfsScatterAngle;
    void Start()
    {
        FruitController.CutEvent += OnCut;
    }

    private void OnCut(MoveController.MovingObject cutObject, List<GameObject> halfs, Sprite particle, Vector2 cutDirection, float cutSpeed)
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

            newDirection.x = cutObject.Direction.x * Mathf.Cos(Mathf.Deg2Rad * halfsScatterAngle);
            newDirection.y = cutObject.Direction.y * Mathf.Sin(Mathf.Deg2Rad * halfsScatterAngle * side);

            MoveController.GetInstance().AddMovingObject(new MoveController.MovingObject()
            {
                Instance = halfInstance,
                RotationSpeed = cutObject.RotationSpeed,
                Direction = newDirection
            }) ;

            ShadowController.GetInstance().AddShadow(halfInstance.transform.GetChild(0).gameObject);
        }
        particlesController.CreateParticles(cutObject.Instance, particle);
        Destroy(cutObject.Instance);
    }

    
}
