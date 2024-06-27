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
    Vector3 midPos;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cmTransposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        player = GameObject.FindWithTag("Player").transform;
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
        // 카메라의 Field of View, Follow OffSet 변경
        virtualCamera.m_Lens.FieldOfView = _zoomFOV[_zoomLevel];
        cmTransposer.m_FollowOffset = _followOffsets[_zoomLevel];
    }

    void OnZoomStateChanged()
    {
        switch (zoomState)
        {
            case Enum_ZoomTypes.Default:                
                break;
            case Enum_ZoomTypes.DialogZoom:
                Vector3 dialogZoomPos = CalculateDialogZoomPos();
                transform.position = dialogZoomPos;
                transform.LookAt(midPos);
                break;
        }
    }

    private Vector3 CalculateDialogZoomPos()
    {
        float distance = 15f;

        // 캐릭터와 Npc의 중점
        midPos = (player.transform.position + NpcPos) / 2.0f;
        // 캐릭터에서 Npc로 향하는 벡터
        Vector3 forward = (player.transform.position - NpcPos).normalized;

        // 캐릭터의 우측 방향 벡터 (캐릭터가 바라보는 방향과 하늘 방향의 외적)
        Vector3 rightDirection = Vector3.Cross(forward, Vector3.up).normalized;
        // 캐릭터의 우측과 하늘 사이 45도 방향 벡터 계산
        Vector3 direction45 = (rightDirection + Vector3.up).normalized;

        // 목표 위치 계산
        Vector3 targetPosition = midPos + direction45 * distance;
        return targetPosition;
    }
}