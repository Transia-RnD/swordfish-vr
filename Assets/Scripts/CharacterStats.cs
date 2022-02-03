using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public bool isLoggedIn = false;
    public string seed;
    public string defaultSignor;
    public decimal currencyTotal;
    public float currHealth;
    public float maxHealth;
    public float currStamina;
    public float maxStamina;
    public bool isDead = false;

    public void CheckHealth()
    {
        if (currHealth >= maxHealth)
        {
            currHealth = maxHealth;
        }
        if (currHealth <= 0)
        {
            currHealth = 0;
            isDead = true;
        }
    }

    public void CheckStamina()
    {
        if (currStamina >= maxStamina)
        {
            currStamina = maxStamina;
        }
        if (currStamina <= 0)
        {
            currStamina = 0;
            isDead = true;
        }
    }

    public virtual void Die()
    {

    }

    public void TakeDamage(float damage)
    {
        currHealth -= damage;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
