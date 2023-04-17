using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float ExplosionDelay = 5f;
    public GameObject ExplosionPrefab;
    public float blastRadius = 2f;
    public int blastDamage = 5;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplosionCoroutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator ExplosionCoroutine()
    {
        // Wait
        yield return new WaitForSeconds(ExplosionDelay);

        // Explode
        Explode();
    }

    private void Explode()
    {
        //Create Explosion
        var explosion = Instantiate(ExplosionPrefab, transform.position, ExplosionPrefab.transform.rotation);

        // Destroy platforms
        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach (Collider collider in colliders)
        {
            GameObject hittenObject = collider.gameObject;
            if (hittenObject.CompareTag("Platform"))
            {
                LifeScript lifeScript = hittenObject.GetComponent<LifeScript>();
                if (lifeScript != null)
                {
                    float distance = (hittenObject.transform.position - transform.position).magnitude;
                    float distanceRate = Mathf.Clamp(distance / blastRadius, 0.0f, 1.0f);
                    float damageRate = 1f - Mathf.Pow(distanceRate, 4);
                    int damage = Mathf.CeilToInt(damageRate * blastDamage);
                    lifeScript.health -= damage;
                    if (lifeScript.health <= 0)
                    {
                        Destroy(hittenObject);

                    }
                }
            }
        }

        // Create SFX

        //Destroy Bomb

        Destroy(gameObject);
        Destroy(explosion, 3.1f);
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, blastRadius);
    }
}
