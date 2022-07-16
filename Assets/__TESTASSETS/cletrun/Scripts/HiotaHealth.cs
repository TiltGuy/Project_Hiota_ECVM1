using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Controller_FSM))]
public class HiotaHealth : MonoBehaviour
{
	public float _health;
	private float _maxHealth;
	public Image healthBar;
	public float healthPointBarFillAmount;
	private float _maxGuard;
	private float _currentGuard;
	[SerializeField]
	private Image guardBarImage;
	private float guardPointBarFillAmount;
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
		healthPointBarFillAmount = _health / _maxHealth;
		healthBar.fillAmount = healthPointBarFillAmount;

		_currentGuard = controller.statCurrentGuard;
		_maxGuard = controller.HiotaStats.maxGuard;
		guardPointBarFillAmount = _currentGuard / _maxGuard;
		guardBarImage.fillAmount = guardPointBarFillAmount;


		
		//UPDATE DELEGATE
	}

    private void OnEnable()
    {
		controller.LoseHPDelegate += UpdateHPFillBar;
		controller.UpdateGuardAmountDelegate += UpdateGuardBar;
	}

    private void OnDisable()
    {
		controller.LoseHPDelegate -= UpdateHPFillBar;
		controller.UpdateGuardAmountDelegate -= UpdateGuardBar;
	}

    void Update()
	{
		if (_health == 0)
		{
			Debug.Log("tu es décédé", this);
		}
	}

	//public void Hurt(float attackDamage)
	//{
	//	_health -= attackDamage;
	//	healthPointBarFillAmount = _health / _maxHealth;
	//	healthBar.fillAmount = healthPointBarFillAmount;
	//	//Debug.Log("Health: " + _health);
	//	//Debug.Log("Fill : " + Fill);
	//	//Debug.Log("Damages : " + attackDamage);
		
	//}

	void OnTriggerEnter (Collider other)
	{
		if(other.tag == "LifeLoot")
		{
			_health += 1;
			healthPointBarFillAmount += 0.2f;
			healthBar.fillAmount = healthPointBarFillAmount;
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

	void UpdateHPFillBar(float currentHpToUpdate)
    {
		_health = currentHpToUpdate;
		healthPointBarFillAmount = _health / _maxHealth;
		healthBar.fillAmount = healthPointBarFillAmount;
		//print("J'update mes HPs");
	}

	void UpdateGuardBar(float currentGuardPointToUpdate)
    {
		_currentGuard = currentGuardPointToUpdate;
		guardPointBarFillAmount = _currentGuard / _maxGuard;
		guardBarImage.fillAmount = guardPointBarFillAmount;


	}
	
}
