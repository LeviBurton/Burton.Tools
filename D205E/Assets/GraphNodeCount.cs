using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Burton.Lib.Unity;

public class GraphNodeCount : MonoBehaviour {

    public UnityGraph Graph;

	// Use this for initialization
	void Start () {
        var Text = GetComponent<Text>();
        Text.text = Graph.Graph.ActiveNodeCount().ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
