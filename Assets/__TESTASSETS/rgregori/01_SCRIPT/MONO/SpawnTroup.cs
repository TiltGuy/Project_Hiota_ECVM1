using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTroup : MonoBehaviour
{

    public static SpawnTroup instance;


    public float distToSpawn;

    public LevelManager levelManager;

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
            Vector2 randomPointInCircle = Random.insideUnitCircle;
            Vector3 randomPosition = new Vector3(randomPointInCircle.x * distToSpawn, 0, randomPointInCircle.y * distToSpawn);
            GameObject currentEnemey = Instantiate(enemy, transform.position + randomPosition, transform.rotation);
            EnemyHolder currentHolder;
            currentHolder.controllerFSM = currentEnemey.GetComponent<Controller_FSM>();
            currentHolder.characterSpecs = currentEnemey.GetComponent<CharacterSpecs>();
            foreach( SkillCard_SO skillCard in DeckManager.instance._EnemiesDeck)
            {
                skillCard.ApplyEffects(currentHolder.controllerFSM, currentHolder.characterSpecs);
            }
        }
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

    //public void DefineNextTroopIndex(  )
    //{
    //    Troup_SO[] troups = LevelManager.instance.Troups;
    //    int min = Mathf.FloorToInt(0 + LevelManager.currentRoomIndex / levelManager.NbTotalRooms * (troups.Length - 1));
    //    float maxBase = (troups.Length - 1) * LevelManager.instance.arrayFraction;
    //    //Debug.Log("min = " + min);
    //    //Debug.Log("maxBase = " + maxBase);
    //    min = Mathf.Clamp(min, 0, Mathf.RoundToInt(troups.Length - 1 - maxBase));
    //    int max = Mathf.RoundToInt(maxBase + min);
    //    //max = Mathf.Clamp(max, 2, Troups.Length - 1);
    //    nextTroopIndex = Random.Range(min, max);
    //    Debug.Log(troups[nextTroopIndex].Enemies.ToString());
    //    //Debug.Log("troupIndex = " + troopIndex);
    //}
}
