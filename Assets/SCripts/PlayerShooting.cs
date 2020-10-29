using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerShooting : MonoBehaviour
{
    public static PlayerShooting instance = null;

    public GameObject bulletBolter;
    public GameObject bulletPlasma;

    public ParticleSystem shootingBolterEffect;
    public ParticleSystem shootingBolterEffectUp;

    public ParticleSystem shootingPlasmaEffect;
    public ParticleSystem shootingPlasmaEffectUp;

    public Transform pointShoot;
    public Transform pointShootUp;

    public AudioSource[] shotBolt;

    public AudioSource noAmmoBolt;

    public float timeRecharge;
    public float startTimeRechBolter;
    public float startTimeRechPlasma;

    [HideInInspector]
    public bool botton;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //private void Start()
    //{
    //    shotBolt = GameObject.
    //}

    private void Update()
    {
        if (botton == true)
        {
            if (timeRecharge <= 0)
            {
                Shoot();
                
            }
            else
            {
                timeRecharge -= Time.deltaTime;
            }
        }
        else
        {
            if (timeRecharge >= 0)
            {
                timeRecharge -= Time.deltaTime;
            }
        }

        
        
    }

    void Shoot()
    {
        if (PlayerController.instance.plasmAnimCount)
        {
            if (PlayerController.instance.bulletShotUp != true)
            {
                shootingPlasmaEffect.Play();
                Instantiate(bulletPlasma, pointShoot.position, pointShoot.rotation);
                StartTimeRechargeSlectedGun();

            }
            else
            {
                shootingPlasmaEffectUp.Play();
                Instantiate(bulletPlasma, pointShootUp.position, Quaternion.Euler(0, 0, 90));
                StartTimeRechargeSlectedGun();
            }
        }
        else
        {
            if (PlayerController.instance.bulletShotUp != true)
            {
                if (Player.singletone.ammoBolter > 0)
                {
                    SoundBolterShootingPlayAndCheck();
                    Instantiate(bulletBolter, pointShoot.position, pointShoot.rotation);
                    shootingBolterEffect.Play();
                    Player.singletone.AmmoReducing();
                    StartTimeRechargeSlectedGun();
                }
                else
                {
                    noAmmoBolt.Play();
                }

            }
            else
            {
                if (Player.singletone.ammoBolter > 0)
                {
                    SoundBolterShootingPlayAndCheck();
                    Instantiate(bulletBolter, pointShootUp.position, Quaternion.Euler(0, 0, 90));
                    shootingBolterEffectUp.Play();
                    Player.singletone.AmmoReducing(); ;
                    StartTimeRechargeSlectedGun();
                }
                else
                {
                    noAmmoBolt.Play();
                }
            }
        }
        
    }

    public void StartTimeRechargeSlectedGun()
    {
        if (!PlayerController.instance.plasmAnimCount)
        {
            timeRecharge = startTimeRechBolter;
        }
        else
        {
            timeRecharge = startTimeRechPlasma;
        }
    }

    public void OnShootBottonDown()
    {
        botton = true;

    }
    public void OnShootBottonUp()
    {
        botton = false;
    }

    public void SoundBolterShootingPlayAndCheck()
    {
        if (Player.singletone.ammoBolter % 2 == 1)
        {
            shotBolt[0].Play();
            
        }
        else
        {
            shotBolt[1].Play(); 
        }

    }
}
