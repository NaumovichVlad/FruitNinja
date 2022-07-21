using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    [System.Serializable]
    private class FruitSprites
    {
        public Sprite FirstHalf;

        public Sprite SecondHalf;

        public Sprite Particle;
    }

    private FruitSprites _fruit;

    [SerializeField] private List<FruitSprites> fruitSprites;
    [SerializeField] private FruitHalfController fruitHalfCreator;

    private void InitializeFruit()
    {
        _fruit = GetRandomFruitSprite();
        AddHalf(_fruit.FirstHalf);
        AddHalf(_fruit.SecondHalf);

    }

    private void AddHalf(Sprite half)
    {
        fruitHalfCreator.CreateNewHalf(half);
        var halfInstance = Instantiate(fruitHalfCreator.gameObject);
        halfInstance.transform.SetParent(gameObject.transform, false);
        ShadowController.GetInstance().CreateShadow(halfInstance);
    }

    private FruitSprites GetRandomFruitSprite()
    {
        return fruitSprites[Random.Range(0, fruitSprites.Count)];
    }

    private void Start()
    {
        InitializeFruit();
    }
}
