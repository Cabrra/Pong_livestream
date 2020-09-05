using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPaddle : MonoBehaviour
{

    //Game variables
    private static float BULLET_TIMER = 1.5f; // 1.5 seconds 
    private static float SHOOT_DELAY = 0.5f; // 0.5 seconds 
    private static int TOTAL_BULLETS = 10; // 10 bullets 
    private static string BULLET_UI_TEXT_TAG = "PlayerBullets";
    private static float SHIELD_FULL_HP = 50.0f;
    private static float SHIELD_REGEN = 20.0f;
    private static float SHIELD_REGEN_TIMER = 5.0f;
    private static float SHIELD_STUN_TIMER = 1.5f;

    //Movement for player
    public KeyCode moveUp = KeyCode.W;
    public KeyCode moveDowwn = KeyCode.S;
    public KeyCode shoot = KeyCode.Space;
    //for AI
    private static string BALL_TAG = "Ball";
    private static float PADDLE_REACH = 1.0f;
    private GameObject theBall;
    private float currentShootingTimer;

    public float speed = 10.0f; // the speed that a paddle can move per frame: pixels per frame
    public float boundY = 5.01f; //the height of the object
    public Rigidbody2D body2d; // will be defined on the start() method
    public GameObject bulletObject;
    public float currentBullets;
    public Text bulletsText;
    public bool isLeft;

    //shield variable
    private float shieldCurrentCooldown;
    private float currentShield;
    private bool isStunned;
    private float stunTimer;
    public GameObject shieldUI;

    private float bulletRegenTimer;
    private float shootTimer;
    public bool isPlayer;

    // Start is called before the first frame update
    void Start()
    {
        /*
         * this will get the rigidbody from the object this script is attached to.
         * If we had more than one rigidbody we would need to specify the object.
         */
        this.theBall = GameObject.FindGameObjectWithTag(BALL_TAG);
        body2d = GetComponent<Rigidbody2D>();
        currentBullets = TOTAL_BULLETS;
        bulletRegenTimer = BULLET_TIMER;
        shootTimer = 0.0f;
        currentShield = SHIELD_FULL_HP;
        isStunned = false;
        stunTimer = SHIELD_STUN_TIMER;
        shieldCurrentCooldown = SHIELD_REGEN_TIMER;
        resetShootingTimer();

        this.bulletsText = GameObject.FindGameObjectWithTag(BULLET_UI_TEXT_TAG)
            .GetComponent<Text>();
        updateBulletsUI();
    }

    // Update is called once per frame
    void Update()
    {
        manageBullets();
        //rigidbody has a lot of cool members such as posdition and velocity
        var velocity = body2d.velocity;

        if (isPlayer)
        {
            velocity.y = managePlayerMovement();
            if (Input.GetKey(shoot)) manageShooting();
        }
        else
        {
            manageAIShooting();
            velocity.y = manageAIMovement();

        }

        manageShields();
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

    private void manageBullets()
    {
        //only manage bullets if we don't have the 
        float dt = Time.deltaTime;
        if (currentBullets < TOTAL_BULLETS
            && !isStunned)
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
        if (isPlayer)
        {
            updateBulletsUI();
        }
    }

    private void updateBulletsUI()
    {
        bulletsText.text = currentBullets.ToString();
    }

    private void manageShooting()
    {
        if (!isStunned)
        {

            if (currentBullets > 0 && shootTimer <= 0)
            {
                int sign = 1;
                if (!isLeft) sign = -1;
                shootTimer = SHOOT_DELAY;
                currentBullets--;
                GameObject bullet = Instantiate(bulletObject);
                Vector3 pos = this.transform.position;
                pos.x += (0.5f * sign);
                bullet.transform.position = pos;

                bullet.SendMessage("setOwner", isLeft, SendMessageOptions.RequireReceiver);
            }
        }
    }

    private float managePlayerMovement()
    {
        //early exit
        if (isStunned)
        {
            return 0;
        }

        if (Input.GetKey(moveDowwn))
        {
            //move down
            return -speed;
        }
        else if (Input.GetKey(moveUp))
        {
            //move up
            return speed;
        }
        else
        {
            return 0;
        }
    }

    private void manageShields()
    {
        //optimization - call Time.deltatime only once and pass it
        float dt = Time.deltaTime;
        if (isStunned)
        {
            //handle stun
            if (stunTimer <= 0)
            {
                stunTimer = SHIELD_STUN_TIMER;
                isStunned = false;
            }
            else
            {
                stunTimer -= dt;
            }
        }

        //handle shield regeneration
        if (currentShield < SHIELD_FULL_HP)
        {
            if (shieldCurrentCooldown <= 0)
            {
                shieldCurrentCooldown = SHIELD_REGEN_TIMER;
                currentShield += SHIELD_REGEN;
                //sanitize
                if (currentShield > SHIELD_FULL_HP)
                {
                    currentShield = SHIELD_FULL_HP;
                }
            }
            else
            {
                shieldCurrentCooldown -= dt;
            }
        }

        if (shieldUI != null)
        {
            //calculate X scale using currentHealth
            //full health means scale.X = 1
            var scale = shieldUI.transform.localScale;
            scale.x = currentShield / SHIELD_FULL_HP;
            shieldUI.transform.localScale = scale;
        }
    }
    public void damageShield(float damage)
    {
        if (!isStunned)
        {
            this.currentShield -= damage;
            if (this.currentShield <= 0)
            {
                currentShield = 0.0f;
                isStunned = true;
            }
        }
    }

    private float manageAIMovement()
    {
        //early exit
        if (isStunned)
        {
            return 0;
        }

        float ballPosY = this.theBall.transform.position.y;
        var velocity = body2d.velocity;
        float distanceY = ballPosY - this.transform.position.y;

        if (distanceY > PADDLE_REACH)
        {
            //move up
            //using player's method
            return this.speed;

        }
        else if (distanceY < -PADDLE_REACH)
        {
            //move down
            return -this.speed;
        }
        else
        {
            return 0;
        }
    }

    private void resetShootingTimer()
    {
        currentShootingTimer = Random.Range(0.0f, 8.0f);
    }

    private void manageAIShooting()
    {
        if (currentShootingTimer <= 0)
        {
            manageShooting();
            resetShootingTimer();
        }
        else
        {
            currentShootingTimer -= Time.deltaTime;
        }
    }
}
