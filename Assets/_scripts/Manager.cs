using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static string LEFT_WALL_ID = "leftWall";
    private static string RIGHT_WALL_ID = "rightWall";
    private static string BALL_TAG = "Ball";

    public static int playerScore;
    public static int aiScore;
    GameObject theBall;
    public GUISkin layout;

    // Start is called before the first frame update
    void Start()
    {
        playerScore = aiScore = 0;
        this.theBall = GameObject.FindGameObjectWithTag(BALL_TAG);
    }

    public static void score(string wallId)
    {
        if (wallId.Equals(LEFT_WALL_ID))
        {
            aiScore++;
        }
        else if (wallId.Equals(RIGHT_WALL_ID))
        {
            playerScore++;
        }
    }

    void OnGUI()
    {
        GUI.skin = layout;
        GUI.Label(new Rect(Screen.width / 2 - 150, 25, 100, 100), "" + playerScore);
        GUI.Label(new Rect(Screen.width / 2 + 150, 25, 100, 100), "" + aiScore);

        if (GUI.Button(new Rect(Screen.width / 2 - 60, 30, 120, 53), "RESTART"))
        {
            playerScore = 0;
            aiScore = 0;
            theBall.SendMessage("restartGame", 0.5f, SendMessageOptions.RequireReceiver);
        }

        //let's make limit 10
        if (playerScore == 10)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER ONE WINS!!");
            theBall.SendMessage("resetBall", 0f, SendMessageOptions.RequireReceiver);

        }
        else if (aiScore == 10)
        {
            GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "AI WINS!!");
            theBall.SendMessage("resetBall", 0f, SendMessageOptions.RequireReceiver);
        }
    }
}
