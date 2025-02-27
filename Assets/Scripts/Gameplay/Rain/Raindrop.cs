﻿using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Raindrop : MonoBehaviour
{
    public int Index { get; private set; }

    private int damage = 1;
    private RainJobsSystem rainJobsSystem;
    private bool physicsMovement = false;
    private Rigidbody rigidbody;
    private Collider collider;
    private const float DisablePhysicsDelay = 10f;

    public void Initialize(RainJobsSystem system, int damage, int index)
    {
        rainJobsSystem = system;
        this.damage = damage;
        Index = index;
    }

    public void EnablePhysics()
    {
        physicsMovement = true;
        rainJobsSystem.DisableRaindropMovementControll(Index);
        collider.isTrigger = false;
        if (rigidbody == null) rigidbody = this.AddComponent<Rigidbody>();
        StartCoroutine(DisablePhysicsWithDelay());
    }

    public void SetLinearVelocity(Vector3 velocity)
    {
        rigidbody.linearVelocity = velocity;
    }

    private void Awake()
    {
        collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(DamageTypes.Raindrop, damage);
            rainJobsSystem.ResetRaindrop(Index);
        }
    }

    private void DisablePhysics()
    {
        physicsMovement = false;
        rainJobsSystem.EnableRaindropMovementControll(Index);
        collider.isTrigger = true;
        Destroy(rigidbody);
    }

    private IEnumerator DisablePhysicsWithDelay()
    {
        yield return new WaitForSeconds(DisablePhysicsDelay);
        DisablePhysics();
    }
}
