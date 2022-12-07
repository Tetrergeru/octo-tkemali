using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody Body;
    [SerializeField] private GameObject ExplosionPrefab;
    [SerializeField] private float _damage = 70;
    [SerializeField] private float _speed = 15;

    void Start()
    {
        Body.velocity = transform.forward * _speed;
        StartCoroutine(DieOverLifetime(5));
    }

    IEnumerator DieOverLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Die();
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision enter");
        var player = collision.gameObject.GetComponent<PlayerAttributes>();
        if (player != null)
        {
            player.Health -= _damage;
        }
        Die();
    }

    void Die()
    {
        Instantiate(ExplosionPrefab, this.transform.position, new Quaternion());
        Destroy(this.gameObject);
    }
}
