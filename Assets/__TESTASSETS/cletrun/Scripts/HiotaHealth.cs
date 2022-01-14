using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiotaHealth : MonoBehaviour
{
	public float _health;
	private float _maxHealth;
	public Image hiotaHealthBar;
	public float Fill;
	public Transform Particle_Damage_Taken;
	public Transform Particle_Health_Recovered;


	void Start()
	{
		Fill = 1f;
		_health = 10;
		_maxHealth = 10;
	}

	void Update()
	{
		if (_health == 0)
		{
			Destroy(this.gameObject);
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
		Instantiate(Particle_Damage_Taken, transform.position, Quaternion.identity);
	}

	void OnTriggerEnter (Collider other)
	{
		if(other.tag == "LifeLoot")
		{
			_health += 1;
			Fill += 0.2f;
			hiotaHealthBar.fillAmount = Fill;
			print("Regeneratiooonnn !!!");
			Debug.Log("Health: " + _health);
			Destroy(other.gameObject);
			Instantiate(Particle_Health_Recovered, transform.position, Quaternion.identity);
		}
	}

	
}
