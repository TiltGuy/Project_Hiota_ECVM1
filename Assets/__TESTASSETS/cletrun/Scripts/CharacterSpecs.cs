using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Controller_FSM))]
public class CharacterSpecs : MonoBehaviour
{

	#region DEPENDENCIES
	[Header(" -- DEPENDENCIES -- ")]

	public CharacterStats_SO CharStats_SO;
	private Controller_FSM controller;

	#endregion


	#region ATTACK Settings

	[Header(" -- ATTACK SETTINGS -- ")]

	[Tooltip("The speedTurn of the player when it attack")]
	[SerializeField] public float m_speedTurnWhenAttack = 5f;

	[Tooltip("the speed of the rotation between the forward of the character and the direction to go when it's in Focus")]
	public float m_turnSpeedWhenFocused = 20;

	[Tooltip("the current Stats of the Basic Attack that will be used for the next or current hit")]
	public AttackStats_SO BasicAttackStats;

	[Tooltip("the current Stats and HitBox of the Side Attack that will be used for the next or current hit")]
	public AttackStats_SO SideAttackStats;

	[Tooltip("the current Stats and HitBox of the Front Attack that will be used for the next or current hit")]
	public AttackStats_SO FrontAttackStats;

	[Tooltip("the current Stats and HitBox of the Back Attack that will be used for the next or current hit")]
	public AttackStats_SO BackAttackStats;

	[Tooltip("the current Stats and HitBox of the Parry Attack that will be used for the next or current hit")]
	public AttackStats_SO ParryAttackStats;


    //[Tooltip("The speed of the player")]
    //public float m_HoldAttackSpeed = 5f;

    //[Tooltip("The time remaining of a Attack")]
    //public float maxAttackTime = .2f;


    #endregion

    #region GLOBAL SETTINGS
    [Header(" -- GLOBAL SETTINGS -- ")]

    [HideInInspector]
    private float health;
    [HideInInspector]
    private float currentGuard;
    private float _maxHealth;
	private float _maxGuard;
	public float Health
	{
		get => health;
		set
		{
			if (health < 0)
			{
				health = 0;
				UpdateHPFillBar(health);
			}
			else
			{
				health = value;
				UpdateHPFillBar(health);
			}
		}
	}


	public float CurrentGuard
	{
		get => currentGuard;
		set
		{
			if (currentGuard < 0)
			{
				currentGuard = 0;
				UpdateGuardBar(currentGuard);

			}
			else
			{
				currentGuard = value;
				UpdateGuardBar(currentGuard);
			}
		}
	}

	public float MaxHealth
	{
		get => _maxHealth;
		set
		{
			if (_maxHealth < 0)
			{
				_maxHealth = 0;
			}
			else
			{
				if (_maxHealth < health)
				{
					Health = _maxHealth;
				}
				else
				{
					_maxHealth = value;
				}
			}
		}
	}


	public float MaxGuard
	{
		get => _maxGuard;
		set
		{
			if (_maxGuard < 0)
			{
				_maxGuard = 0;
			}
			else
			{
				if (_maxGuard < currentGuard)
				{
					CurrentGuard = _maxGuard;
				}
				else
				{
					_maxGuard = value;
				}
			}
		}
	}

    private float currentArmor;
	public float CurrentArmor 
	{ 
		get => currentArmor;
		set 
		{ 
			if(currentArmor<0)
            {
				currentArmor = 0;
            }
			else
			{
				currentArmor = value;
			}
		}
	}

	#endregion

	#region IMAGES DEPENDENCIES
	[Header(" -- IMAGES DEPENDENCIES -- ")]

	public Image healthBar;
	public float healthPointBarFillAmount;
	[SerializeField]
	private Image guardBarImage;
	private float guardPointBarFillAmount;
	#endregion

	#region FX SETTINGS
	[Header(" -- FX SETTINGS -- ")]
	public Transform Particle_Damage_Taken;
	public Transform Particle_Health_Recovered;

    
    #endregion

    private void Awake()
    {
		controller = GetComponent<Controller_FSM>();
    }

    void Start()
	{
		MaxHealth = CharStats_SO.maxHealth;
		Health = CharStats_SO.baseHealth;
		//healthPointBarFillAmount = Health / MaxHealth;
		//healthBar.fillAmount = healthPointBarFillAmount;

		MaxGuard = CharStats_SO.maxGuard;
		CurrentGuard = CharStats_SO.baseGuard;
		//guardPointBarFillAmount = CurrentGuard / MaxGuard;
		//guardBarImage.fillAmount = guardPointBarFillAmount;

		CurrentArmor = CharStats_SO.baseArmor;

		
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
		if (Health == 0)
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
			Health += 1;
			healthPointBarFillAmount += 0.2f;
			healthBar.fillAmount = healthPointBarFillAmount;
			//print("Regeneratiooonnn !!!");
			Debug.Log("Health: " + Health,this);
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
		health = currentHpToUpdate;
		healthPointBarFillAmount = health / _maxHealth;
		healthBar.fillAmount = healthPointBarFillAmount;
		//print("J'update mes HPs");
	}

	void UpdateGuardBar(float currentGuardPointToUpdate)
    {
		currentGuard = currentGuardPointToUpdate;
		guardPointBarFillAmount = currentGuard / _maxGuard;
		guardBarImage.fillAmount = guardPointBarFillAmount;


	}
	
}
