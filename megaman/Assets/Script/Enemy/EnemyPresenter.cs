

using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyPresenter : MonoBehaviour, IDamageable
{
    public float moveInterval = 1.5f;
    public float shootInterval = 2.5f;
    public Transform[] lanePositions;
    public float[] laneYPositions = { 1.0f, 0f, -1.0f };

    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    // modelに移管予定
    public int maxHp = 5;
    public int currentHp;

    void Start()
    {
        currentHp = maxHp;
        StartAI().Forget();
    }

    // 単純なAI
    private async UniTaskVoid StartAI()
    {
        while (true)
        {
            await UniTask.Delay(System.TimeSpan.FromSeconds(moveInterval));
            MoveToRandomLane();

            await UniTask.Delay(System.TimeSpan.FromSeconds(shootInterval));
            Shoot();
        }
    }

    private void MoveToRandomLane()
    {
        int index = Random.Range(0, laneYPositions.Length);
        Vector3 newPosition = new Vector3(transform.position.x, laneYPositions[index], 0);
        transform.position = newPosition;
    }

    private void Shoot()
    {
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        var bulletScript = bullet.GetComponent<Bullet>();
        if( bulletScript != null)
        {
            bulletScript.SetDirection(Vector2.left);
            // 弾のスプライトを左右反転させる
            bullet.GetComponent<SpriteRenderer>().flipX = true;
        }
    }


    public void TakeDamage(int damage)
    {
        currentHp -= damage;

        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("Enemy Died");
        Destroy(gameObject);
    }
}
