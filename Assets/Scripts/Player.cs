using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : Monster {

    public Text healthText;

    // Use this for initialization
	public override void Begin () {

        base.Begin();

        this.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);

    }

    protected override IEnumerator Move(Vector2 destination)
    {
        gameManager.playerDest = destination;
        return base.Move(destination);
    }

    public override void UpdateUI()
    {
        healthText.text = "Health: " + health + "/" + maxHealth;

        base.UpdateUI();
    }

    public override void TakeTurn()
    {
        int horizontal = 0;
        int vertical = 0;
        if (!moving)
        {
            horizontal = (int)Input.GetAxisRaw("Horizontal");
            vertical = (int)Input.GetAxisRaw("Vertical");
        }

        if (horizontal != 0)
        {
            vertical = 0;
        }

        if (horizontal != 0 || vertical != 0)
        {
            //Call AttemptMove passing in the generic parameter Wall, since that is what Player may interact with if they encounter one (by attacking it)
            //Pass in horizontal and vertical as parameters to specify the direction to move Player in.
            if(AttemptMove(new Vector2(this.transform.position.x + horizontal, this.transform.position.y + vertical)))
            {
                gameManager.playerTurn = false;
            }
        }
        UpdateUI();
    }
}
