using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Image healthFillImage;

    public int health;
    public int maxHealth;
    public bool isInvulnerable;
    public float invulerabilityPeriod;

    public static PlayerHealth Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one instance of the player health!");
            return;
        }
        Instance = this;

        GameEvents.Instance.onTakeDamage += TakeDamage;
    }

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        Updatehealth();
    }

    void Updatehealth()
    {
        float healthFillAmmount = ((float) health) / ((float)maxHealth);

        healthFillImage.fillAmount = healthFillAmmount;
    }

    void TakeDamage(int damage)
    {
        if (isInvulnerable == false)
        {
            health -= damage;
            StartCoroutine(InvulernableAfterDamage());
        }
        
    }

    IEnumerator InvulernableAfterDamage()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulerabilityPeriod);
        isInvulnerable = false;
    }

    private void OnDestroy()
    {
        GameEvents.Instance.onTakeDamage -= TakeDamage;
    }
}
