using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float ExplosionDelay = 5f;
    public GameObject ExplosionPrefab;
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

        // Create SFX

        //Destroy Bomb

        Destroy(gameObject);
        Destroy(explosion, 3.1f);
    }
}
