using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    private static float X_VELOCITY = 15.0f;
    private static string PLAYER_TAG = "Player";
    private static string AI_TAG = "AI";
    private static float BULLET_LIFETIME = 5.0f;
    private static float BULLET_DAMAGE = 20.0f;
    private static Vector3 ROTATION_DIRECTION = new Vector3(0.0f, 0.0f, 90.0f);

    public bool ownerIsLeft;
    private Rigidbody2D body2d;
    private float currentLife;

    // Start is called before the first frame update
    void Start()
    {
        this.body2d = this.GetComponent<Rigidbody2D>();
        currentLife = BULLET_LIFETIME;

        if(ownerIsLeft)
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
        if (collision.collider.CompareTag(PLAYER_TAG) ||
            collision.collider.CompareTag(AI_TAG))
        {
            manageCollision(collision);
        }          
    }

    private void manageCollision (Collision2D collision)
    {
        PlayerPaddle script = collision.collider
               .GetComponent<PlayerPaddle>();
        if (script.isLeft != ownerIsLeft)
        {
            destroyBullet();
            collision.collider
                .gameObject.SendMessage(
                "damageShield", BULLET_DAMAGE, SendMessageOptions.RequireReceiver);
        }
    }

    private void destroyBullet()
    {
        Destroy(this.gameObject);
    }

    public void setOwner(bool isPlayer)
    {
        this.ownerIsLeft = isPlayer;
    }
}
