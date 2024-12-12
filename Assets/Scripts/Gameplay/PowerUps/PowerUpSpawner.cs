using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private PowerUpConfig powerUpConfig;
    [SerializeField] private ObjectPool<PowerUp> powerUpPool;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PowerUpMediator powerUpMediator;

    private void Start()
    {
        powerUpPool = new ObjectPool<PowerUp>(
            CreatePowerUp,
            OnTakePowerUp,
            OnReleasePowerUp,
            OnDestroyPowerUp,
            false
        );

        StartCoroutine(StartSpawner());
    }

    private IEnumerator StartSpawner()
    {
        yield return new WaitForSeconds(powerUpConfig.SpawnInterval);
        SpawnPowerUp();
        StartCoroutine(StartSpawner());
    }

    private void SpawnPowerUp()
    {
        Vector2 randomCircle = Helper.RandomPointInAnnulus(new Vector2(playerTransform.position.x, playerTransform.position.z), powerUpConfig.MinDistance, powerUpConfig.MaxDistance);

        Vector3 spawnPosition = new Vector3(randomCircle.x, powerUpConfig.SpawnY, randomCircle.y);

        PowerUp powerUp = powerUpPool.Get();
        powerUp.transform.position = spawnPosition;
    }

    private PowerUp CreatePowerUp()
    {
        return Instantiate(powerUpConfig.PowerUpPrefab, this.transform).Initialize(powerUpMediator, powerUpConfig ,ReleaseToPool);
    }
    private void ReleaseToPool(PowerUp powerUp)
    {
        powerUpPool.Release(powerUp);
    }

    private void OnTakePowerUp(PowerUp powerUp)
    {
        powerUp.gameObject.SetActive(true);
    }

    private void OnReleasePowerUp(PowerUp powerUp)
    {
        powerUp.gameObject.SetActive(false);
    }

    private void OnDestroyPowerUp(PowerUp powerUp)
    {
        Destroy(powerUp);
    }
}
