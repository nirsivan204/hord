using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class shepherd : MonoBehaviour
{
    // Start is called before the first frame update
    private float movementX = 0;
    private float movementY = 0;
    private Rigidbody rb;
    public float speed = 10;
    public float radius = 2;
    public float repulse = 3;
    public GameObject food;
    public FoodCreatedEvent foodEvent = new FoodCreatedEvent();
    private bool isFoodExist = false;

    public class FoodCreatedEvent : UnityEvent<Vector3>
    {
    }
    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0, movementY);
        rb.AddForce(movement * speed);
        pushSheeps();
    }

    private void OnFire()
    {
        if (!isFoodExist)
        {
            GameObject clone = Instantiate(food);
            isFoodExist = true;
            clone.transform.position = transform.position + Vector3.forward;
            clone.GetComponent<Food>().foodEatenEvent.AddListener(foodEaten);
            foodEvent.Invoke(clone.transform.position);
        }

    }

    private void foodEaten()
    {
        isFoodExist = false;
    }

    private void pushSheeps()
    {
        Vector3 diff;
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < cols.Length; i++)
        {
            if (cols[i].CompareTag("sheep"))
            {
                diff = cols[i].transform.position - transform.position;
                cols[i].gameObject.GetComponent<Rigidbody>().AddForce(repulse * diff / diff.sqrMagnitude);
            }

        }
    }
}
