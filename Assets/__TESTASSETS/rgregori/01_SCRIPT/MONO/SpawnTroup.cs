using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTroup : MonoBehaviour
{

    public Troup_SO[] Troups;
    public float distToSpawn;

    public LevelManager levelManager;

    [Header("--- DIFFICULTY SETTINGS ---")]

    [SerializeField]
    private float arrayFraction = .33f;

    // Start is called before the first frame update
    void Start()
    {
        if(Troups != null)
        {
            SortTroupsByDifficulty();
            int min = Mathf.FloorToInt(0 + LevelManager.currentRoomIndex/levelManager.NbTotalRooms * (Troups.Length - 1));
            float maxBase = (Troups.Length - 1) * arrayFraction;
            Debug.Log("min = " + min);
            Debug.Log("maxBase = " + maxBase);
            min = Mathf.Clamp(min, 0, Mathf.RoundToInt(Troups.Length - 1 -maxBase));
            int max = Mathf.RoundToInt( maxBase + min);
            //max = Mathf.Clamp(max, 2, Troups.Length - 1);
            int troopIndex = Random.Range(min, max);
            Debug.Log("troupIndex = " + troopIndex);
            foreach ( GameObject enemy in Troups[troopIndex].Enemies )
            {
                Vector2 randomPointInCircle = Random.insideUnitCircle;
                Vector3 randomPosition = new Vector3(randomPointInCircle.x * distToSpawn, 0, randomPointInCircle.y * distToSpawn);
                Instantiate(enemy, transform.position + randomPosition, transform.rotation);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distToSpawn);
    }

    private void SortTroupsByDifficulty()
    {
        var itemMoved = false;
        do
        {
            itemMoved = false;
            for ( int i = 0; i < Troups.Length - 1; i++ )
            {
                if ( Troups[i].DifficultyLevel() > Troups[i + 1].DifficultyLevel() )
                {
                    var lowerValue = Troups[i + 1];
                    Troups[i + 1] = Troups[i];
                    Troups[i] = lowerValue;
                    itemMoved = true;
                }
            }
        } while ( itemMoved );
    }
}
