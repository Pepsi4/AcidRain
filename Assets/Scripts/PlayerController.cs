﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private IPlayerState currentState;
    private Animator animator;
    public VirtualJoystick joystick;
    public float rotationSpeed = 10f;
    private Vector3 moveDirection;
    private Collider collider;
    [field: SerializeField] public float BaseMoveSpeed { get; private set; } = 5f;
    private float currentSpeed;
    [field: SerializeField] public PowerUpMediator PowerUpMediator { get; private set; }
    [field: SerializeField] public PlayerHealth PlayerHealth { get; private set; }
    private bool canMove = true;
    void Start()
    {
        currentSpeed = BaseMoveSpeed;
        collider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        SetState(new IdleState());
    }

    void Update()
    {
        currentState.Handle(this);

        HandleRotation();
        HandleMovement();
    }

    public void SetState(IPlayerState newState)
    {
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
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
        if (canMove == false) return;

        float horizontal = GetMoveInput().x;
        float vertical = GetMoveInput().y;

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            moveDirection = new Vector3(horizontal, 0, vertical).normalized;

            Quaternion toRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void HandleMovement()
    {
        if (canMove == false) return;
        Vector3 direction = new Vector3(GetMoveInput().x, 0, GetMoveInput().y).normalized;
        if (direction.magnitude >= 0.01f)
        {
            transform.Translate(direction * currentSpeed * Time.deltaTime, Space.World);
        }
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void DisableCollider()
    {
        collider.enabled = false;
    }

    private List<float> speedModifiers = new List<float> { 1f }; // Базовий коефіцієнт

    public void AddSpeedModifier(float factor)
    {
        speedModifiers.Add(factor);
        UpdateSpeed();
    }

    public void RemoveSpeedModifier(float factor)
    {
        speedModifiers.Remove(speedModifiers.First(x => x == factor));
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        float totalFactor = 1f;
        foreach (float factor in speedModifiers)
        {
            totalFactor *= factor;
        }
        currentSpeed = BaseMoveSpeed * totalFactor;
    }

}
