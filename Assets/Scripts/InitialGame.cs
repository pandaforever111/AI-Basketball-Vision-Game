using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialGame : MonoBehaviour
{
    public GameObject InitialBG;
    public GameObject LetsPlay;
    public GameObject NextBG;
    public GameObject ScoreBoard;
    public GameObject Ball;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeBG()
    {
        InitialBG.SetActive(false);
        NextBG.SetActive(true);
        Ball.SetActive(true);
        ScoreBoard.SetActive(true);
        LetsPlay.SetActive(false);

    }

    public void LetsPlayGame()
    {
        LetsPlay.SetActive(true);
        Invoke("ChangeBG", 1.5f);
    }
}
