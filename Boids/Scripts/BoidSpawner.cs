using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject boidPrefab;
    [SerializeField]
    private float radius;
    [SerializeField]
    private float ammount;

    public float alignmentRadius;
    public float cohesionRadius;
    public float seperationRadius;
    public float seperationForse;

    public int found;
    public Vector3 average = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < ammount; ++i)
        {
            Instantiate(boidPrefab, this.transform.position + Random.insideUnitSphere * radius, Random.rotation);
        }
    }
}
