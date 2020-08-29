using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior : MonoBehaviour
{
    private static string BALL_TAG = "Ball";
    private static float PADDLE_REACH = 1.0f;

    public float speed = 10.0f; // the speed that a paddle can move per frame: pixels per frame
    public float boundY = 5.01f; //the height of the object
    public Rigidbody2D body2d; // will be defined on the start() method
    GameObject theBall;

    // Start is called before the first frame update
    void Start()
    {
        this.theBall = GameObject.FindGameObjectWithTag(BALL_TAG);
    }

    // Update is called once per frame
    void Update()
    {
        //compare where is the AI paddle position and move accordingly
        float ballPosY = this.theBall.transform.position.y;
        var velocity = body2d.velocity;
        float distanceY = ballPosY - this.transform.position.y;

        if (distanceY > PADDLE_REACH)
        {
            //move up
            //using player's method
            velocity.y = this.speed;

        } else if (distanceY < -PADDLE_REACH)
        {
            //move down
            velocity.y = -this.speed ;

        } else
        {
            velocity.y = 0;
        }

        body2d.velocity = velocity;

        //move the paddle and check bounds - this may need some testing
        //get the object's position
        var position = body2d.position;

        if (position.y > boundY)
        {
            //if the object is outside the bound then snap back to the boundY
            position.y = boundY;
        }
        else if (position.y < -boundY)
        {
            position.y = -boundY;
        }

        //transform is this.transform the transform matrix rom this object.
        //updated for clarity
        this.transform.position = position;


    }
}
