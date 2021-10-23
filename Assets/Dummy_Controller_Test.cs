using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_Controller_Test : MonoBehaviour, IDamageable
{
    public CharacterStats_SO characterStats;
    public float currentHealth;

    private void Start()
    {
        currentHealth = characterStats.baseHealth;
    }


    private float CalculateFinalDamages(float damages, float Armor)
    {
        float OutputDamage = Mathf.Clamp(damages - Armor, 0, damages);
        Debug.Log(OutputDamage);
        return OutputDamage;
    }

    public void TakeDamages(float damageTaken)
    {
        float damageOuput = CalculateFinalDamages(damageTaken, characterStats.baseArmor);
        LoseHP(damageTaken);
        //LoseHP(damageTaken, currentHealth);
        //Debug.Log("ARGH!!! j'ai pris : " + CalculateFinalDamages(damages, characterStats.baseArmor) + " points de Dommages", this);
        Debug.Log("il ne me reste plus que " + currentHealth + " d'HP", this);
    }

    private void LoseHP(float damageTaken)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageTaken;
        }
    }
}
