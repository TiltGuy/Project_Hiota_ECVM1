using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    private NavMeshAgent agent;
    public Transform player;
    private Rigidbody rb;

    //ParticleSystems
    public Transform AggroTaken;

    //Agro
    [SerializeField] private float targetDistance;
    private float playerDistance;
    private bool canAgro;
    private bool isAgro = false;
    [SerializeField] private Transform target;
    private Vector3 ZombiSpeed;
    [SerializeField] private float Cooldown;



    //Patrol
    [SerializeField] private Transform[] interestPoints;
    private int index;
    [SerializeField] private bool canStroll;
    private bool isStrolling = true;
    private Vector3 lastPos;
    private bool targetAquired = false;


    //Gizmos
    [SerializeField] private float agroDistance;
    [SerializeField] private float stopDistance;
    [SerializeField] private float attackDistance;

    #region -- NEW CODE --

    //Health
    public CharacterStats_SO characterStats;
    public float currentHealth;
    public GameObject HitFXprefab;
    private Collider coll;

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


    //Health Bar
    public Image Bar;
    public float Fill;


    //Attack
    public bool canAttack;
    //public float attackDamage = 1f;
    
    

    //Loot
    public GameObject lifeLoot;


    //Animations
    public Animator enemyAnimator;

    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        
        Fill = 1f;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        enemyAnimator = GetComponentInChildren<Animator>();

        if (canStroll)
        {
            RandomTarget();
            targetAquired = true;
        }


        lastPos = this.transform.position;

        currentHealth = characterStats.baseHealth;
        coll = GetComponent<Collider>();
        Debug.Log(coll, this);

        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            playerDistance = Vector3.Distance(transform.position, player.transform.position);

            if (canStroll || isAgro)
            {
                targetDistance = Vector3.Distance(transform.position, target.position);
            }

            if (canAgro && isAgro == false /*&& targetDistance <= playerDistance*/)
            {
                target = player.transform;
                isAgro = true;
                isStrolling = false;
                targetAquired = true;

            }
            else if (canStroll && isStrolling == false && isAgro == false && targetAquired == false)
            {
                Invoke("RandomTarget",5);
                targetAquired = true;
            }

            if (isAgro)
            {
                agent.destination = target.position;
                Instantiate(AggroTaken, transform.position, Quaternion.identity);
            }

            if (playerDistance <= agroDistance)
            {
                canAgro = true;
                canAttack = false;
            }
            else if (playerDistance > agroDistance && isAgro)
			{
                Invoke("RandomTarget", Cooldown);
                new WaitForSeconds(Cooldown);
                isAgro = false;
                canAgro = false;
                targetAquired = false;

            }

			if (targetDistance <= stopDistance && targetDistance <= playerDistance)
            {
                if (isAgro)
                {
                    //agent.isStopped = true;
                    
                    //rb.velocity = Vector3.zero;
                    //lance une attaque
                    canAttack = true;
                    enemyAnimator.SetBool("canAttack", true);
                    //print("agrougrou!");
                }
                else if (isStrolling)
                {
                    isStrolling = false;
                    targetAquired = false;
                }

            }
			else
			{
                canAttack = false;
                enemyAnimator.SetBool("canAttack", false);
                isStrolling = true;
			}

        }

        if (lastPos != transform.position)               //
        {                                               //
            ZombiSpeed = transform.position - lastPos;  // Check si le zombie
            ZombiSpeed /= Time.deltaTime;               // avance pour activer
            lastPos = transform.position;               // l'animation de marche.
        }                                               //
                                                        //
                                                        //anim.SetFloat("SpeedZ", ZombiSpeed.magnitude);  //

        
    }

    private void FixedUpdate()
    {

    }

    void OnDrawGizmosSelected()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1, 0), agroDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1, 0), stopDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1, 0), attackDistance);
    }

    void RandomTarget()
    {
        index = (Random.Range(0, interestPoints.Length));
        target = interestPoints[index];
        agent.destination = target.position;
        isStrolling = true;
    }

    private float CalculateFinalDamages(float damages, float Armor)
    {
        float OutputDamage = Mathf.Clamp(damages - Armor, 0, damages);
        Debug.Log(OutputDamage, this);
        return OutputDamage;
    }

    private void LoseHP(float damageTaken)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damageTaken;
            
        }
    }

    public void TakeDamages(float damageTaken, Transform striker)
    {
        float damageOuput = CalculateFinalDamages(damageTaken, characterStats.baseArmor);
        LoseHP(damageTaken);
        Fill -= damageTaken;
        Bar.fillAmount = Fill;
        
        
        
        
        //RaycastHit hit;

        Vector3 ClosestPointToStriker = coll.ClosestPointOnBounds(striker.position);
        if (HitFXprefab != null)
        {
            Instantiate(HitFXprefab, ClosestPointToStriker, Quaternion.identity);
            
        }

        //if (Physics.Raycast(transform.position, (striker.transform.position - transform.position), out hit))
        //{
        //    Instantiate(HitFXprefab, hit.point, Quaternion.identity);
        //}
        //LoseHP(damageTaken, currentHealth);
        //Debug.Log("ARGH!!! j'ai pris : " + CalculateFinalDamages(damages, characterStats.baseArmor) + " points de Dommages", this);
        Debug.Log("il ne me reste plus que " + currentHealth + " d'HP", this);

        if(Fill <= 0)
		{
            Destroy(gameObject);

            
            // Instantiate Object for Hiota ++Health
            Instantiate(lifeLoot, transform.position, transform.rotation);
		}
    }

    
    
}
