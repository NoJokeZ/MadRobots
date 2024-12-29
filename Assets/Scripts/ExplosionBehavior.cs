using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehavior : MonoBehaviour
{
    private float lifeSpan = 3f;
    private int damage = 5;


    private void Update()
    {
        lifeSpan -= Time.deltaTime;
        
        if (lifeSpan <= 0f )
        {
            Destroy(gameObject);
        }
    }
}
