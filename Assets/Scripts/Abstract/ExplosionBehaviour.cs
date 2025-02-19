using UnityEngine;

public abstract class ExplosionBehaviour : MonoBehaviour
{
    //Life spans
    private float hitBoxLifeSpan = 0.5f; //for damage calc
    private float lifeSpan = 3f; //for visuals and Sound
    //Damage value
    public float Damage { get; protected set; }

    //Collider values
    protected SphereCollider explosionCollider;
    private bool isColliderDeleted = false;

    protected virtual void Awake()
    {
        explosionCollider = GetComponent<SphereCollider>();
    }

    protected virtual void Update()
    {
        hitBoxLifeSpan -= Time.deltaTime;
        lifeSpan -= Time.deltaTime;

        if (!isColliderDeleted && hitBoxLifeSpan <= 0f) //Delete collider if nothing was hit and after lifespan
        {
            isColliderDeleted = true;
            Destroy(explosionCollider);
        }

        if (lifeSpan <= 0f) //Destroys itself after lifespan ends
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Delete collider if something was hit
        isColliderDeleted = true;
        Destroy(explosionCollider);
    }
}
