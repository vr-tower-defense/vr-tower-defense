using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    public float BulletDamage;
    public float TimeBeforeDestroy = 3;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, TimeBeforeDestroy);
    }

    void OnCollisionEnter(Collision collision)
    {
        Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
        enemyScript?.Damage(BulletDamage);

        Destroy(gameObject);
    }
}
