using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour, IDamageable
{
    private NavMeshAgent agent;
    public Transform player;


    //Agro
    [SerializeField] private float targetDistance;
    private float playerDistance;
    private bool canAgro;
    private bool isAgro = false;
    private Transform target;
    private Vector3 ZombiSpeed;



    //Patrol
    [SerializeField] private Transform[] interestPoints;
    private int index;
    [SerializeField] private bool canStroll;
    private bool isStrolling = true;
    private Vector3 lastPos;


    //Gizmos
    [SerializeField] private float agroDistance;
    [SerializeField] private float stopDistance;


    //Health
    public CharacterStats_SO characterStats;
    public float currentHealth;
    public GameObject HitFXprefab;
    private Collider coll;


    //Health Bar
    public Image Bar;
    public float Fill;


    //Attack
    public int attackDamage = 1;

    // Start is called before the first frame update
    void Awake()
    {
        
        Fill = 1f;
        agent = GetComponent<NavMeshAgent>();
        //anim = GetComponent<Animator>();
        if (canStroll)
        {
            RandomTarget();
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

            if (canAgro && isAgro == false)
            {
                target = player.transform;
                isAgro = true;
                isStrolling = false;

            }
            else if (canStroll && isStrolling == false && isAgro == false)
            {
                RandomTarget();
            }

            if (isAgro)
            {
                agent.destination = target.position;
            }

            if (playerDistance <= agroDistance)
            {
                canAgro = true;
            }

            if (targetDistance <= stopDistance)
            {
                if (isAgro)
                {
                    print("agrougrou!");
                }
                else if (isStrolling)
                {
                    isStrolling = false;
                }

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
        Fill -= 0.2f;
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
		}
    }

    void OnTriggerEnter(Collider other)
	{
        HiotaHealth player = other.GetComponent<HiotaHealth>();
        if(player != null)
		{
            player.Hurt(attackDamage);
		}
        
	}
}
