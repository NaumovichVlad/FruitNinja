using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private SpriteRenderer bombPartRenderer;
    [SerializeField] private Sprite bombSprite;
    [SerializeField] private float explosionPower;

    public static event OnExplosion ExplosionEvent;
    public delegate void OnExplosion(Vector2 explosionPosition, float explosionPower);

    private const int _bombPartsCount = 4;

    void Start()
    {
        InitializeBomb();
        SwipeDetection.SwipeEvent += OnSwipe;
    }

    private void InitializeBomb()
    {
        ShadowController.GetInstance().CreateShadow(gameObject, bombSprite);
    }

    private void OnSwipe(Vector2 direction, Vector2 swipePosition, float swipeSpeed)
    {
        if (CalculateLength((Vector2)gameObject.transform.position, swipePosition) < gameObject.transform.localScale.x)
        {
            var states = MoveController.GetInstance().PeekMovingObject(gameObject);
            
            LaunchParts(states);

            explosionPrefab.transform.position = gameObject.transform.position;

            ExplosionEvent(gameObject.transform.position, explosionPower);

            Instantiate(explosionPrefab);

            Destroy(gameObject);
        }
    }

    private void LaunchParts(MoveController.MovingObject states)
    {
        const float angle = 360;
        var launchAngle = angle / _bombPartsCount;

        var parts = CreateSpriteParts(bombSprite);

        for (var i = 0; i < _bombPartsCount; i++)
        {
            bombPartRenderer.sprite = parts[i];

            var partInstance = new MoveController.MovingObject()
            {
                Instance = Instantiate(bombPartRenderer.gameObject),
                RotationSpeed = states.RotationSpeed,
                Direction = new Vector2(
                    states.Direction.x * Mathf.Cos(Mathf.Deg2Rad * launchAngle * i), states.Direction.y * Mathf.Sin(Mathf.Deg2Rad * launchAngle * i))
            };

            partInstance.Instance.transform.position = gameObject.transform.position;
            partInstance.Instance.transform.rotation = gameObject.transform.rotation;

            MoveController.GetInstance().AddMovingObject(partInstance);
            ShadowController.GetInstance().CreateShadow(partInstance.Instance, parts[i]);
        }
        
    }

    private List<Sprite> CreateSpriteParts(Sprite texture)
    {
        var parts = new List<Sprite>();

        var xLength = texture.texture.width / _bombPartsCount * 2;
        var yLength = texture.texture.height / _bombPartsCount * 2;

        for (var i = 0; i < _bombPartsCount / 2; i++)
        {
            for (var j = 0; j < _bombPartsCount / 2; j++)
            {
                var rect = new Rect(xLength * i, yLength * j, xLength, yLength);
                var part = Sprite.Create(texture.texture, rect, new Vector2(1 - i, Mathf.Abs(j - 1)));

                parts.Add(part);
            }
        }

        return parts;
    }

    private float CalculateLength(Vector2 firstVector, Vector2 secondVector)
    {
        return Mathf.Sqrt(Mathf.Pow(firstVector.x - secondVector.x, 2) + Mathf.Pow(firstVector.y - secondVector.y, 2));
    }

    private void OnDestroy()
    {
        SwipeDetection.SwipeEvent -= OnSwipe;
    }
}
