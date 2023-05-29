using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Controller_FSM))]
public class CharacterSpecs : MonoBehaviour
{

	#region DEPENDENCIES
	[Header(" -- DEPENDENCIES -- ")]

	public CharacterStats_SO CharStats_SO;
	private Controller_FSM controller;
    private PauseManager pauseManager;

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

    public System.Action<CharacterSpecs> onHealthDepleted;
    public UnityEvent onKilled;
    public delegate void MultiCastDelegate();
    public MultiCastDelegate OnSomethingKilledMe;
    public delegate void MultiCastDelegateWithFloat(float value);
    public MultiCastDelegateWithFloat OnLoosingHealth;

    //[HideInInspector]
    private float health;
   // [HideInInspector]
    private float currentGuard;
    private float _maxHealth;
	private float _maxGuard;
    private float startArmor;
	private float currentArmor;
    public float currentDamagesModifier = 0f;
	public float Health
	{
		get => health;
		set
        {
            health = Mathf.Clamp(value, 0, _maxHealth);
            healthPointBarFillAmount = health / _maxHealth;
            healthBar.value = healthPointBarFillAmount;
            OnLoosingHealth?.Invoke(healthPointBarFillAmount);
            if(health < MaxHealth)
            {
                DisplayEnemiesHPBarOnTouch();
            }
            if ( health <= 0f )
            {
                controller.currentCharacterTarget = null;
                //Debug.Log(this + " => Killed");
                onHealthDepleted?.Invoke(this);
                onKilled?.Invoke();
                OnSomethingKilledMe?.Invoke();
                if(pauseManager)
                {
                    Destroy(pauseManager.gameObject);
                }
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
				if(guardBarImage)
				{
					UpdateGuardBar(currentGuard);
				}

			}
			else
			{
				currentGuard = value;
				if (guardBarImage)
				{
					UpdateGuardBar(currentGuard);
				}
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

	public float CurrentArmor 
	{ 
		get => currentArmor;
		set 
		{
            currentArmor = value;
            currentArmor = Mathf.Clamp(value, 0f, value);
		}
	}

	#endregion

	#region IMAGES DEPENDENCIES
	[Header(" -- IMAGES DEPENDENCIES -- ")]

	public Slider healthBar;
	public float healthPointBarFillAmount;
	[SerializeField]
	private Slider guardBarImage;
	private float guardPointBarFillAmount;
	#endregion

	#region FX SETTINGS
	[Header(" -- FX SETTINGS -- ")]
	public Transform Particle_Damage_Taken;
	public Transform Particle_Health_Recovered;


    #endregion

    #region SOUND SETTINGS
    [Header(" -- SOUND SETTINGS -- ")]

    public FMODUnity.EventReference HealEvent;


    #endregion

    private void Awake()
    {
		controller = GetComponent<Controller_FSM>();
        if ( gameObject.CompareTag("Player") )
        {
            GameObject PM = GameObject.Find("PauseManager");
            pauseManager = PM?.GetComponent<PauseManager>();
            if ( pauseManager != null )
            {
                //Debug.Log("Pause Manager = " + pauseManager,pauseManager);
            }
            else
            {
                Debug.LogWarning("Pause manager not found");
            }
        }
    }

    void Start()
	{
		MaxHealth = CharStats_SO.maxHealth;
		Health = CharStats_SO.StartHealth;
        if(gameObject.CompareTag("Player"))
        {
            //Debug.Log("transform.position of the player : " + transform.position);
        }
		//healthPointBarFillAmount = Health / MaxHealth;
		//healthBar.fillAmount = healthPointBarFillAmount;

		MaxGuard = CharStats_SO.maxGuard;
		CurrentGuard = CharStats_SO.baseGuard;
        //guardPointBarFillAmount = CurrentGuard / MaxGuard;
        //guardBarImage.fillAmount = guardPointBarFillAmount;

        startArmor = CharStats_SO.baseArmor;
        CurrentArmor = startArmor;

		
		//UPDATE DELEGATE
	}

    private void OnEnable()
    {
		controller.UpdateGuardAmountDelegate += UpdateGuardBar;
        controller.OnPerfectGuard += RegenerateLife;

    }

    private void OnDisable()
    {
		controller.UpdateGuardAmountDelegate -= UpdateGuardBar;
        controller.OnPerfectGuard -= RegenerateLife;
        onHealthDepleted = null;
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

    public void UnRegisterSelf()
    {
        DeckManager.instance.UnRegisterEnemy(this.gameObject);
    }

	void OnTriggerEnter (Collider other)
	{
		if(other.tag == "LifeLoot")
		{
			Health += 1;
			healthPointBarFillAmount += 0.2f;
			healthBar.value = healthPointBarFillAmount;
			//print("Regeneratiooonnn !!!");
			//Debug.Log("Health: " + Health,this);
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

	void UpdateGuardBar(float currentGuardPointToUpdate)
    {
		currentGuard = currentGuardPointToUpdate;
		guardPointBarFillAmount = currentGuard / _maxGuard;
		guardBarImage.value = guardPointBarFillAmount;
	}

    [ContextMenu("Kill")]
    public void Kill()
    {
        this.Health = 0;
    }

    public void RegenerateLife()
    {
        float valueToGain = CharStats_SO.healthGainedWhenPGuarding;
        Health += valueToGain;
        Instantiate(Particle_Health_Recovered, this.gameObject.transform.position, Quaternion.identity);


        FMODUnity.RuntimeManager.PlayOneShotAttached(HealEvent, gameObject);
        //Debug.Log("REGENERATE", this);
    }

    public void RegenerateLife(float amountToRegain)
    {
        Health += amountToRegain;
        Instantiate(Particle_Health_Recovered, this.gameObject.transform.position, Quaternion.identity);
        //Debug.Log("REGENERATE", this);

        FMODUnity.RuntimeManager.PlayOneShotAttached(HealEvent, gameObject);
    }

    private void DisplayEnemiesHPBarOnTouch()
    {
        IABrain currentBrain = transform.GetComponent<IABrain>();
        if(transform.CompareTag("Enemy") && currentBrain != null)
        {
            currentBrain.DisplayHealthBar(true, false);
        }
    }

}
