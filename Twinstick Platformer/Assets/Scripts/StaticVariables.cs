using UnityEngine;
using System.Collections;

public class StaticVariables : MonoBehaviour
{

    private static float enemyMovespeed;
    private static int playerHealth = 10;
    private static int enemyHealth;
    private static Vector2 activeCheckpoint;

    public static int PlayerHealth
    {
        get { return playerHealth; }
        set { playerHealth = value; }
    }

    public static float EnemyMovespeed
    {
        get { return enemyMovespeed; }
        set { enemyMovespeed = value; }
    }

    public static int EnemyHealth
    {
        get { return enemyHealth; }
        set { enemyHealth = value; }
    }

    public static Vector2 ActiveCheckpoint
    {
        get { return activeCheckpoint; }
        set { activeCheckpoint = value; }
    }

}
