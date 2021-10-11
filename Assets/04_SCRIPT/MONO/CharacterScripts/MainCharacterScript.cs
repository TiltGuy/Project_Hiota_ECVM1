using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterScript : MonoBehaviour
{

#region Global Settings

    [Header(" -- GLOBAL SETTINGS -- ")]

    [Tooltip("The speed of the player")]
    [SerializeField] private float m_speed = 5f;

    [Tooltip("Height of the jump")]
    [SerializeField] private float m_jumpHeight = 2f;

    /* Jamais appelé ou utilisé, ça sert à quoi ?
    [SerializeField] private float m_groundDistance = 0.2f;
    */

    [Tooltip("Distance of the dash")]
    [SerializeField] private float m_dashDistance = 5f;

    [Tooltip("It needs the prefab of CameraBase")]
    public Transform m_cameraBaseDirection;

    [Tooltip("the speed of the rotation between the forward of th character and the direction to go")]
    public float m_turnSpeed = 20;

    [Tooltip("The sum of the x axis of the controller and the ZQSD")]
    [SerializeField] private float m_finalInputCharacterX;

    [Tooltip("The sum of the z axis of the controller and the ZQSD")]
    [SerializeField] private float m_finalInputCharacterZ;

    ///<summary> The boolean that check if player is colliding with the ground</summary>
    private bool m_isGrounded = false;

    ///<summary> The Forward vector of the camera</summary>
    private Vector3 m_camF;

    ///<summary> The Right vector of the camera</summary>
    private Vector3 m_camR;

    ///<summary> The rigidbody of the player</summary>
    private Rigidbody m_body;

    ///<summary> The vector3 of the inputs. The inputs are by axis</summary>
    private Vector3 m_inputsKeyBoard = Vector3.zero;

    ///<summary> the float of the inputs of the controller. The inputs are by axis but seperated into 2 variables</summary>
    private float m_LeftStickControllerX;
    private float m_LeftStickControllerZ;


    #endregion

    private void Awake() 
    {
        //INSTANTIATION OF THE RIGIDBODY OF THE PLAYER
        m_body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //INITIALISATION AND UPDATE OF FORWARD & RIGHT VECTORS OF THE CAMERA
        m_camF = m_cameraBaseDirection.forward;
        m_camR = m_cameraBaseDirection.right;

        m_camF.y = 0;
        m_camR.y = 0;
        m_camF = m_camF.normalized;
        m_camR = m_camR.normalized;

        //INITIALISATION AND UPDATE OF THE INPUTS ON THE KEYBOARD
        m_inputsKeyBoard = Vector3.zero;
        m_inputsKeyBoard.x = Input.GetAxis("Horizontal");
        
        m_inputsKeyBoard.z = Input.GetAxis("Vertical");
        m_inputsKeyBoard = Vector3.ClampMagnitude(m_inputsKeyBoard, 1);

        //INITIALISATION AND UPDATE OF THE INPUTS ON THE CONTROLLER
        m_LeftStickControllerX = Input.GetAxis("LeftStickHorizontal");
        m_LeftStickControllerZ = Input.GetAxis("LeftStickVertical");

        //addition of the two axis and you don't have to set manually the active controller
        m_finalInputCharacterX = m_LeftStickControllerX + m_inputsKeyBoard.x;
        m_finalInputCharacterZ = m_LeftStickControllerZ + m_inputsKeyBoard.z;

        //ACCORD THE LOOK OF THE CHARACTER TO THE DIRECTION WHERE IT GOES
        if (m_inputsKeyBoard != Vector3.zero)
        {
            Vector3 _DirectionToGo = m_camF * m_inputsKeyBoard.z + m_camR * m_inputsKeyBoard.x;
            Quaternion finalrot = Quaternion.LookRotation(_DirectionToGo, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, finalrot, m_turnSpeed * Time.deltaTime);
        }
            

        //CHECKING IS THE PLAYER IS JUMPING & GIVING HIM A FORCE UPWARD
        if ((Input.GetButtonDown("JumpKeyBoard") || Input.GetButtonDown("JumpController")) && m_isGrounded)
        {
            m_body.AddForce(Vector3.up * Mathf.Sqrt(m_jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            m_isGrounded = false;
        }

        //CHEKING IF THE PLAYER IS DASHING & ADDING FORCE IN THE DIRECTION
        //NEED TO CHECK BECAUSE IT CAUSES A SPINNING OF THE CHARACTER WHEN TURNING DURING DASH
        
        if (Input.GetButtonDown("DashKeyBoard") || Input.GetButtonDown("DashController"))
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward, m_dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * m_body.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * m_body.drag + 1)) / -Time.deltaTime)));
            m_body.AddForce(dashVelocity, ForceMode.VelocityChange);
            Debug.Log("DASH!!!");
        }
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            m_isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            m_isGrounded = false;
        }
    }

    private void FixedUpdate()
    {
        //MOVEMENT OF THE PLAYER DEPENDING OF THE SPEED, THE INPUT DIRECTION AND THE CAMERA POSITION
        m_body.MovePosition(m_body.position + (m_camF *m_inputsKeyBoard.z + m_camR * m_inputsKeyBoard.x)  * m_speed * Time.fixedDeltaTime);
    }
}