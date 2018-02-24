using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour
{
	private static GameControl instance; //A reference to our game control script so we can access it statically.
	public Text scoreText; //A reference to the UI text component that displays the player's score.
	public GameObject gameOvertext; //A reference to the object that displays the text which appears when the player dies.

	public int score { get; private set; } //The player's score.
	public bool gameOver = false; //Is the game over?
	public float scrollSpeed = -1.5f;


	public static GameControl Instance
	{
		get
		{
			if (instance != null)
			{
				return instance;
			}
			else
			{
				instance = FindObjectOfType<GameControl>();

				if (instance == null)
				{
					instance = new GameObject().AddComponent<GameControl>();
				}
			}

			return instance;
		}
	}

	void Update()
	{
		//If the game is over and the player has pressed some input...
		if (gameOver && Input.GetMouseButtonDown(0))
		{
			//...reload the current scene.
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		}
	}

	public void BirdScored()
	{
		//The bird can't score if the game is over.
		if (gameOver)
			return;
		//If the game is not over, increase the score...
		score++;

		if (scoreText != null)
		{
			//...and adjust the score text.
			scoreText.text = "Score: " + score.ToString();
		}
	}

	public void BirdDied()
	{
		if (gameOvertext != null)
		{
			//Activate the game over text.
			gameOvertext.SetActive(true);
		}

		//Set the game to be over.
		gameOver = true;
	}

	public void Reset()
	{
		gameOver = false;
		score = 0;
	}

}