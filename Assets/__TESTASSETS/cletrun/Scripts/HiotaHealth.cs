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
		Debug.Log("Health: " + _health);
		Debug.Log("Fill : " + Fill);
		Debug.Log("Damages : " + attackDamage);
		
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
		}
	}

	
}
