using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGatherer : MonoBehaviour
{
    [SerializeField]
    private Controller_FSM controller;
    [SerializeField]
    private LayerMask layerMask;
    public List<Transform> PotentialEnemies;
    public List<Transform> TargetableEnemies;
    private Transform mainCameraTransform;
    private Camera mainCamera;
    private Plane[] planes;
    private Collider objToVerify;

    [SerializeField]
    private Transform target;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(!PotentialEnemies.Contains(other.transform))
            {
                PotentialEnemies.Add(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(PotentialEnemies.Contains(other.transform))
            {
                TargetableEnemies.Remove(other.transform);
                PotentialEnemies.Remove(other.transform);
            }
        }
    }

    private void Update()
    {
        planes = GeometryUtility.CalculateFrustumPlanes(mainCamera);

        if (PotentialEnemies.Count > 0)
        {
            foreach (Transform enemy in PotentialEnemies)
            {
                Vector3 dir = (enemy.position - transform.position);
                //Debug.Log("check : " + CheckSightLine(enemies.transform));
                objToVerify = enemy.GetComponent<Collider>();

                if(GeometryUtility.TestPlanesAABB(planes, objToVerify.bounds))
                {
                    Debug.Log("enemy : " + enemy + " has been detected");
                    if (CheckSightLine(enemy.transform))
                    {

                        // if True then add to another List of Targetable Enemies
                        if (!TargetableEnemies.Contains(enemy.transform))
                        {
                            TargetableEnemies.Add(enemy.transform);
                        }
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, dir, Color.red);
                        if (TargetableEnemies.Contains(enemy.transform))
                        {
                            TargetableEnemies.Remove(enemy.transform);

                        }
                        // if Not then remove this element of the List of Targetable Enemies
                    }
                }
                else
                {
                    Debug.Log("the enemy : " + enemy.transform + " hasn't been detected", enemy.transform);
                }

                
            }
        }


        


        


    }

    private void FixedUpdate()
    {
        
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

    private void UpdateTargetableList(List<Transform> ListToRearrange)
    {
        if (ListToRearrange.Count > 0)
        {
            // Check Distance to the target Gatherer
            // check the Scalar of the CurrentTarget with the Camera
        }
    }

    bool CheckObjectToTheRight(Vector3 Origin, Vector3 target)
    {
        float scalarFactor = Vector3.Dot(Origin.normalized, target.normalized);
        
        // If Left
        if (scalarFactor < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

}
