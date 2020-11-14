using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthFill;
    public Color healthColor;
    public Color ghostColor;
    public Color white;

    public void SetHealth(float health)
    {
        healthFill.value = health;
    }

    public void SetMaxHealth(float health)
    {
        healthFill.maxValue = health;
        healthFill.value = health;
    }

    public void SetColor(int select)
    {
        if (select == 1)
            healthFill.fillRect.GetComponent<Image>().color = healthColor;
        if(select == 2)
            healthFill.fillRect.GetComponent<Image>().color = ghostColor;
        if (select == 3)
            healthFill.fillRect.GetComponent<Image>().color = white;
    }

    public float getHealth()
    {
        return healthFill.value;
    }
}
