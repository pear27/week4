using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;   // ���� ��� (Goose)
    public float smoothSpeed = 0.125f;
    public float fixedY = 0f; // ī�޶��� Y ��ġ�� ����
    public Vector3 offset;     // ī�޶� ��ġ ������

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
