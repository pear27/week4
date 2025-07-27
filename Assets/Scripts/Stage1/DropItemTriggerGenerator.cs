using UnityEngine;
using System.Collections;

public class DropItemTriggerGenerator : MonoBehaviour
{
    public Transform itemsParent; // �������� ������ �θ� ������Ʈ
    public float triggerOffsetY = -8f;  // ������ ��ġ 
    public Vector2 triggerSize = new Vector2(1f, 8f); // Ʈ������ ũ��
    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        Transform[] items = new Transform[itemsParent.childCount];
        for(int i = 0; i < itemsParent.childCount; i++)
        {
            items[i] = itemsParent.GetChild(i);
        }

        int randomIndex = Random.Range(0, items.Length);
        Transform chosenItem = items[randomIndex];
        
        CreateTrigger(chosenItem);
    }

    void CreateTrigger(Transform item)
    {
        GameObject trigger = new GameObject("DropItemTrigger_" + item.name);
        trigger.transform.position = new Vector3(item.position.x, item.position.y + triggerOffsetY, 0);

        trigger.transform.parent = itemsParent;

        // ������ �ʱ�ȭ 
        trigger.transform.localScale = Vector3.one;

        BoxCollider2D collider = trigger.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        
        StartCoroutine(ApplyColliderSize(collider));

        DropItemTrigger triggerScript = trigger.AddComponent<DropItemTrigger>();
        triggerScript.itemToDrop = item.GetComponent<Rigidbody2D>();
    }

    IEnumerator ApplyColliderSize(BoxCollider2D collider)
    {
        yield return null; // �� ������ ��� �� ����
        collider.size = triggerSize;
    }
}
