using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GsmeManager : MonoBehaviour
{

    public TextMeshProUGUI scoreText;
    private int score = 0;
    public GameObject ball1;
    public GameObject resetBall;
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score= " + score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            score++;
            scoreText.text = "Score= " + score;

        }
    }


    public void resetGame()
    {
        ball1.transform.position = resetBall.transform.position;
    }
}
