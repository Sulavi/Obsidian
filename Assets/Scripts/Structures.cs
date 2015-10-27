using UnityEngine;
using System;
using System.Collections;

public class Structures : MonoBehaviour {

    public class Entity
    {
        public Vector2 location;
        public Transform entity;

        public Entity(Vector2 loc, Transform ent)
        {
            location = loc;
            entity = ent;
        }
    }
    
}
