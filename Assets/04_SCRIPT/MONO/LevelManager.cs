﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{

    public static float currentRoomIndex;

    public static LevelManager instance;

    public float palierRoomIndex = 10;
    public string palierSceneName;

    public string[] sceneNames;

    [SerializeField]
    private GameObject[] Props;

    //[SerializeField]
    //private GameObject[] Troups;
    [SerializeField]
    private float nbTotalRooms;

    //[SerializeField]
    //private float minProbability = 25;
    //[SerializeField]
    //private float maxProbability = 75;

    public float NbTotalRooms
    {
        get => nbTotalRooms;
    }

    private void Awake()
    {
        instance = this;
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
}