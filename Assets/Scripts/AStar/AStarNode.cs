using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Almacena los datos de un nodo del grafo
/// </summary>
public class AStarNode {
    public Vector2 position; // Posición dentro del mundo que ocupa este nodo
    /// <summary>
    /// Ejemplos de conexiones a otros nodos.
    /// TODO: Codificar según la implementación escogida
    /// </summary>
    public AStarConnection north, north_east, east, south_east, south, south_west, west, north_west;

    public List<AStarNode> vecinos=new List<AStarNode>();
  
    public AStarNode()
    {
        north = new AStarConnection();
        north_west = new AStarConnection();
        west = new AStarConnection();
        south_west = new AStarConnection();
        south = new AStarConnection();
        south_east = new AStarConnection();
        east = new AStarConnection();
        north_east = new AStarConnection();
        
        
    }
}