using EventSystem;
using CameraSystem;
using UnityEngine;
using WeaponSystem;
using Sahil.AStar;

namespace ShootingGame
{
public class Player : Character,IShotable
{
    public Transform target;
    PlayerController m_playerController;
    MovementComponent m_movementComponent;
    WeaponHolder m_weaponHolder;

    [SerializeField]WeaponBase tmp_pistol;
    [SerializeField]LayerMask m_weaponLayerMask;
    [SerializeField]Texture tmp_crosshair;
        System.Collections.Generic.List<Cell> path;
    void Start()
    {
        m_playerController = new PlayerController(gameObject,GameManager.instance.GetCameraManager().GetCamera("PlayerFollowCamera").gameObject);
        m_movementComponent = GetComponent<MovementComponent>();
        m_weaponHolder = new WeaponHolder(m_weaponLayerMask);

        GameManager.instance.GetEventManager().SubscribeToEvent(MoveEvent.EventType(), HandleMoveEvent);
        GameManager.instance.GetEventManager().SubscribeToEvent(JumpEvent.EventType(), HandleJumpEvent);
        GameManager.instance.GetEventManager().SubscribeToEvent(SetPlayerRotationEvent.EventType(), HandleSetPlayerRotationEvent);
        GameManager.instance.GetEventManager().SubscribeToEvent(WeaponFireEvent.EventType(), HandleWeaponFireEvent);

        WeaponBase pistol = Instantiate(tmp_pistol);
        m_weaponHolder.AddNewWeapon(pistol);
    }
    private void Update()
    {
    }

    void HandleJumpEvent(IEvent aEvent) 
    {
        JumpEvent jumpEvent = (JumpEvent)aEvent;
        if (jumpEvent.GetCallerGameObject() == gameObject)
            m_movementComponent.Jump();
    }

    void HandleSetPlayerRotationEvent(IEvent aEvent)
    {
        SetPlayerRotationEvent rotateEvent = (SetPlayerRotationEvent)aEvent;
        Vector3 rotation = rotateEvent.GetRotation();
        rotation.y = 0;

        m_movementComponent.AddRotation(rotation);
    }

    void HandleMoveEvent(IEvent aEvent)
    {
        MoveEvent moveEvent = (MoveEvent)aEvent;
        m_movementComponent.SetMovementDirection(moveEvent.GetDirection());
    }

    void HandleWeaponFireEvent(IEvent aEvent)
    {
        WeaponFireEvent weaponFireEvent = (WeaponFireEvent)aEvent;
        if(weaponFireEvent.GetCallerObject()==gameObject)
        {
            BaseCamera camera= GameManager.instance.GetCameraManager().GetCamera("PlayerFollowCamera");
            m_weaponHolder.Fire(camera.GetCamera().ViewportPointToRay(new Vector3(0.5f,0.5f,0)));
        }
    }

    void OnGUI()
    {
        GUI.color = Color.black;
        GUI.DrawTexture(new Rect(Screen.width / 2 + 0.75f, Screen.height / 2 + 0.75f, 4, 4), tmp_crosshair);
        GUI.color = Color.white;
        GUI.DrawTexture(new Rect(Screen.width / 2, Screen.height / 2, 4, 4), tmp_crosshair);
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Vector3 pos = GameManager.instance.GetGridForPathFinding().NodeFromWorldPoint(transform.position).GetWorldPosition();
        //Gizmos.DrawCube(pos, Vector3.one*10);
        //pos = GameManager.instance.GetGridForPathFinding().NodeFromWorldPoint(target.transform.position).GetWorldPosition();
        //Gizmos.DrawCube(pos, Vector3.one);

        if (path == null) return;
        for (int j = 0; j < path.Count; j++)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawCube(path[j].GetWorldPosition() + Vector3.up * .6f, Vector3.one * 2);
        }
    }

    public void HandleGettingShot(Vector3 projetileDirection, int damageAmount)
    {
        throw new System.NotImplementedException();
    }

}
}
