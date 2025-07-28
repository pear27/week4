using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GooseTiedController : MonoBehaviour
{
    public Animator animator;            // Animator 참조 (Idle 등 상태 전환용)
    public SpriteRenderer spriteRenderer; // Sprite 바꿀 Renderer
    public Sprite tiedSprite;           // 이미지 1 (묶인 상태)
    public Sprite defaultPeckSprite;    // 이미지 2 (밧줄 풀기 기본)
    public Sprite peckSprite;           // 이미지 3 (쪼는 동작)

    public float mashDecayRate = 1f; // 초당 게이지 감소량
    public float mashIncrease = 10f; // 한 번 누를 때 증가량
    public float requiredMash = 100f; // 풀기 위해 필요한 게이지 양
    public float currentMash = 0f;

    private bool isTied = true;

    void Start()
    {
        if (animator != null)
            animator.enabled = false; // Animator 비활성화

        if (spriteRenderer != null)            
            spriteRenderer.sprite = tiedSprite; // 처음 상태 1번 이미지

        StartCoroutine(ChangeToPeckReady());
    }

    IEnumerator ChangeToPeckReady()
    {
        yield return new WaitForSeconds(2f); // 2초 유지
        spriteRenderer.sprite = defaultPeckSprite; // 2번 이미지로 변경
    }

    void Update()
    {
        if (!isTied)
            return; // 묶여있지 않으면 아무 동작도 하지 않음

        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentMash += mashIncrease; // 스페이스바 누를 때마다 게이지 증가
            StartCoroutine(PeckAnimation());
        }

        currentMash -= mashDecayRate * Time.deltaTime; // 게이지 감소
        currentMash = Mathf.Clamp(currentMash, 0, requiredMash); // 게이지 범위 제한
        
        if (currentMash >= requiredMash)
            ReleaseGoose();   
    }

    IEnumerator PeckAnimation()
    {
        spriteRenderer.sprite = peckSprite;
        yield return new WaitForSeconds(0.1f); // 쪼는 동작 지속
        spriteRenderer.sprite = defaultPeckSprite;
    }

    void ReleaseGoose()
    {
        isTied = false;
        if (animator != null)
            animator.enabled = true; // Animator 활성화
       
        Debug.Log("Goose is free!");
    }
}
