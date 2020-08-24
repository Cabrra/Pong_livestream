using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPaddle : MonoBehaviour {

    //Game variables

    //Movement
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDowwn = KeyCode.S;
    public float speed = 10.0f; // the speed that a paddle can move per frame: pixels per frame
    public float boundY = 5.01f; //the height of the object
    public Rigidbody2D body2d; // will be defined on the start() method


    // Start is called before the first frame update
    void Start()
    {
        /*
         * this will get the rigidbody from the object this script is attached to.
         * If we had more than one rigidbody we would need to specify the object.
         */
        body2d = GetComponent<Rigidbody2D>(); 
    }

    // Update is called once per frame
    void Update(){
        //rigidbody has a lot of cool members such as posdition and velocity
        var velocity = body2d.velocity;
        //are we pressing down?
        if (Input.GetKey(moveDowwn)) {
            //move down
            velocity.y = -speed;
        } else if (Input.GetKey(moveUp)){
            //move up
            velocity.y = speed;
        } else{
            velocity.y = 0;
        }

        body2d.velocity = velocity;

        //move the paddle and check bounds - this may need some testing
        //get the object's position
        var position = body2d.position;

        if (position.y > boundY) {
            //if the object is outside the bound then snap back to the boundY
            position.y = boundY;
        } else if (position.y < -boundY) {
            position.y = -boundY;
        }

        //transform is this.transform the transform matrix rom this object.
        //updated for clarity
        this.transform.position = position;
    }
}
