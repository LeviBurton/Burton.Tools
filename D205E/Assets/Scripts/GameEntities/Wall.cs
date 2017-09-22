using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public int Length;
    public int Width;
    public float Height;

    public Color Color;

    private Vector3 Size = new Vector3(1, 1, 1);

	// Use this for initialization
	void Start () {
	    	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDrawGizmos()
    {
        var DrawPosition = transform.position;
        var UnityGraph = FindObjectOfType<UnityGraph>();
        var DrawSize = new Vector3();

        DrawPosition.x += UnityGraph.TileWidth / 2;
        DrawPosition.z += UnityGraph.TileHeight / 2;
        DrawPosition.y += (float)Height / 2.0f;

        Gizmos.color = Color;

        DrawSize.y = Height;
        DrawSize.x = UnityGraph.TileWidth;
        DrawSize.z = UnityGraph.TileHeight; 

        if (Length > 0)
        {
            for (int x = 1; x <= Length; x++)
            {
                Gizmos.DrawWireCube(DrawPosition, DrawSize);
                DrawPosition.x += UnityGraph.TileWidth; // Account for origin being bottom left
            }
        }
        if (Width > 0)
        {
            DrawPosition = transform.position;
            for (int y = 1; y <= Width; y++)
            {
                Gizmos.DrawWireCube(DrawPosition, DrawSize);
                DrawPosition.z += UnityGraph.TileHeight;
            }
        }
    }
}
