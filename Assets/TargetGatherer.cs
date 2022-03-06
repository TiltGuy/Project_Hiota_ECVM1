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
    private Transform currentTarget;
    public float dirNum;

    private void Awake()
    {
        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.transform;
        currentTarget = controller.currentHiotaTarget;
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

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
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

        Vector3 heading = currentTarget.position - mainCameraTransform.position;
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
                    //Debug.Log("the enemy : " + enemy.transform + " hasn't been detected", enemy.transform);
                }

                
            }
        }

    }

    float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
        print(dir + " dirnum valeur");

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

    private void UpdateTargetableList(List<Transform> ListToRearrange)
    {
        if (ListToRearrange.Count > 0)
        {
            // Check Distance to the target Gatherer
            // check the Scalar of the CurrentTarget with the Camera
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
        Transform currentHiotaTarget = controller.currentHiotaTarget;
        Transform objectToReturn;
        objectToReturn = currentHiotaTarget;
        //print(input);
        if (input.x>.25)
        {
            
            for(int i = 0; i < TargetableEnemies.Count; i++)
            {
                if(TargetableEnemies[i].transform != currentHiotaTarget)
                {
                    Vector3 heading = currentTarget.position - mainCameraTransform.position;
                    float currentDirNum = AngleDir(mainCameraTransform.forward, heading, mainCameraTransform.up);
                    if(currentDirNum<0)
                    {
                        foreach(Transform targets in TargetableEnemies)
                        {

                        }
                    }
                }

            }
            Debug.Log("final " + objectToReturn, objectToReturn);
            return objectToReturn;
        }
        else if(input.x < 0.25f)
        {
            for (int i = 0; i < TargetableEnemies.Count; i++)
            {
                Vector3 currentPotentialNewDirection = TargetableEnemies[i].position - mainCameraTransform.position;
                if ((TargetableEnemies[i].transform != currentHiotaTarget)
                    && (CheckObjectToTheLeft(mainCameraTransform.right, currentPotentialNewDirection)))
                {
                    float currentDot = Vector3.Dot(mainCameraTransform.right, currentPotentialNewDirection);
                    foreach (Transform targetsToCompare in TargetableEnemies)
                    {
                        if (targetsToCompare != currentHiotaTarget)
                        {
                            Vector3 currentDirectionToCompare = targetsToCompare.position - mainCameraTransform.position;
                            float currentDotToCompare = Vector3.Dot(mainCameraTransform.right, currentDirectionToCompare);
                            if ((currentDot <= currentDotToCompare))
                            {
                                //Debug.Log("Je passe bien ici");
                                if (CheckObjectToTheLeft(mainCameraTransform.right, currentDirectionToCompare))
                                {

                                    objectToReturn = targetsToCompare;
                                }
                            }

                        }
                    }

                }
            }
            return objectToReturn;
        }
        return objectToReturn;
    }

}
