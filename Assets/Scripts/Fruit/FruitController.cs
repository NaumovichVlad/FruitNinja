using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    [System.Serializable]
    private class FruitSprites
    {
        public Sprite Fruit;

        public Sprite Particle;

        public Color JuiceColor;
    }

    private FruitSprites _fruit;

    [SerializeField] private List<FruitSprites> fruitSprites;
    [SerializeField] private SpriteRenderer fruitRenderer;

    private void InitializeFruit()
    {
        _fruit = GetRandomFruitSprite();
        fruitRenderer.sprite = _fruit.Fruit;
        ShadowController.GetInstance().CreateShadow(gameObject, _fruit.Fruit);
    }

    private FruitSprites GetRandomFruitSprite()
    {
        return fruitSprites[Random.Range(0, fruitSprites.Count)];
    }

    private void Start()
    {
        InitializeFruit();
        SwipeDetection.SwipeEvent += OnSwipe;
    }

    private void OnSwipe(Vector2 direction, Vector2 swipePosition, float swipeSpeed)
    {
        if (CalculateLength((Vector2)gameObject.transform.position, swipePosition) < gameObject.transform.localScale.x)
        {
            var states = MoveController.GetInstance().PeekMovingObject(gameObject);

            CuttingController.GetInstance().Cut(states, _fruit.Fruit, _fruit.Particle, _fruit.JuiceColor, direction, swipeSpeed);

            ScoreCounterController.GetInstance().AddScore(gameObject);

            Destroy(gameObject);
        }
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
