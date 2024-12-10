using UnityEngine;

public class DeadState : IPlayerState
{
    private readonly int DeadHash = Animator.StringToHash("Die");

    public void Handle(PlayerController player)
    {
        // Нічого не робимо, гравець у стані смерті
    }

    public void Enter(PlayerController player)
    {
        Debug.Log("Player entered Dead state.");

        // Відтворення анімації смерті
        player.PlayAnimation(DeadHash);
        player.DisableCollider();
        player.DisableMovement();
    }

    public void Exit(PlayerController player)
    {
        // Нічого не робимо на виході зі стану Dead
    }
}
