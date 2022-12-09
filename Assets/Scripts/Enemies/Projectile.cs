using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody Body;
    [SerializeField] private GameObject ExplosionPrefab;
    [SerializeField] private float _damage = 70;
    [SerializeField] private float _speed = 15;

    private bool _dead = false;
    private Coroutine _coroutine;

    void Start()
    {
        Body.velocity = transform.forward * _speed;
        _coroutine = StartCoroutine(DieOverLifetime(5));
    }

    IEnumerator DieOverLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Die();
    }

    void OnCollisionEnter(Collision collision)
    {
        var player = collision.gameObject.GetComponent<PlayerAttributes>();
        if (player != null)
        {
            player.Health -= _damage;
        }
        this.StopCoroutine(_coroutine);
        Die();
    }

    void Die()
    {
        if (_dead) return;

        _dead = true;
        Instantiate(ExplosionPrefab, this.transform.position, new Quaternion());
        Destroy(this.gameObject);
    }
}
