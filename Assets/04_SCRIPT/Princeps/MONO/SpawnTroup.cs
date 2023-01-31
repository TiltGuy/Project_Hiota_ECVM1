using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnTroup : MonoBehaviour
{

    public static SpawnTroup instance;


    public float distToSpawn;

    public struct EnemyHolder
    {
        public CharacterSpecs characterSpecs;
        public Controller_FSM controllerFSM;
    }

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        Troup_SO[] troups = LevelManager.instance.Troups;
        if ( troups != null)
        {

            SortTroupsByDifficulty(troups);
            //if ( nextTroopIndex == -1 )
            //{
            //    DefineNextTroopIndex();
            //    Debug.Log("ALED!!");
            //}
            InstantiateEnemiesInsideCircle(troups);
        }
    }

    private void InstantiateEnemiesInsideCircle( Troup_SO[] troups )
    {
        foreach ( GameObject enemy in troups[LevelManager.nextTroopIndex ].Enemies )
        {
            Vector3 randomPosition = GetARandomPointInCircle();
            GameObject currentEnemy = Instantiate(enemy, transform.position + randomPosition, transform.rotation);
            
            currentEnemy.transform.parent = transform;
            currentEnemy.transform.parent = null;
            EnemyHolder currentHolder;
            currentHolder.controllerFSM = currentEnemy.GetComponent<Controller_FSM>();
            currentHolder.characterSpecs = currentEnemy.GetComponent<CharacterSpecs>();

        }
    }

    private Vector3 GetARandomPointInCircle()
    {
        Vector2 randomPointInCircle = Random.insideUnitCircle;
        Vector3 randomPosition = new Vector3(
            randomPointInCircle.x * distToSpawn,
            0,
            randomPointInCircle.y * distToSpawn);
        
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position + randomPosition, out hit, 5f, NavMesh.AllAreas) )
        {
            if (hit.mask == NavMesh.GetAreaFromName("Trap") )
            {

                print(hit.mask);
            }
        }

        return randomPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distToSpawn);

    }

    private void SortTroupsByDifficulty(Troup_SO[] troups)
    {
        var itemMoved = false;
        do
        {
            itemMoved = false;
            for ( int i = 0; i < troups.Length - 1; i++ )
            {
                if ( troups[i].DifficultyLevel() > troups[i + 1].DifficultyLevel() )
                {
                    var lowerValue = troups[i + 1];
                    troups[i + 1] = troups[i];
                    troups[i] = lowerValue;
                    itemMoved = true;
                }
            }
        } while ( itemMoved );
    }
}
