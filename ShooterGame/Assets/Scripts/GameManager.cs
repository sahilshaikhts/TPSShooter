using UnityEngine;
using CameraSystem;
using EventSystem;
using InputSystem;
using Sahil.AStar;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    EventManager m_eventManager;
    CameraManager m_cameraManager;
    InputManager m_inputManager;
    Timer m_timer;

    [SerializeField] Grid_AStar m_AStar_Grid;

    [SerializeField] public InputDevice tmp_inputDevice;//tmp

    private void OnEnable()
    {
        instance = this;
        m_eventManager = new EventManager();
        m_cameraManager = new CameraManager();
        m_inputManager = new InputManager();
        m_timer=new Timer();

        m_AStar_Grid.CreateGrid();

        //TODO: this should be loaded from saved file from disk ,not scirptableobject
        AddInputDevice((InputDevice)tmp_inputDevice);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        m_timer.Update();
        m_eventManager.Update();
        m_inputManager.Update();
    }

    public CameraManager GetCameraManager() { return m_cameraManager; }
    public EventManager GetEventManager() { return m_eventManager; }
    public InputManager GetInputManager() { return m_inputManager; }
    public Timer GetTimer() { return m_timer; }
    public Grid_AStar GetGridForPathFinding() { return m_AStar_Grid; }

    public void SetNewInputDevice(InputDevice aInputDevice) { m_inputManager.ClearInputDevices(); m_inputManager.AddInputDevice(aInputDevice); }
    public void AddInputDevice(InputDevice aInputDevice) { m_inputManager.AddInputDevice(aInputDevice); }



    private void OnDrawGizmos()
    {
        if (m_AStar_Grid == null && m_AStar_Grid.m_grid == null) return;


        for (int x = 0; x < m_AStar_Grid.m_grid.GetLength(0); x++)
        for (int y = 0; y < m_AStar_Grid.m_grid.GetLength(1); y++)
        {
                if (m_AStar_Grid.m_grid[x, y].IsWalkable())
                    Gizmos.color = new Color(1, 1, 1, .6f);
                else
                    Gizmos.color = Color.red;
            Gizmos.DrawCube(m_AStar_Grid.m_grid[x,y].GetWorldPosition(), Vector3.one*2);
        }
    }
}

