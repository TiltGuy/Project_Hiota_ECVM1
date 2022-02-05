using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Controller_FSM))]
public class HiotaHealth : MonoBehaviour
{
	[SerializeField]
	private CharacterStats_SO HiotaStats;
	public float _health;
	private float _maxHealth;
	public Image hiotaHealthBar;
	public float Fill;
	public Transform Particle_Damage_Taken;
	public Transform Particle_Health_Recovered;


    void Start()
	{
		
        _health = HiotaStats.baseHealth;
		_maxHealth = HiotaStats.maxHealth;
		Fill = _health / _maxHealth;
		hiotaHealthBar.fillAmount = Fill;
	}

	void Update()
	{
		if (_health == 0)
		{
			Debug.Log("tu es d�c�d�", this);
		}
	}

	public void Hurt(float attackDamage)
	{
		_health -= attackDamage;
		Fill = _health / _maxHealth;
		hiotaHealthBar.fillAmount = Fill;
		//Debug.Log("Health: " + _health);
		//Debug.Log("Fill : " + Fill);
		//Debug.Log("Damages : " + attackDamage);
		if(!Particle_Damage_Taken)
        {
			Debug.Log("Particle_Damage_Taken EMPTY",this);
        }
		else
			Instantiate(Particle_Damage_Taken, transform.position, Quaternion.identity);
	}

	void OnTriggerEnter (Collider other)
	{
		if(other.tag == "LifeLoot")
		{
			_health += 1;
			Fill += 0.2f;
			hiotaHealthBar.fillAmount = Fill;
			//print("Regeneratiooonnn !!!");
			Debug.Log("Health: " + _health,this);
			Destroy(other.gameObject);
			if(!Particle_Health_Recovered)
            {
				Debug.Log("Particle_Health_Recovered EMPTY", this);
			}
			else
				Instantiate(Particle_Health_Recovered, transform.position, Quaternion.identity);
		}
	}

	
}
