using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Food : MonoBehaviour
{
    public UnityEvent foodEatenEvent = new UnityEvent();
    // Start is called before the first frame update

    private void OnDestroy()
    {
        foodEatenEvent.Invoke();
    }
}
