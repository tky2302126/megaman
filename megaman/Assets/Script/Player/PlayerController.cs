using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    [Header("レーン設定")]
    public float[] laneYPositions = { 1.5f, 0f, -1.5f };
    private int currentLane = 1;

    [Header("移動設定")]
    public float moveDuration = 0.15f;
    private bool isMoving = false;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = new Vector3(transform.position.x, laneYPositions[currentLane], transform.position.z);
    }

    private void Update()
    {
        HandleInput().Forget();
    }

    private async UniTaskVoid HandleInput()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.W) && currentLane > 0)
        {
            currentLane--;
            await MoveToLane(currentLane);
        }
        else if (Input.GetKeyDown(KeyCode.S) && currentLane < laneYPositions.Length - 1)
        {
            currentLane++;
            await MoveToLane(currentLane);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    private async UniTask MoveToLane(int laneIndex)
    {
        isMoving = true;
        animator?.SetTrigger("Move");

        Vector3 start = transform.position;
        Vector3 end = new Vector3(start.x, laneYPositions[laneIndex], start.z);

        float elapsed = 0f;
        while (elapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / moveDuration);
            elapsed += Time.deltaTime;
            await UniTask.Yield();
        }

        transform.position = end;
        isMoving = false;
    }

    private void Attack()
    {
        animator?.SetTrigger("Attack");
        // 弾発射処理はステップ3で追加
        Debug.Log("Attack!");
    }

    bool IsIdle()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("Idle");
    }
}
