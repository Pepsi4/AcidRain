using UnityEngine;

public class IdleState : IPlayerState
{
    private readonly int IdleHash = Animator.StringToHash("Idle");

    public void Handle(PlayerController player)
    {
        float horizontal = player.GetMoveInput().x;
        float vertical = player.GetMoveInput().y;

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            player.SetState(new RunState());
        }
    }

    public void Enter(PlayerController player)
    {
        player.PlayAnimation(IdleHash);
    }

    public void Exit(PlayerController player) { }
}
