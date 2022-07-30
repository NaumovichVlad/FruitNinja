using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingController : MonoBehaviour
{
    [SerializeField] private float boost;
    [SerializeField] private ParticlesController particlesController;
    [SerializeField] private SpriteRenderer halfRenderer;
    [SerializeField] private float halfsScatterAngle;
    [SerializeField] private int partCount;

    private static CuttingController cuttingController;

    private void Awake()
    {
        cuttingController = this;
    }

    public static CuttingController GetInstance()
    {
        return cuttingController;
    }

    public void Cut(MoveController.MovingObject cutObject, Sprite texture, Sprite particle, Color juiceColor, Vector2 cutDirection, float cutSpeed)
    {
        Cut(cutObject, texture, cutSpeed);

        particlesController.CreateParticles(cutObject.Instance, particle, juiceColor);
    }

    public void Cut(MoveController.MovingObject cutObject, Sprite texture, float cutSpeed, bool destroy = false)
    {
        var parts = CreateSpriteParts(texture);

        Cut(cutObject, parts, cutSpeed, destroy);
    }

    public void Cut(MoveController.MovingObject cutObject, List<Sprite> parts, float cutSpeed, bool destroy = false)
    {
        int side = -1;

        for (var i = 0; i < parts.Count; i++)
        {
            halfRenderer.sprite = parts[i];

            var halfInstance = Instantiate(halfRenderer.gameObject, cutObject.Instance.transform.position, cutObject.Instance.transform.rotation);
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

            ShadowController.GetInstance().CreateShadow(halfInstance, parts[i]);
        }

        if (destroy)
        {
            Destroy(cutObject.Instance);
        }

    }

    private List<Sprite> CreateSpriteParts(Sprite texture)
    {
        var parts = new List<Sprite>();

        var rect = new Rect(0f, 0f, texture.texture.width / 2, texture.texture.height);
        var part = Sprite.Create(texture.texture, rect, new Vector2(1f, 0.5f));
        parts.Add(part);
        
        rect = new Rect(texture.texture.width / 2, 0f, texture.texture.width / 2, texture.texture.height);
        part = Sprite.Create(texture.texture, rect, new Vector2(0f, 0.5f));
        parts.Add(part);

        return parts;
    }
}
