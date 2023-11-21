using System.Collections;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    [SerializeField] private float lifetime;
    void Start()
    {
        StartCoroutine(DestroyDelay()); //Cleans up particles
    }
    IEnumerator DestroyDelay()
        {
            yield return new WaitForSeconds(lifetime);
            Destroy(gameObject);
        }
    }
