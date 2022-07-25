using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTextController : MonoBehaviour
{
    [SerializeField] private Text comboText;
    [SerializeField] private float transparencySpeed;

    public void SetCombo(int combo)
    {
        comboText.text = combo.ToString();
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
