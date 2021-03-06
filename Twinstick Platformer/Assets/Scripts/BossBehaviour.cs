﻿using UnityEngine;
using System.Collections;
using System.ComponentModel;
using UnityEngine.SceneManagement;

public class BossBehaviour : MonoBehaviour
{
    private int phase, startHealth, attacksFired = 1;
    private float fireCooldown, cycleTime, spawnTime, fadeTime, passiveSoundColdown, deathDelay =2;
    private Animator anim;
    public float MaxCycleLenght, FireSpeed;
    public GameObject TPPoint1, TPPoint2, TPPoint3, Missile, UI;
    public bool FireMissleStar, ResetCycleTime, TeleportNow, Die;
    private bool isDead = false;
    public Texture2D Black;
    public AudioSource MissileLaunch, Laugh;
	void Start ()
	{
	    startHealth = StaticVariables.BossHealth;
	    anim = GetComponent<Animator>();
	    transform.parent.position = TPPoint1.transform.position;
        iTween.CameraFadeAdd(Black);
	}
	

	void Update ()
	{
        Debug.Log(StaticVariables.BossHealth + " startliv:" + startHealth);
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
            case 0:
            anim.SetBool("Teleport", false);
            anim.SetBool("StartFire", false);
            anim.SetInteger("Phase",1);
            anim.SetBool("Dying", true);
                break;
	    }
	    cycleTime += Time.deltaTime;

	    if (ResetCycleTime)
	    {
	        cycleTime = 0;
            anim.SetBool("StartFire",false);
	    }
	    MissileStar();
        Teleport();
        passiveSoundColdown += Random.Range(0.2f, 1.2f) * Time.deltaTime;
        if (passiveSoundColdown > 15)
        {

            Laugh.Play();
            passiveSoundColdown = 0;
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
            phase = 0;
        }


        if (Die && !isDead)
        {
            UI.SetActive(false);
            iTween.CameraFadeTo(100,20);
            fadeTime = 2;
            isDead = true;
        }
        if (isDead && fadeTime< 0)
        {
            StaticVariables.BossHealth = startHealth;
            Debug.Log("loaded");
            SceneManager.LoadScene(0);
        }
        if (isDead)
        {
            fadeTime -= Time.deltaTime;
        }

        if (StaticVariables.PlayerHealth <= 0)
        {
            GameObject.Find("Player").GetComponent<Player>().DeadSound.enabled = true;

        }

        if (GameObject.Find("Player").GetComponent<Player>().DeadSound.isPlaying)
        {
            deathDelay -= Time.deltaTime;
        }
        else if (deathDelay < 2)
        {
            StaticVariables.BossHealth = startHealth;
            StaticVariables.PlayerHealth = StaticVariables.MaxHealth;
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    void Phase1()
    {

        if (cycleTime > FireSpeed && !anim.GetBool("StartFire") && !anim.GetBool("Teleport"))
        {
            anim.SetBool("StartFire", true);
            attacksFired ++;
            anim.SetInteger("Phase", 1);
        }

    }

    void Phase2()
    {
        if (cycleTime > FireSpeed && !anim.GetBool("Teleport"))
        {
            attacksFired++;
            cycleTime = -2;
        }
        anim.SetInteger("Phase", 2);
        MisselRain();
    }

    void Phase3()
    {
        if (cycleTime > FireSpeed && !anim.GetBool("StartFire") && !anim.GetBool("Teleport"))
        {
            anim.SetBool("StartFire", true);
            attacksFired++;
        }
        anim.SetInteger("Phase", 3);
        MisselRain();
    }

    void MissileStar()
    {
        fireCooldown -= Time.deltaTime;
        if (FireMissleStar && fireCooldown < 0)
        {
            MissileLaunch.Play();
            for (int i = 0; i < 360; i += 45)
            {
                Instantiate(Missile, transform.position, Quaternion.Euler(0, 0, i));
            }
            fireCooldown = 0.3f;
        }

    }

    void MisselRain()
    {
        
        spawnTime += Time.deltaTime;
        if (spawnTime > 0.1f)
        {
            for (int i = 70; i < Random.Range(0, 100); i++)
            {
                Instantiate(Missile, new Vector3(Random.Range(-176, -26), 46.5f),
                    Quaternion.Euler(0, 0, Random.Range(150, 210)));
            }
            spawnTime = 0;
        }


    }

    void Teleport()
    {
        if (attacksFired < 3 && transform.parent.position != TPPoint1.transform.position && !anim.GetBool("Teleport"))
        {
            anim.SetBool("Teleport", true);
        }
        else if (attacksFired > 2 && attacksFired < 5 && transform.parent.position != TPPoint2.transform.position && !anim.GetBool("Teleport"))
        {
            anim.SetBool("Teleport", true);
        }
        else if (transform.parent.position != TPPoint3.transform.position && attacksFired > 4 && !anim.GetBool("Teleport"))
        {
            anim.SetBool("Teleport", true);
        }

        if (attacksFired > 6)
        {
            attacksFired = 1;
        }

        if (attacksFired < 3 && TeleportNow)
        {
            transform.parent.position = TPPoint1.transform.position;
            anim.SetBool("Teleport", false);
        }
        else if (attacksFired < 5 && TeleportNow)
        {
            transform.parent.position = TPPoint2.transform.position;
            anim.SetBool("Teleport", false);
        }
        else if (TeleportNow)
        {
            transform.parent.position = TPPoint3.transform.position;
            anim.SetBool("Teleport", false);
        }


    }

}
