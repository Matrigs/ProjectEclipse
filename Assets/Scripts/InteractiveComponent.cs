using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveComponent : MonoBehaviour
{
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }

    public virtual void Action()
    {
        Debug.Log("This is a generic shit");
    }
    
}
