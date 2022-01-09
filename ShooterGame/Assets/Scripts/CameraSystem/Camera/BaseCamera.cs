using UnityEngine;
namespace CameraSystem
{
public class BaseCamera : MonoBehaviour
{
    protected Camera m_camera;
    [SerializeField] string m_name;
    [SerializeField] LayerMask m_collisionLayer;

    [SerializeField] protected float m_POIDetectionRadius;//Range in which camera looks for point of interest (POI)

    protected void OnEnable()
    {
        m_camera = GetComponent<Camera>();
        RegisterToCameraManager();

    }
    private void RegisterToCameraManager()
    {
        CameraManager cameraManager = GameManager.GetCameraManager();
        cameraManager.RegisterCamera(this);
    }

    public void Activate()
    {
        GetComponent<Camera>().enabled = true;
    }

    public void Deactivate()
    {
        GetComponent<Camera>().enabled = false;
    }

   protected private CameraInterestPoint[] FindNearByInterestPoint()
   {
       CameraInterestPoint[] interestPoints = null;

       Collider[] colliders = Physics.OverlapSphere(transform.position, m_POIDetectionRadius, m_collisionLayer);
       
       if (colliders.Length > 0)
       {
           interestPoints = new CameraInterestPoint[colliders.Length];
   
           for (int i = 0; i < colliders.Length; i++)
           {
               interestPoints[i] = colliders[i].GetComponent<CameraInterestPoint>();
           }
       }
   
       return interestPoints;
   }
    
    public string GetName() { return m_name; }
    public GameObject GetGameObject() { return gameObject; }
    public Camera GetCamera() { return m_camera; }

}
}
