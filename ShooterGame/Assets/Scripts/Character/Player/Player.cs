using EventSystem;
using CameraSystem;
using UnityEngine;
using WeaponSystem;

namespace ShootingGame
{
public class Player : Character
{
    PlayerController m_playerController;
    MovementComponent m_movementComponent;
    GM_WeaponHolder m_weaponHolder;

    [SerializeField]WeaponBase tmp_pistol;
    [SerializeField]LayerMask m_weaponLayerMask;
    [SerializeField]Texture tmp_crosshair;

    void Start()
    {
        m_playerController = new PlayerController(gameObject,GameManager.GetCameraManager().GetCamera("PlayerFollowCamera").gameObject);
        m_movementComponent = GetComponent<MovementComponent>();
        m_weaponHolder = new GM_WeaponHolder(m_weaponLayerMask);

        GameManager.GetEventManager().SubscribeToEvent(MoveEvent.EventType(), HandleMoveEvent);
        GameManager.GetEventManager().SubscribeToEvent(JumpEvent.EventType(), HandleJumpEvent);
        GameManager.GetEventManager().SubscribeToEvent(SetPlayerRotationEvent.EventType(), HandleSetPlayerRotationEvent);
        GameManager.GetEventManager().SubscribeToEvent(WeaponFireEvent.EventType(), HandleWeaponFireEvent);


        WeaponBase pistol = Instantiate(tmp_pistol);
        m_weaponHolder.AddNewWeapon(pistol);
    }

    void HandleJumpEvent(EventSystem.IEvent aEvent) 
    {
        JumpEvent jumpEvent = (JumpEvent)aEvent;
        if (jumpEvent.GetCallerGameObject() == gameObject)
            m_movementComponent.Jump();
    }

    void HandleSetPlayerRotationEvent(EventSystem.IEvent aEvent)
    {
        SetPlayerRotationEvent rotateEvent = (SetPlayerRotationEvent)aEvent;
        Vector3 rotation = rotateEvent.GetRotation();
        rotation.y = 0;

        m_movementComponent.AddRotation(rotation);
    }

    void HandleMoveEvent(EventSystem.IEvent aEvent)
    {
        MoveEvent moveEvent = (MoveEvent)aEvent;
        m_movementComponent.SetMovementDirection(moveEvent.GetDirection());
    }

    void HandleWeaponFireEvent(EventSystem.IEvent aEvent)
    {
        WeaponFireEvent weaponFireEvent = (WeaponFireEvent)aEvent;
        if(weaponFireEvent.GetCallerObject()==gameObject)
        {
            BaseCamera camera= GameManager.GetCameraManager().GetCamera("PlayerFollowCamera");
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
}
}
