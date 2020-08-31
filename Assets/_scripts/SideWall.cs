using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    public static string BALL_TAG = "Ball";
    public static string BULLET_TAG = "Bullet";
    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == BALL_TAG)
        {
            string wallName = this.transform.name;
            Manager.score(wallName);
            // menthodName, input for the method
            coll.gameObject.SendMessage("restartGame", 1.0f, SendMessageOptions.RequireReceiver);
        } 
        else if (coll.gameObject.tag == BULLET_TAG)
        {
            //destroy bullet
            Destroy(coll.gameObject);
        }
    }
}
