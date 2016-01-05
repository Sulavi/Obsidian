using UnityEngine;
using System.Collections;

public class Rat : Monster {

	// Use this for initialization
	public override void Begin () {

        maxHealth = 5;
        health = 5;
        pAttack = 1;
        pDefense = 0;

        base.Begin();

	}

    /*// Update is called once per frame
	void Update () {
		StartCoroutine( Move (new Vector2 (0, 0)));
	}*/
}
