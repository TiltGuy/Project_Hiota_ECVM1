using System.Collections;
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
            if(!PotentialEnemies.Contains(other.transform))
            {
                PotentialEnemies.Add(other.transform);
                AddEnemyToList?.Invoke(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TargetTag) )
        {
            if(PotentialEnemies.Contains(other.transform)
                || (PotentialEnemies.Contains(other.transform) && !other.gameObject.activeInHierarchy) )
            {
                RemoveEnemyToList?.Invoke(other.transform);
                TargetableEnemies.Remove(other.transform);
                PotentialEnemies.Remove(other.transform);
                
            }
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
                        }
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, dir, Color.red);
                        if (TargetableEnemies.Contains(enemy.transform) && !enemy.gameObject.activeInHierarchy)
                        {
                            TargetableEnemies.Remove(enemy.transform);

                        }
                        // if Not then remove this element of the List of Targetable Enemies
                    }
                }
                else
                {
                    if (TargetableEnemies.Contains(objToVerify.transform))
                    {
                        TargetableEnemies.Remove(objToVerify.transform);

                    }
                    //Debug.Log("the enemy : " + enemy.transform + " hasn't been detected", enemy.transform);
                }

                
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

        print(input);
        if (input.x>.7f)
        {
            if(SortedListOfEnemies.IndexOf(currentHiotaTarget) - 1 >=0 )
            {
                Transform nextObjectToTheRight = SortedListOfEnemies[(SortedListOfEnemies.IndexOf(currentHiotaTarget) - 1)];
                objectToReturn = nextObjectToTheRight;
                //Debug.Log(nextObjectToTheRight + "Object to the right", nextObjectToTheRight);
            }
            
            return objectToReturn;
        }
        else if(input.x < -0.7f)
        {
            if (SortedListOfEnemies.IndexOf(currentHiotaTarget) + 1 < SortedListOfEnemies.Count)
            {
                Transform nextObjectToTheLeft = SortedListOfEnemies[(SortedListOfEnemies.IndexOf(currentHiotaTarget) + 1)];
                objectToReturn = nextObjectToTheLeft;
                //Debug.Log((SortedListOfEnemies.IndexOf(currentHiotaTarget) + 1) + "Object to the Left", nextObjectToTheLeft);
            }

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

        return SortedListOfEnemies.First();
    }

}
