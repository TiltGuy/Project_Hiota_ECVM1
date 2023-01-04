using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTroup : MonoBehaviour
{

    public Troup_SO[] Troups;
    public float distToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        if(Troups != null)
        {
            int troopIndex = Random.Range(0, Troups.Length - 1);
            foreach ( GameObject enemy in Troups[troopIndex].Enemies )
            {
                Vector3 randomPosition = new Vector3( Random.insideUnitCircle.x * distToSpawn, transform.position.y, Random.insideUnitCircle.y * distToSpawn);
                Instantiate(enemy, randomPosition, Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distToSpawn);
    }
}
