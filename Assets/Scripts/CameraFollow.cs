using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // 따라갈 대상 (Goose)
    public float smoothSpeed = 0.125f;
    public float fixedY = 0f; // 카메라의 Y 위치를 고정
    public Vector3 offset;     // 카메라 위치 오프셋

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(
            target.position.x + offset.x, 
            fixedY, 
            transform.position.z
            );
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
