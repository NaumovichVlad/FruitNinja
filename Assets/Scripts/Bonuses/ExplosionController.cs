using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer explosionLayerRenderer;
    [SerializeField] private List<Sprite> explosionLayersSprites;
    [SerializeField] private int existanceTime;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float explosionScale;

    

    private readonly List<GameObject> _layers = new List<GameObject>();
    private int _counter = 0;

    private void Start()
    {
        InitializeExplosion();
    }

    private void InitializeExplosion()
    {
        foreach (var sprite in explosionLayersSprites)
        {
            explosionLayerRenderer.sprite = sprite;

            var explosionLayer = Instantiate(explosionLayerRenderer.gameObject);
            explosionLayer.transform.SetParent(gameObject.transform, false);
            explosionLayer.transform.localScale = Vector3.zero;

            _layers.Add(explosionLayer);
        }
    }

    private void Update()
    {
        if (_counter < existanceTime)
        {
            Animate();

            _counter++;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Animate()
    {
        var scale = new Vector3(explosionScale * Time.deltaTime, explosionScale * Time.deltaTime, explosionScale * Time.deltaTime);
        var multiplier = -1;

        _layers[_layers.Count - 1].transform.localScale += scale;
        _layers[_layers.Count - 1].transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime * multiplier));

        for (var i = _layers.Count - 1; i > 0; i--)
        {
            multiplier *= -1;

            _layers[i - 1].transform.localScale = _layers[i].transform.localScale + scale;
            _layers[i - 1].transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime * multiplier));
        }
    }
}
