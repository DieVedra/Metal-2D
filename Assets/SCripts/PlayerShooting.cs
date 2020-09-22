using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerShooting : MonoBehaviour
{
    public static PlayerShooting instance = null;

    public GameObject bulletBolter;
    public GameObject bulletPlasma;

    public Transform pointShoot;
    public Transform pointShootUp;

    public float timeRecharge;
    public float startTimeRechargeBolter;
    public float startTimeRechargePlasma;

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

        
        
    }

    void Shoot()
    {
        if (PlayerController.instance.plasmAnimCount)
        {
            if (PlayerController.instance.bulletShotUp != true)
            {

                Instantiate(bulletPlasma, pointShoot.position, pointShoot.rotation);
                StartTimeRechargeSlectedGun();

            }
            else
            {
                Instantiate(bulletPlasma, pointShootUp.position, Quaternion.Euler(0, 0, 90));
                StartTimeRechargeSlectedGun();
            }
        }
        else
        {
            if (PlayerController.instance.bulletShotUp != true)
            {

                Instantiate(bulletBolter, pointShoot.position, pointShoot.rotation);
                StartTimeRechargeSlectedGun();

            }
            else
            {
                Instantiate(bulletBolter, pointShootUp.position, Quaternion.Euler(0, 0, 90));
                StartTimeRechargeSlectedGun();
            }
        }
        
    }

    public void StartTimeRechargeSlectedGun()
    {
        if (!PlayerController.instance.plasmAnimCount)
        {
            timeRecharge = startTimeRechargeBolter;
        }
        else
        {
            timeRecharge = startTimeRechargePlasma;
        }
    }

    public void OnShootBottonDown()
    {
        botton = true;

    }
    public void OnShootBottonUp()
    {
        botton = false;
        if (PlayerController.instance.plasmAnimCount)
        {
            timeRecharge = 0 /*startTimeRechargePlasma*/;
        }
    }
}
