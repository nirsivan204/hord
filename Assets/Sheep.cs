using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    public Calculator calc;
    public  Vector3 target;
    public Vector3 target_direction;
    private Rigidbody rb;
    private Collider myCol;
    public float speed = 5;
    public float maxSpeed = 5;
    public float RADIUS = 2;
    public float repulse = 2;
    public float vel_factor = 1;
    public float avg_factor = 1;
    public bool isControlled;
    public float min_speed_to_move = 2;
    public float random_factor = 5;
    public float foodFactor = 5;
    public float turn_min = 0.7f;
    public float turn_max = 0.7f;
    public float turn_factor = 0.5f;
    Vector3 prev_heading;
    private bool canTurn = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCol = GetComponent<Collider>();
        if (!isControlled)
        {
            InvokeRepeating("findNextTargetSpeed", 0, 0.1f);
            InvokeRepeating("enableTurn", 0, 0.1f);
            canTurn = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 velocity_diff = (target - rb.velocity);

        if ( velocity_diff.magnitude > min_speed_to_move)
        {
            rb.AddForce(velocity_diff.normalized * speed, ForceMode.VelocityChange);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
            if(canTurn)
            {
                prev_heading = new Vector3(rb.velocity.x, 0, rb.velocity.z);

                transform.LookAt(transform.position + prev_heading);
                canTurn = false;
            }
        }

    }

    private void enableTurn()
    {
        canTurn = true;
    }

    public Vector3 getVelocity()
    {
        return rb.velocity;
    }

    public void findNextTargetSpeed()
    {
        Vector3 res = FindRepulseDir() * repulse;
        res += calc.flockVelocity * vel_factor;
        res +=  (calc.flockAveragePos-transform.position ) * avg_factor;
        if (calc.isThereFood)
        {
             res += (calc.foodLocation - transform.position) * foodFactor;

        }
        target = res;
    }


    private Vector3 FindRepulseDir()
    {
        Vector3 res = Vector3.zero;
        Vector3 distance;
        Collider[] cols = Physics.OverlapSphere(transform.position, RADIUS);
        for(int i =0; i < cols.Length; i++)
        {
            if(cols[i] != myCol && cols[i].CompareTag("sheep"))
            {
                distance = transform.position - cols[i].transform.position;
                res += distance / distance.magnitude;
            }
            
        }
        return res;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("food"))
        {
            Destroy(collision.gameObject);
        }

    }
}
