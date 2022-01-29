using EventSystem;
using System.Collections.Generic;
using UnityEngine;

namespace CameraSystem
{
public class CameraManager
{
    private Dictionary<string, BaseCamera> m_sceneCameras;
    private BaseCamera m_activeCamera;

    public CameraManager()
    {
        m_sceneCameras = new Dictionary<string, BaseCamera>();

        GameManager.GetEventManager().SubscribeToEvent(InputEvent.EventType(), HandleInputEvent);
        GameManager.GetEventManager().SubscribeToEvent(AxisInputEvent.EventType(), HandleAxisInputEvent);
    }

    public void SwitchCamera(BaseCamera aNewActiveCamera)
    {
        if (m_activeCamera)
            m_activeCamera.Deactivate();

        aNewActiveCamera.Activate();

        m_activeCamera = aNewActiveCamera;
    }

    public void SwitchCamera(string aNewActiveCameraName)
    {
        BaseCamera newCamera;
        m_sceneCameras.TryGetValue(aNewActiveCameraName, out newCamera);
        if (newCamera)
        {
            newCamera.Activate();
            if (m_activeCamera)
                m_activeCamera.Deactivate();
            return;
        }
        Debug.LogWarning(aNewActiveCameraName + " camera not found");
    }

    public void RegisterCamera(BaseCamera aCamera)
    {
        if (aCamera.GetName() == "") { Debug.LogWarning(aCamera.name + " Assign a unique m_name in Camera "); }

        if (m_sceneCameras.ContainsKey(aCamera.GetName()))
        {
            Debug.LogWarning(aCamera.name + " Already registered");
            return;
        }

        m_sceneCameras.Add(aCamera.GetName(), aCamera);

        Debug.Log(aCamera.name + " has been registered");
    }

    public BaseCamera GetCamera(string aNewActiveCameraName)
    {
        BaseCamera newCamera;
        m_sceneCameras.TryGetValue(aNewActiveCameraName, out newCamera);
        if (newCamera)
        {
            return newCamera;
        }
        Debug.LogWarning(aNewActiveCameraName + " camera not found");
        return null;
    }

    void HandleInputEvent(IEvent aEvent)
    {
        InputEvent inputEvent = (InputEvent)aEvent;

        if (inputEvent.GetAction() == "FocusAimming")
        {
            BaseCamera playerCameraBase;
            m_sceneCameras.TryGetValue("PlayerFollowCamera", out playerCameraBase);

            if (playerCameraBase)
            {
                TPSFollowCamera playerTPSCamera = (TPSFollowCamera)playerCameraBase;
                playerTPSCamera.SetDistance(playerTPSCamera.GetDistance() / 2);
            }   
        }
        else if (inputEvent.GetAction() == "UnFocusAimming")
        {
            BaseCamera playerCameraBase;
            m_sceneCameras.TryGetValue("PlayerFollowCamera", out playerCameraBase);

            if (playerCameraBase)
            {
                TPSFollowCamera playerTPSCamera = (TPSFollowCamera)playerCameraBase;
                playerTPSCamera.SetDistance(playerTPSCamera.GetDistance() * 2);
            }
        }
    }

    void HandleAxisInputEvent(IEvent aEvent)
    {
        AxisInputEvent inputEvent = (AxisInputEvent)aEvent;
        if (inputEvent.GetAction() == "CameraLook")
        {
            BaseCamera playerCameraBase;
            m_sceneCameras.TryGetValue("PlayerFollowCamera", out playerCameraBase);
            if (playerCameraBase)
            {
                TPSFollowCamera playerCamera = (TPSFollowCamera)playerCameraBase;
                playerCamera.MouseMoved(inputEvent.GetValue());

                GameManager.GetEventManager().AddEvent(new SetPlayerRotationEvent(playerCamera.GetGameObject().transform.forward));
            }
        }
    }

}
}
