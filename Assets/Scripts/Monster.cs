using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Monster : Structures {

    //Basic stats
    public int maxHealth;
	public int health;
	public int pAttack;
	public int pDefense;
	public int mAttack;
	public int mDefense;
	public int captureRating;

    public bool playerTurn;
    public GameManager gameManager;
    public float moveTime = 0.1f;			//Time it will take object to move, in seconds.
		
	//private BoxCollider2D boxCollider; 		//The BoxCollider2D component attached to this object.
	private Rigidbody2D rb2D;				//The Rigidbody2D component attached to this object.
	private float inverseMoveTime;			//Used to make movement more efficient.
    public bool moving = false;

    protected List<GameObject> enemies;
    protected List<Transform> walls;
    public GameObject player;

    public Slider healthSlider;

	public virtual void Begin () //Basically just the start function with a different name so scripts are run in order
	{
		//Get a component reference to this object's BoxCollider2D
		//boxCollider = GetComponent <BoxCollider2D> ();
		
		//Get a component reference to this object's Rigidbody2D
		rb2D = GetComponent <Rigidbody2D> ();
		
		//By storing the reciprocal of the move time we can use it by multiplying instead of dividing, this is more efficient.
		inverseMoveTime = 1f / moveTime;

        gameManager = GameObject.Find("Game Engine").GetComponent<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        walls = gameManager.walls;

        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    public virtual void UpdateUI()
    {
        healthSlider.value = health;
    }

	protected bool CheckSight(){
		//Checks the sight radius for anything to attack
		//Store all monsters seen in the units array
		return false;
	}

	protected bool AttemptMove(Vector2 destination){
        //Returns true if it is a valid option, i.e. the space is availiable or had an enemy
        //Use units array to return GameObject preventing move
        enemies = gameManager.enemies;

        if (walls.Exists(x => (x.position.x == destination.x && x.position.y == destination.y)))
        {
            return false;
        }
        else if(enemies.Exists(x => ((Vector2)x.transform.position == destination)))
        {
            if(this.tag == "Player")
            {
                StartCoroutine(Attack(enemies.Find(x => ((Vector2)x.transform.position == destination))));
                return true;
            }
            else
            {
                return false;
            }
        }
        else if((Vector2)player.transform.position == destination || destination == gameManager.playerDest)
        {
            //StartCoroutine(Attack(player));
            Debug.Log("Player Attacked");
            return true;
        }
        else
        {
            StartCoroutine(Move(destination));
            return true;
        }

	}

	protected virtual IEnumerator Move(Vector2 destination){

        moving = true;

		//Moves the monster to the destination
		//Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
		//Square magnitude is used instead of magnitude because it's computationally cheaper.
		float sqrRemainingDistance = ((Vector2)transform.position - destination).sqrMagnitude;
		
		//While that distance is greater than a very small amount (Epsilon, almost zero):
		while(sqrRemainingDistance > float.Epsilon)
		{
			//Find a new position proportionally closer to the end, based on the moveTime
			Vector3 newPostion = Vector3.MoveTowards(rb2D.position, destination, inverseMoveTime * Time.deltaTime);
			
			//Call MovePosition on attached Rigidbody2D and move it to the calculated position.
			rb2D.MovePosition (newPostion);
			
			//Recalculate the remaining distance after moving.
			sqrRemainingDistance = ((Vector2)transform.position - destination).sqrMagnitude;

            //Move the healthbar along with it
            if(this.tag != "Player")
                healthSlider.transform.position = new Vector3(transform.position.x, transform.position.y + (float)0.43);

			//Return and loop until sqrRemainingDistance is close enough to zero to end the function
			yield return null;
		}
        moving = false;
	}

	protected virtual IEnumerator Attack(GameObject ene){
        //Attacks the referenced enemy
        //Needs to choose between physical and magical attacks

        moving = true;
        Monster enemy = ene.GetComponent<Monster>();
        enemy.health = enemy.health - this.pAttack;

        if(enemy.health <= 0)
        {
            Destroy(enemy.healthSlider.gameObject);
            Destroy(ene);
            gameManager.enemies.Remove(ene);
        }

        yield return new WaitForSeconds(0.25f); // Play attack animation here
        moving = false;
	}

    public virtual void TakeTurn()
    {
        UpdateUI();
        int direction = (int)UnityEngine.Random.Range(0, 4);
        if (moving)
            return;
        else if (direction == 0)
            AttemptMove(new Vector2(transform.position.x + 1, transform.position.y));
        else if (direction == 1)
            AttemptMove(new Vector2(transform.position.x, transform.position.y + 1));
        else if (direction == 2)
            AttemptMove(new Vector2(transform.position.x - 1, transform.position.y));
        else if (direction == 3)
            AttemptMove(new Vector2(transform.position.x, transform.position.y - 1));
    }

    /*protected virtual void Update()
    {
        playerTurn = gameManager.playerTurn;
        if (playerTurn)
            return;
        foreach(Transform enemy in gameManager.enemies)
        {
            TakeTurn(enemy);
        }
        gameManager.playerTurn = !gameManager.playerTurn;
    }*/


}
