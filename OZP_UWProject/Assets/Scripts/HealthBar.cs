using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Transform bar;
    void Start()
    {
        bar = transform.Find("health");
    }

    public void SetSize(float sizeNormalized)
    {
        bar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void SetColor (Color color)
    {
        bar.Find("Bar Sprite").GetComponent<Image>().color = color;
    }
}
