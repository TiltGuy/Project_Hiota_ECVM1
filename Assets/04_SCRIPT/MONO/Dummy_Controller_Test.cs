using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy_Controller_Test : MonoBehaviour, IDamageable
{
    public CharacterStats_SO characterStats;
    public float currentHealth;
    public GameObject HitFXprefab;
    private Collider coll;

    private void Start()
    {
        currentHealth = characterStats.baseHealth;
        coll = GetComponent<Collider>();
        Debug.Log(coll, this);
    }


    private float CalculateFinalDamages(float damages, float Armor)
    {
        float OutputDamage = Mathf.Clamp(damages - Armor, 0, damages);
        Debug.Log(OutputDamage,this);
        return OutputDamage;
    }

    private void LoseHP(float damageTaken)
    {
        if(currentHealth > 0)
        {
            currentHealth -= damageTaken;
        }
    }

    public void TakeDamages(float damageTaken, Transform striker)
    {
        float damageOuput = CalculateFinalDamages(damageTaken, characterStats.baseArmor);
        LoseHP(damageTaken);
        RaycastHit hit;

        Vector3 ClosestPointToStriker = coll.ClosestPointOnBounds(striker.position);
        Instantiate(HitFXprefab, ClosestPointToStriker, Quaternion.identity);

        //if (Physics.Raycast(transform.position, (striker.transform.position - transform.position), out hit))
        //{
        //    Instantiate(HitFXprefab, hit.point, Quaternion.identity);
        //}
        //LoseHP(damageTaken, currentHealth);
        //Debug.Log("ARGH!!! j'ai pris : " + CalculateFinalDamages(damages, characterStats.baseArmor) + " points de Dommages", this);
        Debug.Log("il ne me reste plus que " + currentHealth + " d'HP", this);
    }

    
}
