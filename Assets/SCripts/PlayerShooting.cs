using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerShooting : MonoBehaviour
{
    public static PlayerShooting instance = null;

    [SerializeField] private GameObject _bulletBolter;
    [SerializeField] private GameObject _bulletPlasma;
    [SerializeField] private GameObject _sleeveBolter;
    [SerializeField] private GameObject _granede;

    [SerializeField] private ParticleSystem _shootingBolterEffect;
    //[SerializeField] private ParticleSystem _shootingBolterEffectUp;

    [SerializeField] private ParticleSystem _shootingPlasmaEffect;
    //[SerializeField] private ParticleSystem _shootingPlasmaEffectUp;

    [SerializeField] private Transform _pointShoot;
    [SerializeField] private Transform _pointShootUp;

    [SerializeField] private Transform _pointSleeve;
    [SerializeField] private Transform _pointSleeveUp;

    [SerializeField] private AudioSource[] _shotBoltSound;

    [SerializeField] private AudioSource _noAmmoBolt;

    [SerializeField] private AudioSource _shotPlasm;

    [SerializeField] private float _timeRecharge = 0;
    [SerializeField] private float _startTimeRechBolter;
    [SerializeField] private float _startTimeRechPlasma;
    private float _timeRechargeGranade = 0;
    [SerializeField] private float _startTimeRechGranade;

    private bool _buttonShoot;
    [HideInInspector] public bool PossibleToCreateGranade = true;

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
        Shooting();
        RecharcgeGranade();
    }

    private void Shooting()
    {
        if (_buttonShoot)
        {
            if (_timeRecharge <= 0)
            {
                ShootingSwitch();
            }
            else
            {
                _timeRecharge -= Time.deltaTime;
            }
        }
        else
        {
            if (_timeRecharge >= 0)
            {
                _timeRecharge -= Time.deltaTime;
            }
        }
    }

    private void ShootingSwitch()
    {
        if (PlayerController.instance.PlasmAnimCount)
        {
            ShootPlasmaGun();
        }
        else
        {
            ShootBolterGun();
        }
    }

    private void StartTimeRechargeSelectedGun()
    {
        if (!PlayerController.instance.PlasmAnimCount)
        {
            _timeRecharge = _startTimeRechBolter;
        }
        else
        {
            _timeRecharge = _startTimeRechPlasma;
        }
    }

    private void ShootPlasmaGun()
    {

        if (Player.singletone.ammoPlasma > 0)
        {
            _shotPlasm.Play();

            //PlayerController.instance.BulletShotUp? _shootingPlasmaEffect.Play() : _shootingPlasmaEffectUp.Play();
            Instantiate(_shootingPlasmaEffect,
                        PlayerController.instance.BulletShotUp ? _pointShootUp.position : _pointShoot.position,
                        !PlayerController.instance.BulletShotUp ?
                                                                 (PlayerController.instance.facingRight ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(-180, 90, 0)) :
                                                                  Quaternion.Euler(-90, 90, 0)
                       ) ;
            Instantiate(_bulletPlasma,
                        PlayerController.instance.BulletShotUp ? _pointShootUp.position : _pointShoot.position,
                        !PlayerController.instance.BulletShotUp ? 
                                                                 (PlayerController.instance.facingRight ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, 180)):
                                                                  Quaternion.Euler(0, 0, 90));

            StartTimeRechargeSelectedGun();

            Player.singletone.AmmoReducing();
        }
        else
        {
            NoAmmoPlay();
        }
    }

    private void ShootBolterGun()
    {
       
            if (Player.singletone.ammoBolter > 0)
            {
                SoundBolterShootingPlay();

                Instantiate(_shootingBolterEffect,
                            PlayerController.instance.BulletShotUp ? _pointShootUp.position : _pointShoot.position,
                            !PlayerController.instance.BulletShotUp ?
                                                                     (PlayerController.instance.facingRight ? Quaternion.Euler(0, 90, 0) : Quaternion.Euler(-180, 90, 0)) :
                                                                      Quaternion.Euler(-90, 90, 0)
                           );

                Instantiate(_bulletBolter,
                            PlayerController.instance.BulletShotUp ? _pointShootUp.position : _pointShoot.position,
                            !PlayerController.instance.BulletShotUp ?
                                                                     (PlayerController.instance.facingRight ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, 180)):
                                                                      Quaternion.Euler(0, 0, 90)
                            );

                Instantiate(_sleeveBolter, _pointSleeve.position,
                            !PlayerController.instance.BulletShotUp ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 0, 90));

                _shootingBolterEffect.Play();

                Player.singletone.AmmoReducing();

                StartTimeRechargeSelectedGun();
            }
            else
            {
                NoAmmoPlay();
            }
    }

    private void CastGranade()
    {
        if (PossibleToCreateGranade)
        {
            if (Player.singletone.AmmoGranade > 0)
            {
                //SoundBolterShootingPlay();

                Instantiate(_granede,
                            PlayerController.instance.BulletShotUp ? _pointShootUp.position : _pointShoot.position,
                            !PlayerController.instance.BulletShotUp ?
                                                                     (PlayerController.instance.facingRight ? Quaternion.Euler(0, 0, 45) : Quaternion.Euler(0, 0, 135)) :
                                                                      Quaternion.Euler(0, 0, 90)
                            );

                //_shootingBolterEffect.Play();

                Player.singletone.GranadeReducing();

                PossibleToCreateGranade = false;

                _timeRechargeGranade += _startTimeRechGranade;
            }
            else
            {
                NoAmmoPlay();
            }
        }
    }
    private void RecharcgeGranade()
    {
        if (!PossibleToCreateGranade)
        {
            if (_timeRechargeGranade > 0)
            {
                _timeRechargeGranade -= Time.deltaTime;
            }
            else if(_timeRechargeGranade <= 0)
            {
                PossibleToCreateGranade = true;
            }
        }
    }

    public void OnShootButtonDown()
    {
        _buttonShoot = true;

    }
    public void OnShootButtonUp()
    {
        _buttonShoot = false;
    }

    public void CastGranadeButton()
    {
        CastGranade();
    }

    

    private void SoundBolterShootingPlay()
    {
        if (Player.singletone.ammoBolter % 2 == 1)
        {
            _shotBoltSound[0].Play();
            
        }
        else
        {
            _shotBoltSound[1].Play(); 
        }

    }

    private void NoAmmoPlay()
    {
        _noAmmoBolt.Play();
    }
}
