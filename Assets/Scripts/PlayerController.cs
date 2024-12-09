using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private IPlayerState currentState;
    private Animator animator;
    public VirtualJoystick joystick;
    // Для обертання персонажа
    public float rotationSpeed = 10f;
    private Vector3 moveDirection;

    // Для переміщення персонажа
    [field: SerializeField]public float MoveSpeed { get; set; } = 5f;  // Швидкість руху персонажа

    void Start()
    {
        animator = GetComponent<Animator>();
        SetState(new IdleState()); // Початковий стан - Idle
    }

    void Update()
    {
        currentState.Handle(this);

        // Якщо персонаж рухається, обертати в залежності від напрямку
        HandleRotation();
        HandleMovement();
    }

    public void SetState(IPlayerState newState)
    {
        currentState?.Exit(this); // Виходимо з попереднього стану
        currentState = newState;   // Встановлюємо новий стан
        currentState.Enter(this);   // Заходимо в новий стан
    }

    public void PlayAnimation(int animationHash)
    {
        animator.Play(animationHash);
    }

    public Vector2 GetMoveInput()
    {
        #if UNITY_STANDALONE || UNITY_EDITOR
                return new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        #endif
        return new Vector2(joystick.Horizontal, joystick.Vertical);
    }

    private void HandleRotation()
    {
        float horizontal = GetMoveInput().x;
        float vertical = GetMoveInput().y;

        // Якщо є рух, обертати персонажа в напрямку руху
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            moveDirection = new Vector3(horizontal, 0, vertical).normalized;

            // Обертання персонажа
            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void HandleMovement()
    {
        // Отримуємо напрямок руху
        Vector3 direction = new Vector3(GetMoveInput().x, 0, GetMoveInput().y).normalized;
        // Якщо є інпут, рухаємо персонажа
        if (direction.magnitude >= 0.01f)
        {
            // Рух персонажа вперед
            transform.Translate(direction * MoveSpeed * Time.deltaTime, Space.World);
        }
    }
}
