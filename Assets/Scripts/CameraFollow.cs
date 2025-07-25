using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // 따라갈 대상 (Goose)
    public float smoothSpeed = 0.125f;
    public Vector3 offset;     // 카메라 위치 오프셋

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
