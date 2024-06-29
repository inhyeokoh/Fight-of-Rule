using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraFollow : MonoBehaviour
{
    CinemachineVirtualCamera virtualCamera;
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

    public enum Enum_ZoomTypes
    {
        Default,
        DialogZoom,
    }
    Enum_ZoomTypes zoomState;
    public Enum_ZoomTypes ZoomState
    {
        get { return zoomState; }
        set
        {
            if (zoomState != value)
            {
                zoomState = value;
                OnZoomStateChanged();
            }
        }
    }
    public Vector3 NpcPos { get; set; }
    Transform lookAtTransform;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cmTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        player = GameObject.FindWithTag("Player").transform;

        // LookAt을 위한 Transform을 플레이어의 Transform으로 초기화
        lookAtTransform = new GameObject("LookAtTransform").transform;

        if (virtualCamera != null)
        {
            virtualCamera.Follow = player;
            virtualCamera.LookAt = player;
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
        if (zoomState != Enum_ZoomTypes.Default)
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
        virtualCamera.m_Lens.FieldOfView = _zoomFOV[_zoomLevel];
        cmTransposer.m_FollowOffset = _followOffsets[_zoomLevel];
    }

    void OnZoomStateChanged()
    {
        switch (zoomState)
        {
            case Enum_ZoomTypes.Default:
                UpdateCameraZoom();
                virtualCamera.LookAt = player;
                break;
            case Enum_ZoomTypes.DialogZoom:
                SetDialogCameraPosition();
                break;
        }
    }

    void SetDialogCameraPosition()
    {
        // NPC와 플레이어 사이의 중간 지점
        Vector3 midPoint = (player.position + NpcPos) / 2;
        // NPC와 플레이어 사이의 방향 벡터
        Vector3 direction = (NpcPos - player.position).normalized;
        Vector3 rightOffset = Vector3.Cross(Vector3.up, direction).normalized * 6f;

        lookAtTransform.position = midPoint;

        cmTransposer.m_FollowOffset = rightOffset + new Vector3(0, 4f, 0); // 조금 더 위로 올림
        virtualCamera.m_Lens.FieldOfView = 80f;
        virtualCamera.LookAt = lookAtTransform;
    }
}