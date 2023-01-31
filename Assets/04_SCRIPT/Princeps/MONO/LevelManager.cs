using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Unity.AI.Navigation;

public class LevelManager : MonoBehaviour
{

    public static float currentRoomIndex;

    public bool b_IsPlayerReady = false;

    public static LevelManager instance;

    public NavMeshSurface[] surfaces;

    public float palierRoomIndex = 10;
    public ScenesBuildIndex PalierRoomSceneBuildIndex;

    public ScenesBuildIndex StandardRoomScenesBuildIndex;

    public string NameRoomHub;

    [SerializeField]
    private GameObject[] Props;

    [SerializeField]
    private Troup_SO[] troups;

    //[SerializeField]
    //private GameObject[] Troups;
    [SerializeField]
    private float nbTotalRoomPalier;

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
        get => nbTotalRoomPalier;
    }
    public Troup_SO[] Troups
    {
        get => troups;
    }

    private void Awake()
    {

        if ( LevelManager.instance != null )
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this);

        if(!CheckSurfacesNullGameObject(surfaces))
        {
            Debug.LogError("Pas de platforme ou de surfaces ! ",this);
            //surfaces = platform.GetComponents<NavMeshSurface>();
        }

        if ( nextTroopIndex == -1 )
        {
            DefineNextTroopIndex();
            //Debug.Log("ALED!!");
        }

        int nbToBeSpawned = Mathf.FloorToInt(1 + currentRoomIndex / nbTotalRoomPalier * (Props.Length - 1));

        SpawnAllTheProps(nbToBeSpawned);

        InitializeNavMeshData();
    }

    private void Start()
    {
        if(currentRoomIndex < 0 )
        {
            currentRoomIndex = 0;
        }
        //foreach (GameObject prop in Props)
        //{
        //    if (Random.Range(0,100) > minProbability + currentRoomIndex/nbTotalRooms * (maxProbability - minProbability) )
        //    {
        //        prop.SetActive(true);
        //    }
        //}



        
        //Debug.Log("nbToBeSpawned = " + nbToBeSpawned);

        

    }

    private void InitializeNavMeshData()
    {
        ClearAllNavMeshData();

        BuildNavMeshForAllSurfaces();
    }

    private bool CheckSurfacesNullGameObject(NavMeshSurface[] array)
    {
        foreach(var obj in array)
        {
            if(obj == null)
                return false;
        }

        return true;
    }

    private void SpawnAllTheProps( int nbToBeSpawned )
    {
        if ( Props.Length != 0 )
        {
            for ( int i = 0; i < nbToBeSpawned; i++ )
            {
                SpawnProp();
            }
        }
    }

    private void SpawnProp()
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
                Debug.LogError("Boucle fini sale vilain !!! ", this);
                break;
            }
        }
        Props[randomIndex].SetActive(true);
    }

    private void ClearAllNavMeshData()
    {

        foreach ( var surface in surfaces )
        {
            surface.RemoveData();
        }
    }

    private void BuildNavMeshForAllSurfaces()
    {
        foreach( var surface in surfaces )
        {
            surface.BuildNavMesh();
        }
    }

    [ContextMenu("ForceLoadNextLevel")]

    public void LoadNextLevel()
    {
        float tempNextRoomIndex = currentRoomIndex;
        tempNextRoomIndex++;
        if( tempNextRoomIndex == palierRoomIndex)
        {
            StartCoroutine(PreLoadNextRandomRoom(PalierRoomSceneBuildIndex));
            Debug.Log("Next Palier Room");
        }
        else
        {
            StartCoroutine(PreLoadNextRandomRoom(StandardRoomScenesBuildIndex));
            Debug.Log("Next Standard Room");
        }

        currentRoomIndex = tempNextRoomIndex;
        currentRoomIndex = Mathf.Clamp(currentRoomIndex, 0, nbTotalRoomPalier);
        Debug.Log("currentroomIndex = " + currentRoomIndex);
        //Debug.Log("nbTotalRooms = " + nbTotalRoomPalier);
    }

    private IEnumerator PreLoadNextRandomRoom(ScenesBuildIndex scenesNames)
    {
        yield return null;
        int nextPalierRoomBuildIndex = scenesNames.ListOfScenesBuildIndex[Random.Range(0, PalierRoomSceneBuildIndex.ListOfScenesBuildIndex.Length)];
        Debug.Log("next palier = " + nextPalierRoomBuildIndex);
        GameManager.instance.GoToNextLVL(nextPalierRoomBuildIndex);
    }

    public int LoadNextRandomRoom( ScenesBuildIndex scenesNames )
    {
        int nextPalierRoomBuildIndex = scenesNames.ListOfScenesBuildIndex[Random.Range(0, PalierRoomSceneBuildIndex.ListOfScenesBuildIndex.Length)];
        Debug.Log("next palier = " + nextPalierRoomBuildIndex);
        return nextPalierRoomBuildIndex;
        //GameManager.instance.GoToNextLVL(nextPalierRoomBuildIndex);
    }

    public int DefineNextFightArena()
    {
        float tempNextRoomIndex = currentRoomIndex;
        tempNextRoomIndex++;
        int nextRoomBuildIndex = 0;
        if ( tempNextRoomIndex == palierRoomIndex )
        {
            nextRoomBuildIndex = LoadNextRandomRoom(PalierRoomSceneBuildIndex);
            Debug.Log("Next Palier Room");
        }
        else
        {
            nextRoomBuildIndex = LoadNextRandomRoom(StandardRoomScenesBuildIndex);
            Debug.Log("Next Standard Room");
        }

        currentRoomIndex = tempNextRoomIndex;
        currentRoomIndex = Mathf.Clamp(currentRoomIndex, 0, nbTotalRoomPalier);
        //Debug.Log("currentroomIndex = " + currentRoomIndex);
        return nextRoomBuildIndex;
        //Debug.Log("nbTotalRooms = " + nbTotalRoomPalier);
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
        nextTroopIndex = Random.Range(min, max);
    }

    public void LoadHub()
    {
        currentRoomIndex = 0;
        //SaveSystem.SavePlayer(GetComponent<CharacterSpecs>(), DeckManager.instance);
        SceneManager.LoadScene(NameRoomHub);
        Debug.Log("reload Hub");
    }
}
