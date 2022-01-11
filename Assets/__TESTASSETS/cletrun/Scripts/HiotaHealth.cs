using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiotaHealth : MonoBehaviour
{
	private int _health;
	public Image hiotaHealthBar;
	public float Fill;

	void Start()
	{
		Fill = 1f;
		_health = 8;
	}

	void Update()
	{
		if (Fill <= 0)
		{
			Destroy(gameObject);
		}
	}

	public void Hurt(int attackDamage)
	{
		_health -= attackDamage;
		Fill -= 0.2f;
		hiotaHealthBar.fillAmount = Fill;
		Debug.Log("Health: " + _health);

		
	}
}
