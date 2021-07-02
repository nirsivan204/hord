using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calculator : MonoBehaviour
{
    public Sheep[] sheeps;
    public Vector3 flockVelocity;
    public Vector3 flockAveragePos;
    public Quaternion flockRotation;
    public Vector3 foodLocation;
    public bool isThereFood;
    public float calc_period = 0.05f;
    public shepherd player_shepered;
    private void Start()
    {
        InvokeRepeating("calculate",0, calc_period);
        player_shepered.foodEvent.AddListener(addFoodLocation);
    }

    public void calculate()
    {
        Vector3 avg_res = Vector3.zero;
        Vector3 vel_res = Vector3.zero;
        foreach ( Sheep sheep in sheeps)
        {
            avg_res += sheep.transform.position;
            vel_res += sheep.getVelocity();

        }
        flockAveragePos = avg_res/ sheeps.Length;
        flockVelocity = vel_res/sheeps.Length;
//        Debug.DrawLine(flockAveragePos,flockVelocity,Color.red,10);

    }

    public void addFoodLocation(Vector3 pos)
    {
        foodLocation = pos;
        isThereFood = true;
    }
}
