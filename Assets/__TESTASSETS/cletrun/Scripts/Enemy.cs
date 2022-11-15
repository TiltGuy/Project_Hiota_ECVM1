using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform refAvatar;
    [SerializeField]
    private float detectionDistance;
    [SerializeField]
    private float attackDistance;
    [SerializeField]
    private float preparationAttackSpeed;
    [SerializeField]
    private float AttackSpeed;

    //ParticleSystem
    [SerializeField]
    private Transform AggroTaken;
    private bool EnemySpotPlayer;

    /*public GameObject projectile;
    public float cadenceTir;
    private float timerTir;
    private GameObject clone;*/

    private Vector3 dirAvatar;

    //Patrol
    public bool canDetect = false;
    [HideInInspector]
    public bool inRangeOfAttack = false;
    [SerializeField]
    private Transform zoneEnemy;
    [SerializeField]
    private Transform[] waypoints;

    private Controller_FSM HiotaController;

    //Health
    [SerializeField]
    private CharacterStats_SO characterStats;
    private float _currentHealth;
    private float _currentMaxHealth;
    [SerializeField]
    private GameObject HitFXprefab;
    private Collider coll;
    [SerializeField]
    public bool b_IsDead;

    [SerializeField]
    private bool b_CanMove = true;

    public float baseSpeed;

    //Health Bar
    public Image Bar;
    public float Fill;

    //Loot
    public GameObject lifeLoot;

    [SerializeField]
    private DamageHiota damageHiota;
    [SerializeField]
    private Animator enemyAnimator;

    public delegate void MultiDelegate();
    public MultiDelegate OnDeathEnemy;


    // Start is called before the first frame update
    void Start()
    {
        lifeLoot.SetActive(false);

        refAvatar = GameObject.FindGameObjectWithTag("Player").transform;

        Fill = 1f;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = baseSpeed;
        //rb = GetComponent<Rigidbody>();
        
        //refAvatar = CharacterMouvement.instance.transform;
        if(waypoints.Length >=1)
        {
            agent.SetDestination(RandomNavmeshLocation(4f));
        }
        //trouver un point au hasard sur le NavMesh à 4mètres

        _currentHealth = characterStats.StartHealth;
        _currentMaxHealth = characterStats.maxHealth;
        coll = GetComponent<Collider>();

        if(refAvatar)
        {
            HiotaController = refAvatar.GetComponent<Controller_FSM>();
        }
    }

    private void OnEnable()
    {
        OnDeathEnemy += DeclareIsDead;
        damageHiota.OnBeginAttack += SetCanMoveFalse;
        damageHiota.OnFinishAttack += SetCanMoveTrue;
    }

    private void OnDisable()
    {
        OnDeathEnemy -= DeclareIsDead;
        damageHiota.OnBeginAttack -= SetCanMoveFalse;
        damageHiota.OnFinishAttack -= SetCanMoveTrue;
    }

    // Update is called once per frame
    void Update()
    {
        dirAvatar = refAvatar.position - transform.position;
        //Debug.Log(dirAvatar.magnitude);

        inRangeOfAttack = false;

        enemyAnimator.SetFloat("prepAttackSpeed", preparationAttackSpeed);
        enemyAnimator.SetFloat("attackSpeed", AttackSpeed);

        //if(Vector3.Distance(refAvatar.position, transform.position))
        if (dirAvatar.magnitude > detectionDistance || canDetect == false) // Patrouille
		{
            //Roam
            if((agent.destination - transform.position).magnitude < 2f)
			{
                if(b_CanMove)
                {
                    if ( waypoints.Length >= 1 )
                    {
                        agent.SetDestination(RandomNavmeshLocation(4f));
                    }
                }
                EnemySpotPlayer = false;
            }
		}
        else if (dirAvatar.magnitude > attackDistance) 
		{
            //Chasse
            if(b_CanMove)
            {
                agent.SetDestination(refAvatar.position);
            }
            //agent.destination = refAvatar.position;
            if (!EnemySpotPlayer)
            {
                Instantiate(AggroTaken, transform.position, Quaternion.identity);
                EnemySpotPlayer = true;
            }
            
        }
        else
		{
            //Attack
            if(b_CanMove)
            {

                agent.SetDestination(transform.position);
            }
            inRangeOfAttack = true;
            /*timerTir += Time.deltaTime;

            if(timerTir > cadenceTir)
			{
                timerTir = 0;

                //Script d'attaque

               // clone = Instantiate(projectile, transform.position, Quaternion.identity);
                //clone.GetComponent<Projectile>().target = refAvatar;
			}*/
        }
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 finalPosition = waypoints[Mathf.RoundToInt(Random.value * (waypoints.Length - 1))].position;
        return finalPosition;

    }

    private float CalculateFinalDamages(float damages, float Armor)
    {
        float OutputDamage = Mathf.Clamp(damages - Armor, 0, damages);
        //Debug.Log(OutputDamage, this);
        return OutputDamage;
    }

    private void LoseHP(float damageTaken)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= damageTaken;
            
        }
    }

    public void TakeDamages(float damageTaken, Transform striker)
    {
        float damageOuput = CalculateFinalDamages(damageTaken, characterStats.baseArmor);
        LoseHP(damageOuput);
        Fill = _currentHealth/_currentMaxHealth;
        Fill = Mathf.Clamp(Fill, 0, _currentMaxHealth);
        Bar.fillAmount = Fill;

        


        //RaycastHit hit;

        Vector3 ClosestPointToStriker = coll.ClosestPointOnBounds(striker.position);
        if (HitFXprefab != null)
        {
            Instantiate(HitFXprefab, ClosestPointToStriker, Quaternion.identity);

        }


        //Debug.Log("il ne me reste plus que " + _currentHealth + " d'HP", this);

        if (_currentHealth <= 0)
        {
            lifeLoot.SetActive(true);
            //STOP FUCKING DESTROY A GAME OBJECT WITHOUT A SAFE GUARD
            //Destroy(gameObject);
            // Instantiate Object for Hiota ++Health


            Instantiate(lifeLoot, transform.position, transform.rotation);
            OnDeathEnemy();
            if (HiotaController)
            {
                //HiotaController.OnDeathEnemy();
            }
            gameObject.SetActive(false);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1, 0), detectionDistance);
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackDistance);
    }

    public void SetCanDetect(bool value)
	{
        canDetect = value;

	}

    public void ReturnToZone()
	{
        agent.SetDestination(transform.position + (zoneEnemy.position - transform.position).normalized * 6);
	}

    private void DeclareIsDead()
    {
        if(!b_IsDead)
        {
            b_IsDead = true;
            //Debug.Log("b_IsDead is " + b_IsDead);
        }
        else
        {
            //Debug.Log("I'm already Dead !!!", this);
        }
    }

    private void SetCanMoveTrue()
    {
        b_CanMove = true;
    }

    private void SetCanMoveFalse()
    {
        b_CanMove = false;
    }


}
