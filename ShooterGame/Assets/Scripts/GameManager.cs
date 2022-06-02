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

    //[SerializeField] AStar_Grid m_AStar_Grid;

    [SerializeField] public InputDevice tmp_inputDevice;//tmp

    private void OnEnable()
    {
        instance = this;
        m_eventManager = new EventManager();
        m_cameraManager = new CameraManager();
        m_inputManager = new InputManager();
        m_timer=new Timer();

        //m_AStar_Grid.CreateGrid();

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
    //public Grid<int> GetGridForPathFinding() { return m_AStar_Grid; }

    public void SetNewInputDevice(InputDevice aInputDevice) { m_inputManager.ClearInputDevices(); m_inputManager.AddInputDevice(aInputDevice); }
    public void AddInputDevice(InputDevice aInputDevice) { m_inputManager.AddInputDevice(aInputDevice); }

}

