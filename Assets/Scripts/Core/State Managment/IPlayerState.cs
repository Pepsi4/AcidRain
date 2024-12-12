public interface IPlayerState
{
    void Handle(PlayerController player);
    void Enter(PlayerController player);
    void Exit(PlayerController player);
}
