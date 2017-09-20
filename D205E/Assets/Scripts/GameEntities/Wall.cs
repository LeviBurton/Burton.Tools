using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    public int Length;
    public int Width;
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
        Gizmos.color = Color;
        if (Length > 0)
        {
          
            for (int x = 1; x <= Length; x++)
            {

                Gizmos.DrawWireCube(DrawPosition, Vector3.one * 2);
                DrawPosition.x += 2.0f;
            }
        }
        if (Width > 0)
        {
            DrawPosition = transform.position;
            for (int y = 1; y <= Width; y++)
            {
                Gizmos.DrawWireCube(DrawPosition, Vector3.one * 2);
                DrawPosition.z += 2.0f;
            }
        }
    }
}
