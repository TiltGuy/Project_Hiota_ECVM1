using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class SpawnTroup : MonoBehaviour
{

    public static SpawnTroup instance;

    [System.Serializable]
    public struct SpawnPoint
    {
        public Transform transform;
        public float radiusOfSpawn;
    }

    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

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
        var troupIndex = RoomObject.debugPlay ? Mathf.Min(troups.Length - 1, RoomObject.debugArenaIndex)  : LevelManager.nextTroopIndex;
        Debug.Log("troupIndex=" + troupIndex + " => " + troups[troupIndex]);
        foreach ( GameObject enemy in troups[troupIndex].Enemies )
        {
            Vector3 randomPositionInCircle;
            GameObject currentEnemy;
            if (spawnPoints.Count>0)
            {
                SpawnPoint currentPoint = spawnPoints.OrderBy(s => Random.Range(0f, 1f)).First();
                randomPositionInCircle = GetARandomPointInCircle(currentPoint.radiusOfSpawn);
                currentEnemy = Instantiate(enemy, 
                    currentPoint.transform.position + randomPositionInCircle, 
                    currentPoint.transform.rotation);

            }
            else
            {
                randomPositionInCircle = GetARandomPointInCircle(distToSpawn);
                currentEnemy = Instantiate(enemy, 
                    transform.position + randomPositionInCircle, 
                    transform.rotation);

            }
            currentEnemy.transform.parent = transform;
            currentEnemy.transform.parent = null;
            EnemyHolder currentHolder;
            currentHolder.controllerFSM = currentEnemy.GetComponent<Controller_FSM>();
            currentHolder.characterSpecs = currentEnemy.GetComponent<CharacterSpecs>();

        }
    }

    private Vector3 GetARandomPointInCircle(float distance)
    {
        Vector2 randomPointInCircle = Random.insideUnitCircle;
        Vector3 randomPosition = new Vector3(
            randomPointInCircle.x * distance,
            0,
            randomPointInCircle.y * distance);
        
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
        if (spawnPoints.Count == 0)
        {
            Gizmos.DrawWireSphere(transform.position, distToSpawn);
        }
        foreach(SpawnPoint point in spawnPoints)
        {
            if(point.transform != null)
            {
                Debug.DrawLine(transform.position, point.transform.position);
                Gizmos.DrawWireSphere(point.transform.position, point.radiusOfSpawn);
            }
        }

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
