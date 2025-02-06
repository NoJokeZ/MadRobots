using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyState : MonoBehaviour
{
    Rigidbody rb;

    

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }


    protected virtual void FixedUpdate()
    {

    }
}
