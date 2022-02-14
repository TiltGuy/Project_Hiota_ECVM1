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
    public bool canAttack = false;
    [SerializeField]
    private Transform zoneEnemy;
    [SerializeField]
    private Transform[] waypoints;

    //Health
    [SerializeField]
    private CharacterStats_SO characterStats;
    private float _currentHealth;
    private float _currentMaxHealth;
    [SerializeField]
    private GameObject HitFXprefab;
    private Collider coll;

    //Health Bar
    public Image Bar;
    public float Fill;

    //Loot
    public GameObject lifeLoot;


    // Start is called before the first frame update
    void Start()
    {
        lifeLoot.SetActive(false);

        Fill = 1f;
        agent = GetComponent<NavMeshAgent>();
        //rb = GetComponent<Rigidbody>();
        
        //refAvatar = CharacterMouvement.instance.transform;

        agent.SetDestination(RandomNavmeshLocation(4f));
        //trouver un point au hasard sur le NavMesh à 4mètres

        _currentHealth = characterStats.baseHealth;
        _currentMaxHealth = characterStats.maxHealth;
        coll = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        dirAvatar = refAvatar.position - transform.position;
        //Debug.Log(dirAvatar.magnitude);

        canAttack = false;

        //if(Vector3.Distance(refAvatar.position, transform.position))
        if (dirAvatar.magnitude > detectionDistance || canDetect == false) // Patrouille
		{
            //Roam
            if((agent.destination - transform.position).magnitude < 2f)
			{
                agent.SetDestination(RandomNavmeshLocation(4f));
                EnemySpotPlayer = false;
            }
		}
        else if (dirAvatar.magnitude > attackDistance) 
		{
            //Chasse
            agent.SetDestination(refAvatar.position);
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
            agent.SetDestination(transform.position);
            canAttack = true;
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
        /*Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }*/
        Vector3 finalPosition = waypoints[Mathf.RoundToInt(Random.value * (waypoints.Length - 1))].position;
        return finalPosition;

    }

    private float CalculateFinalDamages(float damages, float Armor)
    {
        float OutputDamage = Mathf.Clamp(damages - Armor, 0, damages);
        Debug.Log(OutputDamage, this);
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


        Debug.Log("il ne me reste plus que " + _currentHealth + " d'HP", this);

        if (_currentHealth <= 0)
        {
            lifeLoot.SetActive(true);
            //STOP FUCKING DESTROY A GAME OBJECT WITHOUT A SAFE GUARD
            //Destroy(gameObject);
            // Instantiate Object for Hiota ++Health


            Instantiate(lifeLoot, transform.position, transform.rotation);
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
}
