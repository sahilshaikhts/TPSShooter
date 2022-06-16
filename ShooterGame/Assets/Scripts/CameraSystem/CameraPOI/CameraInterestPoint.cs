using UnityEngine;

namespace CameraSystem
{
    //Have base POI,make differnt types,staticPOI,dynamicPOI(e.g. new area unlocked, that will have high score and after cinematic ,change it to low or disable)
public class CameraInterestPoint : MonoBehaviour
{
        //Mode m_mode;//Cinematic
    [Range(0, 1)][SerializeField] float m_priorityScore;

    [SerializeField] float m_detectionRadius;

    private void Start()
    {
        SphereCollider collider = gameObject.GetComponent<SphereCollider>();
        collider.isTrigger = true;

        collider.radius = m_detectionRadius;
    }

    public float GetPriorityScore() { return m_priorityScore; }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(Color.blue.r, Color.blue.g, Color.blue.b, 0.1f);
        Gizmos.DrawSphere(transform.position, m_detectionRadius);
    }
}
}
