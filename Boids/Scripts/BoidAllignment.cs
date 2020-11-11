using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Boid))]
public class BoidAllignment : MonoBehaviour
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

        foreach (var boid in boids.Where(b => b != boid))
        {
            diff = boid.transform.position - this.transform.position;
            if (diff.magnitude < radius)
            {
                average += boid.velocity;
                found += 1;
            }
        }
        if (found > 0)
        {
            average = average / found;
            boid.velocity += Vector3.Lerp(boid.velocity, average, Time.deltaTime);
        }
    }
}
