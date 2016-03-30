using UnityEngine;
using System.Collections;

public class StaticVariables : MonoBehaviour
{

    private static float enemyMovespeed;
    private static int playerHealth = 6;
    private static int enemyHealth = 3;
    private static int mana;
    private static int obtainedCollectables;
    private static Vector2 activeCheckpoint;
    private static int bossHealth = 15;

    public static int MaxHealth = 6, MaxMana;

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

    public static int ObtainedCollectables
    {
        get { return obtainedCollectables; }
        set { obtainedCollectables = value; }
    }

    public static int Mana
    {
        get { return mana; }
        set { mana = value; }
    }

    public static int BossHealth
    {
        get { return bossHealth; }
        set { bossHealth = value; }
    }

}
