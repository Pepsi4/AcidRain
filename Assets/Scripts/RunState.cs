using UnityEngine;

public class RunState : IPlayerState
{
    private readonly int RunHash = Animator.StringToHash("Run");

    public void Handle(PlayerController player)
    {
        // Отримуємо інпут руху з джойстика
        float horizontal = player.GetMoveInput().x;
        float vertical = player.GetMoveInput().y;

        // Якщо інпут руху відсутній, змінюємо стан на Idle
        if (Mathf.Abs(horizontal) < 0.1f && Mathf.Abs(vertical) < 0.1f)
        {
            player.SetState(new IdleState());  // Повертаємось до стану Idle
        }
    }

    public void Enter(PlayerController player)
    {
        player.PlayAnimation(RunHash);  // Відтворюємо анімацію бігу
    }

    public void Exit(PlayerController player)
    {
        // Не потрібно нічого робити на виході, залишаємо без змін
    }
}
