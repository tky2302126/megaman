using UnityEngine;

[CreateAssetMenu(fileName = "PlayerModel", menuName = "Game/PlayerModel")]
public class PlayerModel : ScriptableObject
{
    [Header("ステータス")]
    public int maxHp = 5;
    [HideInInspector] public int currentHp;

    public float moveDuration = 0.15f;
    public float fireCooldown = 0.5f;

    public void Init()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
    }

    public bool IsDead => currentHp <= 0;
}
