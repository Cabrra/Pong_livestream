using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    public static string BALL_NAME_ID = "Ball";
    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("collider name is: " + coll.name);

        if (coll.name == BALL_NAME_ID)
        {
            string wallName = this.transform.name;
            Debug.Log("My name is: " + wallName);
            Manager.score(wallName);
            // menthodName, input for the method
            coll.gameObject.SendMessage("restartGame", 1.0f, SendMessageOptions.RequireReceiver);
        }
    }
}
