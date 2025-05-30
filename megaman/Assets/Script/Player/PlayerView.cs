using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void PlayIdle() => animator.Play("Idle");
    public void PlayMove() => animator.Play("Move");
    public void PlayAttack() => animator.SetTrigger("Attack");

    public void MoveTo(Vector2 pos)
    {
        transform.position = pos;
    }

    public Vector2 GetPosition() => transform.position;
}
