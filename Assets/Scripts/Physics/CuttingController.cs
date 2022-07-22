using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingController : MonoBehaviour
{
    [SerializeField] private float boost;
    [SerializeField] private ParticlesController particlesController;
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

            MoveController.GetInstance().AddMovingObject(new MoveController.MovingObject()
            {
                Instance = halfInstance,
                RotationSpeed = cutObject.RotationSpeed,
                Direction = cutObject.Direction * side
            });

            ShadowController.GetInstance().AddShadow(halfInstance.transform.GetChild(0).gameObject);
            side *= -1;
        }
        particlesController.CreateParticles(cutObject.Instance, particle);
        Destroy(cutObject.Instance);
    }

    
}
