using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LvlStatusBar : MonoBehaviour
{

    public Slider lvlStatusBar;

    public GameObject StartLVLPoint;

    public GameObject EndLVLPoint;

    public GameObject PlayerPosition;

     float levelLength;

    //public float playerPositionVal;

    // Start is called before the first frame update
    void Start()
    {
        LevelLength();
    }

    // Update is called once per frame
    void Update()
    {
        lvlStatusBar.value = PlayerPosition.transform.position.x;
    }


    public void LevelLength()
    {
        levelLength = EndLVLPoint.transform.position.x - StartLVLPoint.transform.position.x;

        lvlStatusBar.maxValue = levelLength;
    }

    //public void PositionPlayerInTheLevel()
    //{
    //    playerPositionVal = PlayerPosition.transform.position.x ;
    //}
}
