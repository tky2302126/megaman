using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;

    void Start()
    {
        // 一定時間で自動破棄
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // 右方向に進む
        transform.Translate(Vector2.right * speed * Time.deltaTime);
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
