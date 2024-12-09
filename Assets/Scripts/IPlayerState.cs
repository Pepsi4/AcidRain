public interface IPlayerState
{
    void Handle(PlayerController player);  // Обробка логіки стану
    void Enter(PlayerController player);   // Викликається при вході в стан
    void Exit(PlayerController player);    // Викликається при виході зі стану
}
