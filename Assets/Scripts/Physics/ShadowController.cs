using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowController : MonoBehaviour
{
    private readonly List<GameObject> _shadows = new List<GameObject>();
    private static ShadowController shadowControllerinstance;

    [SerializeField] private GameObject shadowPrefab;
    [SerializeField] private float distanceFromObject;

    private void Awake()
    {
        shadowControllerinstance = this;
    }

    public static ShadowController GetInstance()
    {
        return shadowControllerinstance;
    }

    public void CreateShadow(GameObject instance)
    {
        var shadowSprite = shadowPrefab.GetComponent<SpriteRenderer>();
        shadowSprite.color = Color.black;
        shadowSprite.sprite = instance.GetComponent<SpriteRenderer>().sprite;
        var shadow = Instantiate(shadowPrefab);
        shadow.transform.SetParent(instance.transform, false);
        _shadows.Add(shadow);
    }

    public void AddShadow(GameObject shadow)
    {
        _shadows.Add(shadow);
    }

    void Update()
    {
        RemoveShadows();
        MoveAndRotateShadows();
    }

    private void RemoveShadows()
    {
        for (var i = 0; i < _shadows.Count; i++)
        {
            if (_shadows[i] == null)
            {
                _shadows.RemoveAt(i);
                i--;
            }
        }
    }

    private void MoveAndRotateShadows()
    {
        foreach (var shadow in _shadows)
        {
            if (shadow != null)
            {
                var parentPosition = new Vector2(shadow.transform.parent.transform.position.x, shadow.transform.parent.transform.position.y - distanceFromObject);

                shadow.transform.position = parentPosition;
            }
        }
    }
}
