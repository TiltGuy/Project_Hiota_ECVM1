﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetGatherer : MonoBehaviour
{
    public bool b_IsPlayer;
    [SerializeField] private string TargetTag = "Enemy";
    [SerializeField]
    private Controller_FSM controller;
    [SerializeField]
    private LayerMask layerMask;
    [Header("-- LISTS OF ENEMIES --")]
    public List<Transform> PotentialEnemies;
    public List<Transform> TargetableEnemies;
    [SerializeField]
    List<Transform> SortedListOfEnemies;
    private Transform mainCameraTransform;
    private Camera mainCamera;
    private Plane[] planes;
    private Collider objToVerify;
    private Transform currentTarget;
    [HideInInspector]
    public float dirNum;

    #region DELEGATE INSTANCIATION

    public delegate void MultiDelegate(Transform transform);
    public MultiDelegate AddEnemyToList;
    public MultiDelegate RemoveEnemyToList;

    #endregion

    private void Awake()
    {
        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.transform;
    }

    private void Start()
    {
        if(!mainCameraTransform)
        {
            Debug.LogError("There isn't Camera in Main Camera", this);
        }
        else
        {
            planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);
        }

        if(controller.currentCharacterTarget)
        {
            currentTarget = controller.currentCharacterTarget;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(TargetTag) )
        {
            AddPotentialAliveEnemies(other);
        }
    }

    private void AddPotentialAliveEnemies( Collider other )
    {
        bool isDead = other.GetComponent<Controller_FSM>().B_IsDead;
        if ( !PotentialEnemies.Contains(other.transform) && isDead == false )
        {
            PotentialEnemies.Add(other.transform);
            AddEnemyToList?.Invoke(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TargetTag) )
        {
            RemoveEnemyFromTargetableNdPotential(other);
        }

        if(other.CompareTag("Player"))
        {
            //Debug.Log("Trying to remove Hiota", this);
        }
    }

    private void RemoveEnemyFromTargetableNdPotential( Collider other )
    {
        bool isDead = other.GetComponent<Controller_FSM>().B_IsDead;
        if ( PotentialEnemies.Contains(other.transform)
            || (PotentialEnemies.Contains(other.transform)
            && (!other.gameObject.activeInHierarchy) || isDead == true) )
        {
            RemoveEnemyToList?.Invoke(other.transform);
            TargetableEnemies.Remove(other.transform);
            PotentialEnemies.Remove(other.transform);
        }
    }

    private void Update()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        Vector3 heading = mainCameraTransform.forward;

        if( !b_IsPlayer )
        {
            return;
        }

        if(currentTarget != null)
        {
            heading = currentTarget.position - mainCameraTransform.position;
        }
        dirNum = AngleDir(mainCameraTransform.forward, heading, mainCameraTransform.up);
        

        if (PotentialEnemies.Count > 0)
        {
            foreach (Transform enemy in PotentialEnemies)
            {
                Vector3 dir = (enemy.position - transform.position);
                //Debug.Log("check : " + CheckSightLine(enemies.transform));
                objToVerify = enemy.GetComponent<Collider>();

                if(GeometryUtility.TestPlanesAABB(planes, objToVerify.bounds))
                {
                    //Debug.Log("enemy : " + enemy + " has been detected");
                    if (CheckSightLine(enemy.transform))
                    {

                        // if True then add to another List of Targetable Enemies
                        if (!TargetableEnemies.Contains(enemy.transform) && enemy.gameObject.activeInHierarchy)
                        {
                            TargetableEnemies.Add(enemy.transform);
                            DisplayEnemiesHealthBar(enemy.transform,true);
                        }
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, dir, Color.red);
                        if (TargetableEnemies.Contains(enemy.transform) && (!enemy.gameObject.activeInHierarchy 
                            || enemy.GetComponent<Controller_FSM>().B_IsDead == true))
                        {
                            DisplayEnemiesHealthBar(enemy.transform, false);
                            TargetableEnemies.Remove(enemy.transform);

                        }
                        // if Not then remove this element of the List of Targetable Enemies
                    }
                }
                else
                {
                    if (TargetableEnemies.Contains(objToVerify.transform))
                    {
                        DisplayEnemiesHealthBar(objToVerify.transform, false);
                        TargetableEnemies.Remove(objToVerify.transform);

                    }
                    //Debug.Log("the enemy : " + enemy.transform + " hasn't been detected", enemy.transform);
                }

                
            }
        }

    }

    private void DisplayEnemiesHealthBar(Transform enemy, bool b_ToDisplayBars)
    {
        if(b_IsPlayer)
        {
            if(enemy.GetComponent<IABrain>())
            {
                IABrain currentbrain = enemy.GetComponent<IABrain>();
                currentbrain.DisplayHealthBar(b_ToDisplayBars, false);
                //Debug.Log("Displayong HealthBars = " + enemy.name, transform);
            }
        }
    }

    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
        //print(dir + " dirnum valeur");

        return dir;
    }

    private bool CheckSightLine(Transform target)
    {
        RaycastHit hit;
        Vector3 dir = (target.position - mainCameraTransform.position);
        if (Physics.Raycast(mainCameraTransform.position, dir, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Debug.DrawRay(mainCameraTransform.position, dir, Color.yellow);
                return true;
            }
            else
                return false;
        }
        else
        {
            return false;
        }

    }

    private bool CheckObjectToTheRight(Vector3 Origin, Vector3 target)
    {
        float scalarFactor = Vector3.Dot(Origin.normalized, target.normalized);
        
        // If Left
        if (scalarFactor > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CheckObjectToTheLeft(Vector3 Origin, Vector3 target)
    {
        float scalarFactor = Vector3.Dot(Origin.normalized, target.normalized);

        // If Left
        if (scalarFactor < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public Transform CheckoutNextTargetedEnemy(Vector2 input)
    {
        //print("Vector to next Target = " + input);
        Transform currentHiotaTarget = controller.currentCharacterTarget;
        Transform objectToReturn;
        objectToReturn = currentHiotaTarget;
        
        

        

        SortedListOfEnemies = (List<Transform>)TargetableEnemies.OrderBy(target =>
        {
            Vector3 targetDirection = target.position - mainCameraTransform.position;

            Vector2 cameraForwardToPlane = new Vector2(mainCameraTransform.forward.x, mainCameraTransform.forward.z);

            Vector2 targetDirectionToPlane = new Vector2(targetDirection.x, targetDirection.z);

            float angle = Vector2.SignedAngle(cameraForwardToPlane, targetDirectionToPlane);

            return angle;
        }).ToList();

        //print(input);
        if (input.x>.1f)
        {
            if(SortedListOfEnemies.IndexOf(currentHiotaTarget) - 1 >=0 )
            {
                Transform nextObjectToTheRight = SortedListOfEnemies[(SortedListOfEnemies.IndexOf(currentHiotaTarget) - 1)];
                objectToReturn = nextObjectToTheRight;
                //Debug.Log(nextObjectToTheRight + "Object to the right", nextObjectToTheRight);
            }
            if(objectToReturn != currentHiotaTarget)
                return objectToReturn;
        }
        else if(input.x < -0.1f)
        {
            if (SortedListOfEnemies.IndexOf(currentHiotaTarget) + 1 < SortedListOfEnemies.Count)
            {
                Transform nextObjectToTheLeft = SortedListOfEnemies[(SortedListOfEnemies.IndexOf(currentHiotaTarget) + 1)];
                objectToReturn = nextObjectToTheLeft;
                //Debug.Log((SortedListOfEnemies.IndexOf(currentHiotaTarget) + 1) + "Object to the Left", nextObjectToTheLeft);
            }

            if ( objectToReturn != currentHiotaTarget )
                return objectToReturn;
        }
        return null;
    }

    public Transform CheckoutClosestEnemyToCenterCam()
    {
        List<Transform> SortedListOfEnemies = (List<Transform>)TargetableEnemies.OrderBy( target =>
        {
            Vector3 targetDirection = target.position - mainCameraTransform.position;

            Vector2 cameraForwardToPlane = new Vector2(mainCameraTransform.forward.x, mainCameraTransform.forward.z);

            Vector2 targetDirectionToPlane = new Vector2(targetDirection.x, targetDirection.z);

            float angle = Vector2.Angle(cameraForwardToPlane, targetDirectionToPlane);

            return angle;
        }).ToList();

        //Debug.Log(SortedListOfEnemies.First());

        if(SortedListOfEnemies.Count>0)
        {
            //Debug.Log("retu sorted");
            return SortedListOfEnemies.First();
        }
        else
        {
            //Debug.Log("Return targetable");
            return TargetableEnemies.First();
        }
    }

}
