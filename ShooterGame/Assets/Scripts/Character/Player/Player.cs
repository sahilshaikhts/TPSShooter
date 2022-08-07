using EventSystem;
using CameraSystem;
using UnityEngine;
using WeaponSystem;
using Sahil.AStar;
using System.Collections.Generic;

namespace ShootingGame
{
    public class Player : Character, IShotable
    {
        public Transform target;
        PlayerController m_playerController;
        MovementComponent m_movementComponent;

        WeaponHolder m_weaponHolder;
        PathFinder m_pathFinder;
        BaseCamera m_followCamera;
        [SerializeField] WeaponBase tmp_pistol;
        [SerializeField] LayerMask m_weaponLayerMask;
        [SerializeField] LayerMask m_enemyLayerMask;
        [SerializeField] Texture tmp_crosshair;

        void Start()
        {
            m_followCamera = GameManager.instance.GetCameraManager().GetCamera("PlayerFollowCamera");
            m_playerController = new PlayerController(this, m_followCamera);
            m_movementComponent = GetComponent<MovementComponent>();
            m_weaponHolder = new WeaponHolder(m_weaponLayerMask);

            GameManager.instance.GetEventManager().SubscribeToEvent(MoveEvent.EventType(), HandleMoveEvent);
            GameManager.instance.GetEventManager().SubscribeToEvent(JumpEvent.EventType(), HandleJumpEvent);
            GameManager.instance.GetEventManager().SubscribeToEvent(SetPlayerRotationEvent.EventType(), HandleSetPlayerRotationEvent);
            GameManager.instance.GetEventManager().SubscribeToEvent(WeaponFireEvent.EventType(), HandleWeaponFireEvent);
            GameManager.instance.GetEventManager().SubscribeToEvent(WeaponDrawEvent.EventType(), HandleWeaponDrawEvent);

            m_pathFinder = new PathFinder(GameManager.instance.GetGridForPathFinding());

            WeaponBase pistol = Instantiate(tmp_pistol);
            m_weaponHolder.AddNewWeapon(pistol);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                GameManager.instance.GetEventManager().AddEvent(new AIShootTargetEvent(target.GetComponent<Character>(), GetPositionOfHead() - target.GetComponent<Character>().GetPositionOfHead()));
            }
        }

        void HandleJumpEvent(IEvent aEvent)
        {
            JumpEvent jumpEvent = (JumpEvent)aEvent;
            if (jumpEvent.GetCallerCharacter() == this)
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
            if (weaponFireEvent.GetCallerCharacter() == this)
            {
                m_weaponHolder.Fire(m_followCamera.GetCamera().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)));
            }
        }
        void HandleWeaponDrawEvent(IEvent aEvent)
        {
            WeaponDrawEvent weaponFireEvent = (WeaponDrawEvent)aEvent;
            if (weaponFireEvent.GetCallerCharacter() == this)
            {
                Collider[] enemies_nearBy;
                enemies_nearBy = Physics.OverlapSphere(transform.position, 15, m_enemyLayerMask);

                float fov = m_followCamera.GetCamera().fieldOfView;

                foreach (Collider enemy in enemies_nearBy)
                {
                    Vector3 enemyDirection = enemy.transform.position - transform.position;

                    float dot = Mathf.Clamp(Vector3.Dot(transform.forward, enemyDirection.normalized),-1,1);
                    float angleBetweenEnemy = Mathf.Acos(dot) * Mathf.Rad2Deg;
                    Debug.Log(angleBetweenEnemy);

                    //****Change it to check from camera forward ignoring the x rotation.

                    if (angleBetweenEnemy < 25)
                    {
                        GameManager.instance.GetEventManager().AddEvent(new PlayerAboutToShootEvent(enemy.GetComponent<Character>()));
                    }
                }
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
            List<Vector3> path = m_pathFinder.GetNeighbourCellsPosition(transform.position, 3);

            if (path == null) return;
            float height = GetPositionOfHead().y - transform.position.y;
            for (int j = 0; j < path.Count; j++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[j] + Vector3.up * height, Vector3.one * .5f);
            }
        }

        public void HandleGettingShot(Vector3 projetileDirection, int damageAmount)
        {
            Debug.Log("Got shot!!");
        }

    }
}
