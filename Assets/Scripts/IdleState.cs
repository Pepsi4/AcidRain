using UnityEngine;

public class IdleState : IPlayerState
{
    private readonly int IdleHash = Animator.StringToHash("Idle");

    public void Handle(PlayerController player)
    {
        // Отримуємо інпут руху з джойстика
        float horizontal = player.GetMoveInput().x;
        float vertical = player.GetMoveInput().y;

        // Якщо рухаємось, змінюємо стан на Run
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            player.SetState(new RunState());  // Змінюємо стан на Run
        }
    }

    public void Enter(PlayerController player)
    {
        player.PlayAnimation(IdleHash);  // Відтворюємо анімацію стояння
    }

    public void Exit(PlayerController player)
    {
        // Не потрібно нічого робити на виході, залишаємо без змін
    }
}
