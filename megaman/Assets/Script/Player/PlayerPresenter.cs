using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerPresenter : MonoBehaviour, IDamageable
{
    [Header("レーン設定")]
    public float[] laneYPositions = { 1.5f, 0f, -1.5f };
    private int currentLane = 1;

    [Header("移動設定")]
    public float moveDuration = 0.15f;
    private bool isMoving = false;

    public GameObject bulletPrefab; // Inspectorから指定
    public Transform firePoint;     // 弾の発射位置

    public float fireCooldown = 0.5f;
    private float fireTimer = 0.0f;

    // modelに移管予定
    public int maxHp = 5;
    private int currentHp;

    private Animator animator;

    [SerializeField] private PlayerUI playerUI;

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = new Vector3(transform.position.x, laneYPositions[currentLane], transform.position.z);
        currentHp = maxHp;
        playerUI = FindAnyObjectByType<PlayerUI>();
        playerUI.SetHP(currentHp, maxHp);
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;
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

        if (Input.GetKeyDown(KeyCode.Space) && fireTimer <= 0f)
        {
            Attack();
            fireTimer = fireCooldown;
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

        Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Debug.Log("Attack!");
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        playerUI.SetHP(currentHp, maxHp);

        if(currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("YOU LOSE");
        // 今後：リトライやリザルト画面へ遷移など
        Destroy(gameObject);
    }

    bool IsIdle()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsName("Idle");
    }
}
