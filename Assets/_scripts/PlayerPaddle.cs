using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPaddle : MonoBehaviour {

    //Game variables
    private static float BULLET_TIMER = 4.0f; // 4 seconds 
    private static float SHOOT_DELAY = 1.5f; // 1.5 seconds 
    private static int TOTAL_BULLETS = 5; // 5 bullets 
    private static string BULLET_UI_TEXT_TAG = "PlayerBullets";
    //Movement
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDowwn = KeyCode.S;
    public KeyCode shoot = KeyCode.Space;
    public float speed = 10.0f; // the speed that a paddle can move per frame: pixels per frame
    public float boundY = 5.01f; //the height of the object
    public Rigidbody2D body2d; // will be defined on the start() method
    public GameObject bulletObject;
    public float currentBullets;
    public Text bulletsText;

    private float bulletRegenTimer;
    private float shootTimer;


    // Start is called before the first frame update
    void Start()
    {
        /*
         * this will get the rigidbody from the object this script is attached to.
         * If we had more than one rigidbody we would need to specify the object.
         */
        body2d = GetComponent<Rigidbody2D>();
        currentBullets = TOTAL_BULLETS;
        bulletRegenTimer = BULLET_TIMER;
        shootTimer = 0.0f;
        this.bulletsText = GameObject.FindGameObjectWithTag(BULLET_UI_TEXT_TAG)
            .GetComponent<Text>();
        updateBulletsUI();
    }

    // Update is called once per frame
    void Update(){
        manageBullets();
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

        if (Input.GetKey(shoot))
        {
            if (currentBullets > 0 && shootTimer <= 0)
            {
                shootTimer = SHOOT_DELAY;
                currentBullets--;
                GameObject bullet = Instantiate(bulletObject);
                Vector3 pos = this.transform.position;
                pos.x += 2.0f;
                bullet.transform.position = pos;
            }
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

    private void manageBullets()
    {
        //only manage bullets if we don't have the 
        float dt = Time.deltaTime;
        if (currentBullets < TOTAL_BULLETS)
        {
            if (bulletRegenTimer <= 0)
            {
                currentBullets++;
                bulletRegenTimer = BULLET_TIMER;
            }
            else
            {
                bulletRegenTimer -= dt;
            }
        }

        if (shootTimer > 0)
        {
            shootTimer -= dt;
        }
        updateBulletsUI();
    }

    private void updateBulletsUI()
    {
        bulletsText.text = currentBullets.ToString();
    }
}
