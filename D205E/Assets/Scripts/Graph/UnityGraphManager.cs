using Burton.Lib.Graph;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityGraphManager : UnitySingleton<UnityGraphManager>
{
    public List<UnityGraph> Graphs;

    public float TilePadding = .25f;
    public Color DefaultTileColor = Color.white;
    public bool DrawEdges = true;
    public bool DrawTiles = true;
    public string AssetsBasePath = "Data/Graphs";
    public Color DefaultEdgeLineColor = Color.gray;

    #region ctor
    public UnityGraphManager()
    {
    }
    #endregion

    #region Load/Save Graphs
    public void ResetAll()
    {
        foreach (var Graph in Graphs)
        {
            Graph.BuildDefaultGraph();
        }
    }

    public void LoadAll()
    {
        Graphs = new List<UnityGraph>();
        var ResourceObjects = Resources.LoadAll(AssetsBasePath, typeof(UnityGraph));

        foreach (var Resource in ResourceObjects)
        {
            var Graph = Resource as UnityGraph;
            Graphs.Add(Graph);
        }
    }
  
    public void Load(string GraphName)
    {

    }
    #endregion

    #region Graphs/Nodes
    public void RemoveNode(int NodeIndex)
    {
        Graphs[0].Graph.RemoveNode(NodeIndex);
      
    }
    #endregion

    #region Drawing
  
    #endregion
}
