using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CharacterFollowCamera : MonoBehaviour
{
    public CinemachineVirtualCamera vCam;
    public VCamSwitcher vCamSwitcher;

    Transform player;
    public int _zoomLevel = 0; // 0: 기본줌, 1: 확대줌, 2: 최대확대줌
    float[] _zoomFOV = { 40f, 25f, 68.5f }; // 각 줌 레벨에 따른 FOV
    Vector3[] _followOffsets = {
        new Vector3(0, 30, -30),
        new Vector3(0, 6.8f, -23),
        new Vector3(0, 3.9f, -5.7f)
    };
    [SerializeField]
    InputActionReference mouseScrollAction;
    CinemachineTransposer cmTransposer;


    void Start()
    {
        cmTransposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        player = GameObject.FindWithTag("Player").transform;

        if (vCam != null)
        {
            vCam.Follow = player;
            vCam.LookAt = player;
        }
    }

    private void OnEnable()
    {
        mouseScrollAction.action.Enable();
        mouseScrollAction.action.performed += OnMouseScroll;
    }

    private void OnDisable()
    {
        mouseScrollAction.action.performed -= OnMouseScroll;
        mouseScrollAction.action.Disable();
    }

    private void OnMouseScroll(InputAction.CallbackContext context)
    {
        if (vCamSwitcher.ZoomState != VCamSwitcher.Enum_ZoomTypes.Default)
            return;

        float scrollValue = context.ReadValue<float>();
        if (scrollValue > 0)
        {
            // 마우스 휠 위로
            _zoomLevel = Mathf.Min(_zoomLevel + 1, _zoomFOV.Length - 1);
        }
        else if (scrollValue < 0)
        {
            // 마우스 휠 아래로
            _zoomLevel = Mathf.Max(_zoomLevel - 1, 0);
        }

        UpdateCameraZoom();
    }

    void UpdateCameraZoom()
    {
        vCam.m_Lens.FieldOfView = _zoomFOV[_zoomLevel];
        cmTransposer.m_FollowOffset = _followOffsets[_zoomLevel];
    }
}