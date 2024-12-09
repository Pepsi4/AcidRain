using UnityEngine;

public class Raindrop : MonoBehaviour
{
    private float fallSpeed;
    private System.Action<Raindrop> returnToPool;

    public void Initialize(float speed, System.Action<Raindrop> onReturn)
    {
        fallSpeed = speed;
        returnToPool = onReturn;
    }

    private void Update()
    {
        transform.position += Vector3.down * fallSpeed * Time.deltaTime;

        if (transform.position.y < -10f)
        {
            returnToPool?.Invoke(this);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            returnToPool?.Invoke(this);
        }
        else if (collision.gameObject.CompareTag("Shield"))
        {
            Vector3 slideDirection = Vector3.ProjectOnPlane(Vector3.down, collision.contacts[0].normal).normalized;
            GetComponent<Rigidbody>().linearVelocity = slideDirection * fallSpeed;
        }
    }
}
