using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : Structures {

    public List<Transform> walls;
    public List<Transform> floor;
    public List<GameObject> enemies;
    public GameObject player;

    public bool playerTurn = true; //Monster turn when false
    public Vector2 playerDest; //Stores where the player is moving so monster don't occupy that spot

    public Text turnText;
    public int turn = 1;

    // Use this for initialization
    void Start () {
        //Generate Layout
        GetComponent<LayoutGenerator>().Generate();
        //Generate Monsters
        GetComponent<MonsterGenerator>().Generate();

        player = GameObject.FindGameObjectWithTag("Player");

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<Monster>().Begin();
        }
        player.GetComponent<Player>().Begin();
        playerDest = (Vector2)player.transform.position;
    }
	
	// Update is called once per frame
	void Update () {

        if(player == null)
        {
            SceneManager.LoadScene("Game Over");
        }
        else if (playerTurn)
        {
            player.GetComponent<Player>().TakeTurn();
        }
        else
        {
            foreach(GameObject enemy in enemies)
            {
                enemy.GetComponent<Monster>().TakeTurn();
            }
            playerTurn = true;

            turn++;
            turnText.text = "Turn: " + turn;
        }
	}
}
