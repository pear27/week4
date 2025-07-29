using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // 따라갈 대상 (Goose)
    public float smoothSpeed = 0.125f;
    public float fixedY = 0f; // 카메라의 Y 위치를 고정
    public Vector3 offset;     // 카메라 위치 오프셋

    public BoxCollider2D mapBounds; // 맵의 경계 박스

    private float minX, maxX;

    private float cameraHalfWidth;

    void Start()
    {
        Camera cam = Camera.main;
        cameraHalfWidth = cam.orthographicSize * cam.aspect;

        if (mapBounds != null)
        {
            minX = mapBounds.bounds.min.x + cameraHalfWidth;
            maxX = mapBounds.bounds.max.x - cameraHalfWidth;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        float targetX = target.position.x + offset.x;

        targetX = Mathf.Clamp(targetX, minX, maxX);

        Vector3 desiredPosition = new Vector3(targetX, fixedY, transform.position.z);
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
