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
    Slider healthBar;

	void createAndParent(GameObject toInstantiate, Transform transform, int rotation, Transform parent2){
        GameObject instance = Instantiate(toInstantiate, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, -rotation)) as GameObject;
		instance.transform.SetParent (parent2);
        enemies.Add(instance);
	}


	void loadResources(){
        rat = Resources.Load("Monsters/Rat") as UnityEngine.GameObject;
        healthBar = Resources.Load("HealthBar") as Slider;
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
