using UnityEngine;
using Cysharp.Threading.Tasks;

public class PlayerPresenter : MonoBehaviour, IDamageable
{
    [Header("レーン設定")]
    public float[] laneYPositions = { 1.0f, 0f, -1.0f };
    private int currentLane = 1;

    private bool isMoving = false;

    public GameObject bulletPrefab; // Inspectorから指定
    public Transform firePoint;     // 弾の発射位置

    private float fireTimer = 0.0f;

    [SerializeField] private PlayerModel model;

    private int MaxHp => model.maxHp;
    private int CurrentHp => model.currentHp;
    private float MoveDuration => model.moveDuration;
    private float FireCooldown => model.fireCooldown;

    private Animator animator;

    [SerializeField] private PlayerUI playerUI;

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = new Vector3(transform.position.x, laneYPositions[currentLane], transform.position.z);
        model.Init();
        playerUI = FindAnyObjectByType<PlayerUI>();
        playerUI.SetHP(CurrentHp, MaxHp);
    }

    private void Update()
    {
        fireTimer -= Time.deltaTime;
        if(!isMoving)HandleInput();
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
            fireTimer = FireCooldown;
        }
    }

    private async UniTask MoveToLane(int laneIndex)
    {
        isMoving = true;
        animator?.SetTrigger("Move");

        Vector3 start = transform.position;
        Vector3 end = new Vector3(start.x, laneYPositions[laneIndex], start.z);

        float elapsed = 0f;
        while (elapsed < MoveDuration)
        {
            transform.position = Vector3.Lerp(start, end, elapsed / MoveDuration);
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
        model.TakeDamage(damage);
        playerUI.SetHP(CurrentHp, MaxHp);

        if(model.IsDead)
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
