using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public static Player singletone { get; private set; }

    public float healthPlayer;

    public int ammoBolter;

    public int ammoPlasma;

    public int AmmoGranade;

    public Text healthBarText;

    public Text ammoBolterText;

    public Text ammoPlasmaText;

    [SerializeField] private Text _ammoGranadeText;

    public GameObject player;



    //public LayerMask whatIsEnemy;

    //public Transform pointRunStartEnemySphereTrig;

    //public float trigRange;

    //public const string zomb1StartingRun = "Zomb1_StartingRun";

    //private void Start()
    //{
    //    pointRunStartEnemySphereTrig = 
    //}

    private void Awake()
    {
        singletone = this;
    }

    private void Start()
    {
        healthBarText.text = "x" + healthPlayer.ToString();

        ammoBolterText.text = "x" + ammoBolter.ToString();

        ammoPlasmaText.text = "x" + ammoPlasma.ToString();

        _ammoGranadeText.text = "x" + AmmoGranade.ToString();
    }

    public void AmmoReducing()
    {
        if (PlayerController.instance.PlasmAnimCount)
        {
            ammoPlasma--;

            ammoPlasmaText.text = "x" + ammoPlasma.ToString();

            if (ammoPlasma == 0)
            {
                PlayerController.instance.StopPlasmagunEffect();
            }
        }
        else
        {
            ammoBolter --;

            ammoBolterText.text = "x" + ammoBolter.ToString();
        }
    }

    public void GranadeReducing()
    {
        AmmoGranade--;

        _ammoGranadeText.text = "x" + AmmoGranade.ToString();
    }


    public void TakeDamage(float damage)
    {
        healthPlayer -= damage;

        healthBarText.text = "x" + healthPlayer.ToString();

        if (healthPlayer <= 0)
        {
            Destruction();
            UIControllerInGame.singletone.PauseGame();
            //UIControllerInGame.singletone.btnResume.SetActive(false);
        }
    }

    void Destruction()
    {
        //Destroy(gameObject);
        player.SetActive(false);
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(pointRunStartEnemySphereTrig.position, trigRange);
    //}
}
