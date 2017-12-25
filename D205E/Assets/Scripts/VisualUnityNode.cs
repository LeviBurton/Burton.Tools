using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualUnityNode : MonoBehaviour
{
    public UnityNode UnityNode;

    Color OriginalColor;

    public void OnMouseEnter()
    {
        OriginalColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.white; 
    }

    public void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = OriginalColor;
    }
}
