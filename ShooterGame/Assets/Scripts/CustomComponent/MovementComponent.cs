using UnityEngine;

public class MovementComponent : MonoBehaviour
{
    private GameObject m_owner;

    private Rigidbody m_owner_rigidbody;

    private Vector3 m_inputDirection;
    private Vector3 m_velocity;
    private Vector3 m_additionalForce;
    private Vector3 m_additionalRotationDirection;
    [SerializeField] bool LookTowardsMovementDirection=false;

    [SerializeField] private float m_speed, m_maxSpeed;
    [SerializeField] private float m_jumpSpeed;

    [SerializeField] private float m_gravityAccelaration;
    [SerializeField] private float m_friction;

    [SerializeField] private LayerMask m_jumpGroundCheckMask;

    private bool doJump = false;
    private bool onGround;

    private void Start()
    {
        m_owner = gameObject;
        m_owner_rigidbody = GetComponent<Rigidbody>();

        m_owner_rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        m_velocity = Vector3.zero;
    }

    private void Update()
    {
        onGround = CheckIfOnGround();
    }

    private void FixedUpdate()
    {
        m_velocity = m_owner_rigidbody.velocity;
        CalculateVelocity();
    }

    private void CalculateVelocity()
    {
        Vector3 newVelocity = m_velocity;

        if (!onGround)
        {
            Vector3 movDirection = m_inputDirection;
            movDirection *= m_speed;

            newVelocity += movDirection * Time.fixedDeltaTime;
            newVelocity = Vector3.ClampMagnitude(newVelocity, m_maxSpeed);

            m_inputDirection = Vector3.zero;

            m_velocity = newVelocity;

            ApplyDownwardSpeed();
        }
        else
        if (m_inputDirection.sqrMagnitude > 0.1f)
        {
            Vector3 movDirection = m_inputDirection;

            movDirection.Normalize();

            Vector3 groundTangent = movDirection - Vector3.Project(movDirection, GetGroundNormal());
            groundTangent.Normalize();
            movDirection = groundTangent;

            Vector3 velAlongMoveDir = Vector3.Project(newVelocity, movDirection);

            if (Vector3.Dot(velAlongMoveDir, movDirection) > 0.0f)
            {
                newVelocity = Vector3.Lerp(newVelocity, velAlongMoveDir, m_friction * Time.fixedDeltaTime);
            }
            else
            {
                newVelocity = Vector3.Lerp(newVelocity, Vector3.zero, m_friction * Time.fixedDeltaTime);
            }
            movDirection *= m_speed;

            newVelocity += movDirection * Time.fixedDeltaTime;
            newVelocity = Vector3.ClampMagnitude(newVelocity, m_maxSpeed);

            m_inputDirection = Vector3.zero;

            m_velocity = newVelocity;
        }
        else Friction();

        if (doJump) {PerformJump(); }

        if (m_additionalForce.sqrMagnitude > 0)
        {
            m_velocity += m_additionalForce; m_additionalForce = Vector3.zero;
        }

        ApplyVelocity();

        if (LookTowardsMovementDirection)
        {
            Vector3 facingDirection = m_owner_rigidbody.velocity.normalized;
            facingDirection.Normalize();
            facingDirection.y = 0;

            if (m_owner_rigidbody.velocity.magnitude > 0.1f)
                RotateTowards( facingDirection);
        }

        if (m_additionalRotationDirection.sqrMagnitude > 0)
        {
            RotateTowards(m_additionalRotationDirection);
            m_additionalRotationDirection = Vector3.zero;
        }
        doJump = false;
    }


    private void Friction() { m_velocity = Vector3.Lerp(m_velocity, new Vector3(0, m_owner_rigidbody.velocity.y, 0), m_friction * Time.fixedDeltaTime); }

    private void ApplyDownwardSpeed() { m_velocity.y += m_gravityAccelaration * Time.fixedDeltaTime; }

    private void ApplyVelocity()
    {
        Vector3 velocityDiff = m_velocity - m_owner_rigidbody.velocity;
        m_owner_rigidbody.AddForce(velocityDiff, ForceMode.VelocityChange);
    }

    void PerformJump()
    {
        if (onGround)
        {
            m_velocity.y = m_jumpSpeed;
            transform.position += new Vector3(0.0f, 0.2f, 0.0f);
            doJump = false;
        }
    }

    private bool CheckIfOnGround()
    {
        Ray ray = new Ray(m_owner.transform.position + Vector3.up, -m_owner.transform.up);

        RaycastHit[] hitInfos = Physics.SphereCastAll(ray.origin, 0.5f, ray.direction, 1.02f, m_jumpGroundCheckMask);

        if (hitInfos.Length > 0)
            return true;

        return false;
    }

    private Vector3 GetGroundNormal()
    {
        Vector3 normal = Vector3.zero;
        RaycastHit hit = new RaycastHit();

        Ray ray = new Ray(m_owner.transform.position + Vector3.up, Vector3.down);
        
        RaycastHit[] hitInfos = Physics.SphereCastAll(ray.origin, 0.5f, ray.direction, 2, m_jumpGroundCheckMask);

        float minGroundDist = float.MaxValue;
        foreach (RaycastHit hitInfo in hitInfos)
        {
            if (hitInfo.distance < minGroundDist)
            {
                minGroundDist = hitInfo.distance;
                hit = hitInfo;
                normal = hit.normal;
            }
        }

        Debug.DrawRay(hit.point, normal, Color.red, 1);
        return normal;

    }

    public void SetMovementDirection(Vector3 direction)
    {
        m_inputDirection += direction;
        m_inputDirection.Normalize();
    }

    public void AddRotation(Vector3 direction)
    {
        m_additionalRotationDirection = direction;
    }

    void RotateTowards(Vector3 direction)
    {
        direction.Normalize();

        m_owner_rigidbody.rotation = Quaternion.LookRotation(direction);
    }
  
    public void ApplyForce(Vector3 direction, float force)
    {
        m_additionalForce += direction * force;
        Debug.Log("Force");
    }

    public void Jump() { doJump = true; }


    void MoveHandler(Vector3 direction) { SetMovementDirection(direction); }
}
