using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    private static HealthController instance;
    private readonly Stack<GameObject> _healthes = new Stack<GameObject> ();
    private readonly List<GameObject> _removedHealth = new List<GameObject> ();

    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject healthPrefab;
    [SerializeField] private int startHealthCount;
    [SerializeField] private float scaleSpeed;

    private void Awake()
    {
        instance = this;
        InitializeHealth();
    }

    public static HealthController GetInstance()
    {
        return instance;
    }

    private void InitializeHealth()
    {
        for (var i = 0; i < startHealthCount; i++)
        {
            var health = Instantiate(healthPrefab, gameObject.transform);

            health.transform.Translate(new Vector2 (-i * health.transform.lossyScale.x, 0));

            _healthes.Push(health);
        }
    }

    public void RemoveHealth()
    {
        if (_healthes.Count > 0)
        {
            _removedHealth.Add(_healthes.Pop());
        }
        else
        {
            Application.Quit();

#if DEBUG
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    private void Update()
    {
        for (var j = 0; j < _removedHealth.Count; j++)
        {
            if (_removedHealth[j].transform.localScale.x > 0)
            {
                _removedHealth[j].transform.localScale -= 
                    new Vector3(scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime, scaleSpeed * Time.deltaTime);
            }
            else
            {
                Destroy(_removedHealth[j]);
                _removedHealth.RemoveAt(j--);
            }
        }
    }
}
