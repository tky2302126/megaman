using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;

    private Vector2 direction = Vector2.right;

    void Start()
    {
        // 一定時間で自動破棄
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 右方向に進む
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var target = collision.GetComponent<IDamageable>();
        if(target != null)
        {
            target.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
