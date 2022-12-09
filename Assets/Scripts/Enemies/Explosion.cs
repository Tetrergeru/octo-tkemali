using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particles;

    void Start()
    {
        StartCoroutine(DieOverLifetime(_particles.main.duration));
    }

    IEnumerator DieOverLifetime(float lifetime)
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(this.gameObject);
    }
}
