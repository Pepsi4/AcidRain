using UnityEngine;

public class DeadState : IPlayerState
{
    private readonly int DeadHash = Animator.StringToHash("Die");

    public void Handle(PlayerController player) { }

    public void Enter(PlayerController player)
    {
        player.PlayAnimation(DeadHash);
        player.DisableCollider();
        player.DisableMovement();
    }

    public void Exit(PlayerController player) { }
}
