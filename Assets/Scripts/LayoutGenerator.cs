using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LayoutGenerator : MonoBehaviour {

    GameManager gameManager;

	public int levelSize; // Number of accessable tiles to create

	GameObject tileClone;
	Transform parent;

	GameObject tile;
	GameObject wall;
	GameObject corner;
	GameObject corner2;
	GameObject partition;
	GameObject outcropping;
	GameObject pillar;

    GameObject exit;

	public List<Vector2> tiles = new List<Vector2>{}; // List of created tiles
	public List<Vector2> freeSpaces = new List<Vector2>{}; // List of spaces where an accessable tile could be created.
    public List<Transform> walls;
    public List<Transform> floor;
    public List<Transform> items;

	void loadTiles(){
		tile = Resources.Load("Terrain/Tile") as GameObject;
		wall = Resources.Load("Terrain/Wall") as GameObject;
		corner = Resources.Load("Terrain/Corner") as GameObject;
		partition = Resources.Load("Terrain/Partition") as GameObject;
		outcropping = Resources.Load("Terrain/Outcropping") as GameObject;
		pillar = Resources.Load("Terrain/Pillar") as GameObject;
        //GameObject corner2 = Resources.Load("Terrain/Corner2") as GameObject; //TODO Impliment complete walls

        exit = Resources.Load("Terrain/Exit") as GameObject;
	}

	void fillWall(Vector2 space){
		if(tiles.Contains(new Vector2(space.x+1,space.y))){
			if(tiles.Contains(new Vector2(space.x,space.y+1))){
				if(tiles.Contains(new Vector2(space.x-1,space.y))){
					if(tiles.Contains(new Vector2(space.x,space.y-1))){
						createAndParent(pillar, space, 0, parent);
					}else{
						createAndParent(outcropping, space, 180, parent);
					}
				}else if(tiles.Contains(new Vector2(space.x,space.y-1))){
					createAndParent(outcropping, space, -90, parent);
				}
				else{
					createAndParent(corner, space, -90, parent);
				}
			}else if(tiles.Contains(new Vector2(space.x-1,space.y))){
				if(tiles.Contains(new Vector2(space.x,space.y-1))){
					createAndParent(outcropping, space, 0, parent);
				}else{
					createAndParent(partition, space, 0, parent);
				}
			}else if(tiles.Contains(new Vector2(space.x,space.y-1))){
				createAndParent(corner, space, 0, parent);
			}else{
				createAndParent(wall, space, 0, parent);
			}
		}
		else if(tiles.Contains(new Vector2(space.x,space.y+1))){
			if(tiles.Contains(new Vector2(space.x-1,space.y))){
				if(tiles.Contains(new Vector2(space.x,space.y-1))){
					createAndParent(outcropping, space, 90, parent);
				}else{
					createAndParent(corner, space, 180, parent);
				}
			}else if(tiles.Contains(new Vector2(space.x,space.y-1))){
				createAndParent(partition, space, -90, parent);
			}else{
				createAndParent(wall, space, -90, parent);
			}
		}
		else if(tiles.Contains(new Vector2(space.x-1,space.y))){
			if(tiles.Contains(new Vector2(space.x,space.y-1))){
				createAndParent(corner, space, 90, parent);
			}else{
				createAndParent(wall, space, 180, parent);
			}
		}
		else if(tiles.Contains(new Vector2(space.x,space.y-1))){
			createAndParent(wall, space, 90, parent);
		}
	}

	void createAndParent(GameObject toInstantiate, Vector2 position, int rotation, Transform parent2){
		GameObject instance = Instantiate (toInstantiate, position, Quaternion.Euler (0, 0, -rotation)) as GameObject;
		instance.transform.SetParent (parent2);
        if (toInstantiate == tile)
            floor.Add(instance.transform);
        else if (toInstantiate == exit) ;
        else
            walls.Add(instance.transform);
	}

	public void Generate () {

        gameManager = GetComponent<GameManager>();

		parent = (GameObject.Find("Terrain")).transform;  // Finds the gameobject under which we make all the terrain tiles.

		loadTiles ();

		freeSpaces.Add (new Vector2(0,0)); // Starts the fee spaces list with the origin

		for (int i = 0; i < levelSize; i++) {
			int nextTile = (int)Math.Floor(UnityEngine.Random.Range(0F,freeSpaces.Count)); // Randomly selects an item in the free spaces list to build a tile on.

			createAndParent(tile, new Vector2 (freeSpaces[nextTile].x, freeSpaces[nextTile].y), 0, parent);

			tiles.Add(new Vector2(freeSpaces[nextTile].x, freeSpaces[nextTile].y)); // Add created tile to list

			// Add new adjacent spaces to free spaces list if they do not already contain a tile or are in the list already
			if(!(freeSpaces.Contains(new Vector2(freeSpaces[nextTile].x+1,freeSpaces[nextTile].y))||tiles.Contains(new Vector2(freeSpaces[nextTile].x+1,freeSpaces[nextTile].y))))
				freeSpaces.Add(new Vector2(freeSpaces[nextTile].x+1,freeSpaces[nextTile].y));
			if(!(freeSpaces.Contains(new Vector2(freeSpaces[nextTile].x-1,freeSpaces[nextTile].y))||tiles.Contains(new Vector2(freeSpaces[nextTile].x-1,freeSpaces[nextTile].y))))
				freeSpaces.Add(new Vector2(freeSpaces[nextTile].x-1,freeSpaces[nextTile].y));
			if(!(freeSpaces.Contains(new Vector2(freeSpaces[nextTile].x,freeSpaces[nextTile].y+1))||tiles.Contains(new Vector2(freeSpaces[nextTile].x,freeSpaces[nextTile].y+1))))
				freeSpaces.Add(new Vector2(freeSpaces[nextTile].x,freeSpaces[nextTile].y+1));
			if(!(freeSpaces.Contains(new Vector2(freeSpaces[nextTile].x,freeSpaces[nextTile].y-1))||tiles.Contains(new Vector2(freeSpaces[nextTile].x,freeSpaces[nextTile].y-1))))
				freeSpaces.Add(new Vector2(freeSpaces[nextTile].x,freeSpaces[nextTile].y-1));

			freeSpaces.Remove(freeSpaces[nextTile]); // Remove created tile from free spaces list
		}

        //Place Exit
        int exitTile = (int)Math.Floor(UnityEngine.Random.Range(0F, floor.Count));
        createAndParent(exit, (Vector2)floor[exitTile].position, 0, parent);

		foreach (Vector2 space in freeSpaces) {
			fillWall(space);
		}
        gameManager.floor = floor;
        gameManager.walls = walls;
	}
}
