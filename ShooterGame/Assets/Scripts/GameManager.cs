using CameraSystem;
using EventSystem;
using InputSystem;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static EventManager m_eventManager;
    static CameraManager m_cameraManager;
    static InputManager m_inputManager;

    [SerializeField] public InputDevice tmp_inputDevice;//tmp

    private void OnEnable()
    {
        m_eventManager = new EventManager();
        m_cameraManager = new CameraManager();
        m_inputManager = new InputManager();

        //TODO: this should be loaded from saved file from disk ,not scirptableobject
        AddInputDevice((InputDevice)tmp_inputDevice);


        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        m_eventManager.Update();
        m_inputManager.Update();
    }

    public static CameraManager GetCameraManager() { return m_cameraManager; }
    public static EventManager GetEventManager() { return m_eventManager; }

    public void SetNewInputDevice(InputDevice aInputDevice) { m_inputManager.ClearInputDevices(); m_inputManager.AddInputDevice(aInputDevice); }
    public void AddInputDevice(InputDevice aInputDevice) { m_inputManager.AddInputDevice(aInputDevice); }
}

