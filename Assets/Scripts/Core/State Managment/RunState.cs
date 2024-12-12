using UnityEngine;

public class RunState : IPlayerState
{
    private readonly int RunHash = Animator.StringToHash("Run");

    public void Handle(PlayerController player)
    {
        float horizontal = player.GetMoveInput().x;
        float vertical = player.GetMoveInput().y;

        if (Mathf.Abs(horizontal) < 0.1f && Mathf.Abs(vertical) < 0.1f)
        {
            player.SetState(new IdleState());
        }
    }

    public void Enter(PlayerController player)
    {
        player.PlayAnimation(RunHash);
    }

    public void Exit(PlayerController player) { }
}
