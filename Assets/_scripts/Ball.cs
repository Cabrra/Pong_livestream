using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private static float Y_DIRECTION = 15.0f;

    public Rigidbody2D body2d;

    //helper function to define the start moving direction
    void startMoving()
    {
        //Range defines a ramge from var1 to var2-1 in this case between (0 and 2-1)
        float rand = Random.Range(0, 2);
        if(rand < 1)
        {
            this.body2d.AddForce(new Vector2(15.0f, -Y_DIRECTION));
        } else {
            //opposite direction
            this.body2d.AddForce(new Vector2(-15.0f, -Y_DIRECTION));
        }
    }

    void resetBall()
    {
        //method to reset the ball to initial values
        this.body2d.velocity = Vector2.zero; // this is the vector with X and Y zero
        this.transform.position = Vector2.zero;

    }

    void restartGame()
    {
        resetBall();
        Invoke("startMoving", 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        this.body2d = GetComponent<Rigidbody2D>();
        //new stuff Invoke method
        Invoke("startMoving", 2);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.collider.CompareTag("Player")){
            Vector2 velocity = Vector2.zero;
            velocity.x = this.body2d.velocity.x;
            velocity.y = body2d.velocity.y + 
                (collision.collider.attachedRigidbody.velocity.y * 0.1f);
            this.body2d.velocity = velocity;
        }
    }
}
