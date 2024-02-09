using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public Image Bar;
    public float healthAmount = 100f;
    public int life = 3;
    public TextMeshProUGUI lifeText;

    // Start is called before the first frame update
    void Start()
    {
        UpdateLifeText();
    }

    // Update is called once per frame
    void Update()
    {
        if (healthAmount <= 0)
        {
            if (life > 0)
            {
                life--;
                healthAmount = 120f;
                UpdateLifeText();
            }
            else
            {
                SceneManager.LoadScene("GameOver");
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            TakeDamage(20);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Heal(20);
        }
    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        Bar.fillAmount = healthAmount / 100f;
    }

    public void Heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);
        Bar.fillAmount = healthAmount / 100f;
    }

     void UpdateLifeText()
    {
        if (lifeText != null)
        {
            lifeText.text = "x" + life.ToString();
        }
    }
}
