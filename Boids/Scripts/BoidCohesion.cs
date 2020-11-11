using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Boid))]
public class BoidCohesion : MonoBehaviour
{
    private Boid boid;
    public float radius;
    Vector3 average = Vector3.zero;
    Vector3 diff;

    // Start is called before the first frame update
    void Start()
    {
        boid = GetComponent<Boid>();
    }

    // Update is called once per frame
    void Update()
    {
        Boid[] boids = FindObjectsOfType<Boid>();

        int found = 0;

        foreach(var boid in boids.Where(b => b != boid))
        {
            diff = boid.transform.position - this.transform.position;
            if(diff.magnitude < radius)
            {
                average += diff;
                found += 1;
            }
        }
        if(found > 0)
        {
            average = average / found;
            boid.velocity += Vector3.Lerp(Vector3.zero, average, average.magnitude / radius); 
        }
    }
}
