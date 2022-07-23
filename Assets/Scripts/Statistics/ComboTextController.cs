using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTextController : MonoBehaviour
{
    [SerializeField] private Text comboText;
    [SerializeField] private float transparencySpeed;

    private const string _pattern = "X";

    public void SetCombo(int combo)
    {
        comboText.text = _pattern + combo;
    }

    private void Awake()
    {
        
    }

    private void Update()
    {
        var color = comboText.color;

        color.a -= transparencySpeed * Time.deltaTime;
        comboText.color = color;

        if(color.a < 0)
        {
            Destroy(gameObject);
        }
    }
}
