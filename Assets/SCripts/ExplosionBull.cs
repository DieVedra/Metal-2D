using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBull : MonoBehaviour
{
    public GameObject plasmExp;
    public void spawnExp()
    {
        GameObject clone = Instantiate(plasmExp, transform.position, transform.rotation);
    }
}
