using UnityEngine;

public class DelayedExplosion : MonoBehaviour
{
    [SerializeField] private float delay = 1.5f, radius = 3f, force = 5f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private bool displayRadius;

    [ExecuteInEditMode]
    private void OnDrawGizmos()
    {
        if (displayRadius)
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }

    void Update()
    {
        delay -= Time.deltaTime;

        if (delay <= 0) Explode();
    }


    private void Explode()
	{
        Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);

        Collider[] colToDestroy = Physics.OverlapSphere(transform.position, radius);

		foreach (Collider col in colToDestroy)
		{
            Destructible destructible = col.GetComponent<Destructible>();
            if (destructible != null)
			{
                destructible.Destroy();
            }
		}

        Collider[] colToMove = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in colToMove)
        {
            Rigidbody rb = col.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }

        Destroy(gameObject);
	}
}
