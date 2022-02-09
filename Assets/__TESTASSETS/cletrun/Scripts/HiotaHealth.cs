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

	private Controller_FSM controller;

    private void Awake()
    {
		controller = GetComponent<Controller_FSM>();
    }

    void Start()
	{
		
        _health = controller.statCurrentHealth;
		_maxHealth = controller.HiotaStats.maxHealth;
		Fill = _health / _maxHealth;
		hiotaHealthBar.fillAmount = Fill;

		controller.LoseHPDelegate += UpdateHPFillBar;
	}

	void Update()
	{
		if (_health == 0)
		{
			Debug.Log("tu es décédé", this);
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
	
	public void SpawnHitReactionFX(Transform targetFX)
    {
		if (!targetFX)
		{
			Debug.Log("Particle_Damage_Taken EMPTY", this);
		}
		else
			Instantiate(targetFX, transform.position, Quaternion.identity);
	}

	void UpdateHPFillBar()
    {
		_health = controller.statCurrentHealth;
		Fill = _health / _maxHealth;
		hiotaHealthBar.fillAmount = Fill;
		print("J'update mes HPs");
	}
	
}
