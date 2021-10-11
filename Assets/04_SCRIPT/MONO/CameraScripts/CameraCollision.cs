using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{

#region Global settings

    [Header("-- GLOBAL SETTINGS --")]

    [Tooltip("The minimum distance of the camera")]
    [SerializeField] private float m_minDistance = 1.0f;

    [Tooltip("The maximum distance of the camera")]
    [SerializeField] private float m_maxDistance = 4.0f;

    [Tooltip("The smoothing of the camera's positioning")]
    [SerializeField] private float m_smooth = 10.0f;

    ///<summary>The vector3 position of the camera</summary>
    private Vector3 m_v3DollyDir;
    
    //public Vector3 v3_DollyDirAdjusted;

    ///<summary>The distance between the camera and the player's target</summary>
    private float m_distance;
    
#endregion

    // Start is called before the first frame update
    void Awake()
    {
        m_v3DollyDir = transform.localPosition.normalized;
        m_distance = transform.localPosition.magnitude;
        m_maxDistance = Mathf.Abs(transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v3_DesiredCameraPos = transform.parent.TransformPoint(m_v3DollyDir * m_maxDistance);

        RaycastHit hit;

        if (Physics.Linecast(transform.parent.position, v3_DesiredCameraPos, out hit, 8))
        {
            ///Why "* 0.8f" ? It's avoiding clipping with the camera
            m_distance = Mathf.Clamp(hit.distance * 0.8f, m_minDistance, m_maxDistance);
        }
        else
        {
            m_distance = m_maxDistance;
        }

        ///Set the position of the camera with the distance calculated just up there
        transform.localPosition = Vector3.Lerp(transform.localPosition, m_v3DollyDir * m_distance, m_smooth * Time.deltaTime);
        
        Debug.DrawRay(transform.localPosition, m_v3DollyDir, Color.green);
        //Debug.Log(m_maxDistance);
    }
}
