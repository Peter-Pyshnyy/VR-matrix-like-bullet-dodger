using UnityEngine;
using TMPro;
public class HealthScript : MonoBehaviour
{
    public int health = 3;
    public TextMeshProUGUI healthText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        updateText();
    }

    public void reduceHealth()
    {
        health -= 1;
        updateText();
    }
    void updateText()
    {
        healthText.text = "Health: " + health.ToString();
    }
}
