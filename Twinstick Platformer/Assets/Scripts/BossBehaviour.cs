using UnityEngine;
using System.Collections;
using System.ComponentModel;

public class BossBehaviour : MonoBehaviour
{
    private int phase, startHealth;
    private float cycleTime, startFireSpeed;
    public float MaxCycleLenght, FireSpeed;
    public GameObject TPPoint1, TPPoint2, TPPoint3, Missile;
	void Start ()
	{
	    startHealth = StaticVariables.BossHealth;
	    startFireSpeed = FireSpeed;
	}
	

	void Update () 
    {
	    HandleHealth();
	    switch (phase)
	    {
            case 1:
                Phase1();
                break;
            case 2:
                Phase2();
                break;
            case 3:
                Phase3();
                break;
	    }
	    cycleTime += Time.deltaTime;
	    if (Input.GetKeyDown(KeyCode.K))
	    {
	        StaticVariables.BossHealth -= 6;
            
	    }

    }

    void HandleHealth()
    {
        if (StaticVariables.BossHealth > startHealth - (startHealth/3))
        {
            phase = 1;
        }
        else if (StaticVariables.BossHealth > startHealth - startHealth*2/3)
        {
            phase = 2;
        }
        else if (StaticVariables.BossHealth > 0)
        {
            phase = 3;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Phase1()
    {
        Debug.Log("en");
        if (cycleTime >= MaxCycleLenght)
        {
            cycleTime = 0;
            FireSpeed = startFireSpeed;
        }

        if (cycleTime < MaxCycleLenght/3)
        {
            transform.position = TPPoint1.transform.position;
        }
        else if (cycleTime < MaxCycleLenght*2/3)
        {
            transform.position = TPPoint2.transform.position;
        }
        else
        {
            transform.position = TPPoint3.transform.position;
        }
        if (cycleTime > FireSpeed)
        {
            MissileStar();
            FireSpeed += startFireSpeed;
        }

    }

    void Phase2()
    {
        Debug.Log("to");
        if (cycleTime >= MaxCycleLenght)
        {
            cycleTime = 0;
            FireSpeed = startFireSpeed;
        }
        if (cycleTime < MaxCycleLenght / 3)
        {
            transform.position = TPPoint1.transform.position;
        }
        else if (cycleTime < MaxCycleLenght * 2 / 3)
        {
            transform.position = TPPoint2.transform.position;
        }
        else
        {
            transform.position = TPPoint3.transform.position;
        }
        if (cycleTime > FireSpeed)
        {
            FireSpeed += startFireSpeed;
        }
        MisselRain();
    }

    void Phase3()
    {
        Debug.Log("tre");
        float shortCycleLenght = MaxCycleLenght*0.8f;
        if (cycleTime >= shortCycleLenght)
        {
            cycleTime = 0;
            FireSpeed = startFireSpeed;
        }
        if (cycleTime < shortCycleLenght / 3)
        {
            transform.position = TPPoint1.transform.position;
        }
        else if (cycleTime < shortCycleLenght * 2 / 3)
        {
            transform.position = TPPoint2.transform.position;
        }
        else
        {
            transform.position = TPPoint3.transform.position;
        }
        if (cycleTime > FireSpeed * 0.8)
        {
            MissileStar();
            FireSpeed += startFireSpeed;
        }
        MisselRain();
    }

    void MissileStar()
    {
        for (int i = 0; i < 360; i+=45)
        {
            Instantiate(Missile, transform.position, Quaternion.Euler(0,0,i));
        }
        

    }

    void MisselRain()
    {
        for (int i = 95; i < Random.Range(0,100); i++)
        {
            Instantiate(Missile, new Vector3(Random.Range(-176, -26), 46.5f),
                Quaternion.Euler(0, 0, Random.Range(150, 210)));
        }

    }
}
