using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class MonsterGenerator : Structures {

	GameManager gameManager;

	public int numberOfMonsters;
	public int ratChance;

    public List<GameObject> enemies;
    [HideInInspector] public List<Transform> aTiles;

	Transform parent;
    GameObject rat;

    public Transform canvas;
    public Slider healthBar;

	void createAndParent(GameObject toInstantiate, Transform transform, int rotation, Transform parent2){
        GameObject instance = Instantiate(toInstantiate, transform.position, Quaternion.Euler(0, 0, -rotation)) as GameObject;
		instance.transform.SetParent (parent2);
        enemies.Add(instance);

        //Instantiates the health bar and parents to the world canvas
        Slider hBar = Instantiate(healthBar, new Vector2(transform.position.x, transform.position.y + (float)0.43), Quaternion.Euler(0, 0, 0)) as Slider;   //Positions it just below the top of the tile
        hBar.transform.SetParent(canvas);
        hBar.transform.localScale = new Vector3(1, 1, 1);   //The scale was changed on instantiation for some reason and this changes it back
        instance.GetComponent<Monster>().healthSlider = hBar;
    }

    void loadResources(){
        rat = Resources.Load("Monsters/Rat") as GameObject;
        //healthBar = Resources.Load("HealthBar") as Slider; //Doesn't work for some reason
	}

	public void Generate () {

		gameManager = GetComponent<GameManager> ();
        foreach (Transform transform in gameManager.floor)
        {
            aTiles.Add(transform);
        }    

        parent = (GameObject.Find("Monsters")).transform;
		loadResources ();

		for (int i=0; i < numberOfMonsters; i++) {
			int location = (int)Math.Floor(UnityEngine.Random.Range(0F,aTiles.Count));
			createAndParent(rat, aTiles[location], 0, parent);
			aTiles.Remove(aTiles[location]);


		}
        gameManager.enemies = enemies;
	}
}
