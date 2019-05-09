using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    public int BulletDamage;
    
    public int KnockBackAmount = 25;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        Enemy enemyScript = collision.gameObject.GetComponent<Enemy>();
        if (enemyScript == null) return;
        enemyScript.Damage(BulletDamage);
        //enemyScript._pathFollower.AddOffset(collision.GetContact(0).normal, KnockBackAmount);
        enemyScript.Rigidbody.AddForce(-collision.GetContact(0).normal* KnockBackAmount);
        Destroy(gameObject);
    }
}
