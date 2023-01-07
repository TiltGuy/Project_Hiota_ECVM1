using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class LevelManager : MonoBehaviour
{

    public static float currentRoomIndex;

    public static LevelManager instance;

    public GameObject platform;
    public NavMeshSurface[] surfaces;

    public float palierRoomIndex = 10;
    public string palierSceneName;

    public string[] sceneNames;

    [SerializeField]
    private GameObject[] Props;

    [SerializeField]
    private Troup_SO[] troups;

    //[SerializeField]
    //private GameObject[] Troups;
    [SerializeField]
    private float nbTotalRooms;

    public static int nextTroopIndex = -1;

    [Header("--- DIFFICULTY SETTINGS ---")]

    [SerializeField]
    private float arrayFraction = .33f;

    //[SerializeField]
    //private float minProbability = 25;
    //[SerializeField]
    //private float maxProbability = 75;

    public float NbTotalRooms
    {
        get => nbTotalRooms;
    }
    public Troup_SO[] Troups
    {
        get => troups;
    }

    private void Awake()
    {
        instance = this;
        surfaces = platform.GetComponents<NavMeshSurface>();

        if ( nextTroopIndex == -1 )
        {
            DefineNextTroopIndex();
            Debug.Log("ALED!!");
        }
    }

    private void Start()
    {
        //foreach (GameObject prop in Props)
        //{
        //    if (Random.Range(0,100) > minProbability + currentRoomIndex/nbTotalRooms * (maxProbability - minProbability) )
        //    {
        //        prop.SetActive(true);
        //    }
        //}

        

        int nbToBeSpawned = Mathf.FloorToInt(1 + currentRoomIndex / nbTotalRooms * (Props.Length - 1));
        //Debug.Log("nbToBeSpawned = " + nbToBeSpawned);

        if(Props.Length != 0)
        {
            for ( int i = 0; i < nbToBeSpawned; i++ )
            {
                int security = 0;
                int randomIndex = Random.Range(0, Props.Length);
                //Debug.Log("1er Random Index = " + randomIndex);
                while ( Props[randomIndex].activeInHierarchy )
                {
                    randomIndex = Random.Range(0, Props.Length);
                    //Debug.Log("2ème Random Index = " + randomIndex);
                    security++;
                    if ( security > 1000 )
                    {
                        //Debug.LogError("Boucle fini sale vilain !!! ", this);
                        break;
                    }
                }
                Props[randomIndex].SetActive(true);
            }
        }

        foreach (NavMeshSurface surface in surfaces)
        {
        }

        for (int i = 0; i < surfaces.Length; i++)
        {
            surfaces[i].BuildNavMesh();
        }

    }

    public void LoadNextLevel()
    {
        currentRoomIndex ++;
        currentRoomIndex = Mathf.Clamp(currentRoomIndex, 0, nbTotalRooms);
        Debug.Log("currentroomIndex = " + currentRoomIndex);
        Debug.Log("nbTotalRooms = " + nbTotalRooms);
        if(currentRoomIndex == palierRoomIndex)
        {
            SceneManager.LoadScene(palierSceneName);
        }
        else
        {
            SceneManager.LoadScene(sceneNames[Random.Range(0, sceneNames.Length)]);
        }
    }


    public void DefineNextTroopIndex()
    {
        Troup_SO[] troups = LevelManager.instance.Troups;
        int min = Mathf.FloorToInt(0 + currentRoomIndex / NbTotalRooms * (troups.Length - 1));
        float maxBase = (troups.Length - 1) * arrayFraction;
        //Debug.Log("min = " + min);
        //Debug.Log("maxBase = " + maxBase);
        min = Mathf.Clamp(min, 0, Mathf.RoundToInt(troups.Length - 1 - maxBase));
        int max = Mathf.RoundToInt(maxBase + min);
        //max = Mathf.Clamp(max, 2, Troups.Length - 1);
        nextTroopIndex = Random.Range(min, max);
        Debug.Log(troups[nextTroopIndex].Enemies.ToString());
        //Debug.Log("troupIndex = " + troopIndex);
    }
}
