using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance;
	public Text blueText;
	public Text redText;
	public Ball ball;

	int blueScore = 0;
	int redScore = 0;

	void Awake () {
		instance = this;
	}

	public void ScoreForBlue(){
		blueScore++;
		UpdateUI();
		StartCoroutine("ResetGame");
	}

	public void ScoreForRed(){
		redScore++;
		UpdateUI();
		StartCoroutine("ResetGame");
	}

	void UpdateUI(){
		blueText.text = blueScore.ToString();
		redText.text = redScore.ToString();
	}


	IEnumerator ResetGame(){
		yield return new WaitForSeconds(1);
		ball.ResetBall();
	}
}
