using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class StaticEnemyAI : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform player;
    private Rigidbody rb;

    [SerializeField] private float attackDistance;

    //Health
    public CharacterStats_SO characterStats;
    public float currentHealth;
    public GameObject HitFXprefab;
    private Collider coll;

    //Health Bar
    public Image Bar;
    public float Fill;

    //Agro
    private float playerDistance;

    //Attack
    public bool canAttack;
    //public float attackDamage = 1f;



    //Loot
    public GameObject lifeLoot;


    //Animations
    public Animator enemyAnimator;


    // Start is called before the first frame update
    void Start()
    {

        lifeLoot.SetActive(false);

        Fill = 1f;
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        enemyAnimator = GetComponentInChildren<Animator>();

        currentHealth = characterStats.StartHealth;
        coll = GetComponent<Collider>();
        //Debug.Log(coll, this);
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
		{
            playerDistance = Vector3.Distance(transform.position, player.transform.position);

            if (attackDistance >= playerDistance)
            {
                canAttack = true;
            }
			else
			{
                canAttack = false;
			}
        }
        
    }

    void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, 1, 0), attackDistance);
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

        
        Debug.Log("il ne me reste plus que " + currentHealth + " d'HP", this);

        if (Fill <= 0)
        {
            lifeLoot.SetActive(true);
            Destroy(gameObject);
            // Instantiate Object for Hiota ++Health
            
            Instantiate(lifeLoot, transform.position, transform.rotation);
        }
    }

    public void TakeDamages( float damageTaken, Transform Striker, bool isAHook )
    {
        throw new System.NotImplementedException();
    }
}
