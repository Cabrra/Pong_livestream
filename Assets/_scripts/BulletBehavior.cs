using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private static float X_VELOCITY = 10.0f;
    private static string PLAYER_TAG = "Player";
    private static string AI_TAG = "AI";
    private static float BULLET_LIFETIME = 5.0f;
    private static float BULLET_DAMAGE = 30.0f;
    private static Vector3 ROTATION_DIRECTION = new Vector3(0.0f, 0.0f, 90.0f);

    public bool ownerIsPlayer;
    private Rigidbody2D body2d;
    private float currentLife;

    // Start is called before the first frame update
    void Start()
    {
        this.body2d = this.GetComponent<Rigidbody2D>();
        currentLife = BULLET_LIFETIME;

        if(ownerIsPlayer)
        {
            this.body2d.velocity = new Vector2(X_VELOCITY, 0);
        } 
        else
        {
            this.body2d.velocity = new Vector2(-X_VELOCITY, 0);
        }
    }

    private void Update()
    {
        //lifecycle
        float dt = Time.deltaTime;
        if (currentLife <= 0)
        {
            destroyBullet();
        }

        currentLife -= dt;

        //rotation
        Vector3 rotation = ROTATION_DIRECTION * dt;
        this.transform.Rotate(rotation);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(PLAYER_TAG))
        {
            if(!ownerIsPlayer)
            //{
            //    //destroy the bullet
            //    destroyBullet();
            //}
            //else
            {
                collision.collider
                    .gameObject.SendMessage(
                    "damageShield", BULLET_DAMAGE, SendMessageOptions.RequireReceiver);
                destroyBullet();
            }
        }
        else if (collision.collider.CompareTag(AI_TAG))
        {
            if (ownerIsPlayer)
            {
                //damage shield
                collision.collider
                    .gameObject.SendMessage(
                    "damageShield", BULLET_DAMAGE, SendMessageOptions.RequireReceiver);
                destroyBullet();
            }
            //else
            //{
            //    //destroy the bullet
            //    destroyBullet();
            //}
        }
    }

    private void destroyBullet()
    {
        Destroy(this.gameObject);
    }

    public void setOwner(bool isPlayer)
    {
        this.ownerIsPlayer = isPlayer;
    }
}
