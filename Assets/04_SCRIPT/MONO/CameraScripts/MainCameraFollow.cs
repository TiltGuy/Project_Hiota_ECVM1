using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraFollow : MonoBehaviour
{

#region Global Settings

    [Header("-- GLOBAL SETTINGS --", order = 0)]
    
   //[SerializeField] private float m_cameraMoveSpeed;

    [Tooltip("It needs the GameObject cameraTarget")]
    public Transform m_cameraTarget;
    //Vector3 m_followPOS;

    [Tooltip("It needs the GameObject cameraTarget")]
    public bool b_CameraFocused;

    [Tooltip("The minimum angle of the camera on x axis")]
    [SerializeField] private float m_clampAngleDown = -66;

    [Tooltip("The maximum angle of the camera on x axis")]
    [SerializeField] private float m_clampAngleUp = 80f;

    [Tooltip("The speed at which the rotation occurs when the input is used")]
    [SerializeField] private float m_inputSensivity = 150f;

    [Tooltip("The main camera GameObject")]
    public GameObject m_cameraObj;

    [Tooltip("The player prefab")]
    public GameObject m_playerObj;

    ///<summary> The vector3 of the inputs. The inputs are by axis</summary>
    private Vector3 m_inputs = Vector3.zero;

    /*
    private float m_cameraDistanceXToPlayer;
    private float m_cameraDistanceYToPlayer;
    private float m_cameraDistanceZToPlayer;*/

    ///<summary>The offset of the camera depending on the player's position</summary>
    private Vector3 m_cameraOffset;
    public Vector3 m_FocusCameraOffset;
    #endregion

#region Axis Informations
    [Header("PRIVATE ROTATION AXIS INFORMATIONS", order = 2)]

    /*public float m_smoothX;
    public float m_smoothY;*/

    [Tooltip("The mouse rotation on the x axis")]
     [SerializeField] private float m_mouseX;

    [Tooltip("The mouse rotation on the y axis")]
    [SerializeField] private float m_mouseY;

    [Tooltip("The sum of the x axis of the controller and the mouse")]
    [SerializeField] private float m_finalInputX;
    
    [Tooltip("The sum of the z axis of the controller and the mouse")]
    [SerializeField] private float m_finalInputZ;

    ///<summary>The rotation of the camera in x axis</summary>
    private float m_rotX = 0.0f;

    ///<summary>The rotation of the camera in y axis</summary>
    private float m_rotY = 0.0f;

    [Range(0.01f, 100)]
    [Tooltip("The smoothing factor for the camera rotation ranging between 0.01f and 20f. The higher, the smoother(preferred value = 15)")]
    [SerializeField] private float m_smoothFactor = 0.5f;
#endregion



    void Awake()
    {
        //INSTANTIATE THE ROTATION ANGLES OF THE CAMERA 
        Vector3 v3_rot = transform.localRotation.eulerAngles;
        m_rotX = v3_rot.x;
        m_rotY = v3_rot.y;

        //INSTANTIATE THE CURSOR AND THE OFFSET OF THE CAMERA
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_cameraOffset = transform.position - m_playerObj.transform.position;

        //INSTANTIATE THE BOOLEAN STATE "FOCUSED"
        b_CameraFocused = m_playerObj.gameObject.GetComponent<HiotaController_FSM>().b_IsFocusing;
    }

    private void Update()
    {
        //b_CameraFocused = m_playerObj.gameObject.GetComponent<HiotaController_FSM>().b_IsFocusing;
        
        
        //CameraUpdater();
        
        

        ///Setup and Update the final rotation of the Camera
        

         

        

        ///Setup the clamp of the rotation
        m_rotX = Mathf.Clamp(m_rotX, m_clampAngleDown, m_clampAngleUp);
    }

    private void LateUpdate()
    {
        if (!b_CameraFocused)
        {
            InputUpdate();

            ///////////////////////
            ///ROTATE THE CAMERA///
            ///////////////////////
            ///Update the rotation vector y and X
            m_rotY += m_finalInputX * m_inputSensivity * Time.deltaTime;
            m_rotX += m_finalInputZ * m_inputSensivity * Time.deltaTime;

            ///Update the rotation of the camera
            Quaternion localRotation = Quaternion.Euler(m_rotX, m_rotY, 0.0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, localRotation, m_inputSensivity * Time.deltaTime);
            m_cameraObj.transform.LookAt(m_cameraTarget.position);
            CameraFollow(m_playerObj.transform.position);
        }
        else
        {
            //Vector3 
            //m_cameraObj.transform.LookAt(Vector3.Distance(m_playerObj.GetComponent<HiotaController_FSM>().hiotaTarget.transform.position,m_playerObj.transform.position),);
            Debug.DrawLine(m_playerObj.transform.position, m_playerObj.GetComponent<HiotaController_FSM>().currentHiotaTarget.transform.position);
            Quaternion targetrotation = m_playerObj.transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, m_inputSensivity * Time.deltaTime);
            CameraFollow(m_playerObj.transform.position + m_FocusCameraOffset);
            m_cameraObj.transform.LookAt(m_playerObj.GetComponent<HiotaController_FSM>().currentHiotaTarget.transform.position);

        }
    }

    void CameraFollow(Vector3 player)
    {
        //Position of the player relative to the offset of the camera
        Vector3 desiredPosition = player + m_cameraOffset;

        //Position of the camera going toward it's desired position with a smoothing factor
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, m_smoothFactor * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    

    void InputUpdate()
    {
        ///////////////////////
        ///SETUP OF THE AXIS///
        ///////////////////////
        
        //Setup of the rotation and the sticks just here
        float inputX = Input.GetAxis("RightStickHorizontal");
        float inputZ = Input.GetAxis("RightStickVertical");
        m_mouseX = Input.GetAxis("Mouse X");
        m_mouseY = Input.GetAxis("Mouse Y");

        //addition of the two axis and you don't have to set manually the active controller
        m_finalInputX = inputX + m_mouseX;
        m_finalInputZ = inputZ + m_mouseY;
    }

    Vector3 CalculateMidVector(Vector3 Origin,Vector3 Target)
    {
        return Origin + Target;
    }

    /*void CameraUpdater()
    {
        Transform target = m_cameraTarget.transform;


        float step = m_cameraMoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
    }*/
}
