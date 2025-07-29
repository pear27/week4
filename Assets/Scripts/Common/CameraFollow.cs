using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // ���� ��� (Goose)
    public float smoothSpeed = 0.125f;
    public float fixedY = 0f; // ī�޶��� Y ��ġ�� ����
    public Vector3 offset;     // ī�޶� ��ġ ������

    public BoxCollider2D mapBounds; // ���� ��� �ڽ�

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
