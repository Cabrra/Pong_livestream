using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private static float Y_DIRECTION = 15.0f;
    private static float X_DIRECTION = 15.0f;
    private static float PREFIOUS_POSITION_X;
    private static float BUFFER_TIMER = 4.0f;

    public Rigidbody2D body2d;
    private float bufferTimer;

    //helper function to define the start moving direction
    void startMoving()
    {
        //Range defines a ramge from var1 to var2-1 in this case between (0 and 2-1)
        float rand = Random.Range(0, 2);
        float randX = Random.Range(0, X_DIRECTION) + 4.0f;
        float randY = Random.Range(0, Y_DIRECTION) + 4.0f;
        if(rand < 1)
        {
            this.body2d.AddForce(new Vector2(-randX, randY));
        } else {
            //opposite direction
            this.body2d.AddForce(new Vector2(randX, -randY));
        }
    }

    void resetBall()
    {
        //method to reset the ball to initial values
        this.body2d.velocity = Vector2.zero; // this is the vector with X and Y zero
        this.transform.position = Vector2.zero;

        PREFIOUS_POSITION_X = 0;
        bufferTimer = BUFFER_TIMER;
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

    void Update()
    {
        //get the time that passed between this frame and teh last one
        float dt = Time.deltaTime;
        //verify that the ball isn't stuck stuck
        if(bufferTimer <= 0)
        {
            if(this.transform.position.x == PREFIOUS_POSITION_X ||
                Mathf.Abs(this.body2d.velocity.x) < 0.5f)
            {
                //the ball is stuck - speed is all Y
                this.body2d.AddForce(new Vector2(X_DIRECTION, 0));
            }
        } else
        {
            bufferTimer -= dt;
        }
    }
}
