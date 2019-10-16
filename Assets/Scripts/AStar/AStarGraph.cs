using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Funciona como modelo de nuestro grid de A*
/// </summary>
public class AStarGraph : MonoBehaviour {
    public Vector2 center;
    public Vector2Int mapSize;

    public AStarNode[,] nodes;
    public bool[,] aux;

    /// <summary>
    /// Crea el grafo que utilizará el A* a partir del estado de la escena
    /// </summary>
    public void Scan() {
        var startTime = Time.realtimeSinceStartup;
        // Ejemplo de esqueleto de función para generar el grafo del espacio de navegación
        nodes = new AStarNode[mapSize.x, mapSize.y];
        aux = new bool[mapSize.x, mapSize.y];

        // Vemos en qué posiciones del mapa tenemos obstáculos
        // TODO: Implementar para la solución concreta que hayamos elegido

        for (int x = 0; x < mapSize.x; x++) {
            for (int y = 0; y < mapSize.y; y++) {
                nodes[x, y] = new AStarNode();
                nodes[x, y].position = new Vector2(x, y);
                var hit = Physics2D.Raycast(new Vector2(x - .5f, y), Vector2.right, 1f, 1 << Layers.Obstacles);
                if (hit.collider != null) {
                    // Hemos tocado un obstáculo
                    aux[x, y] = false;
                } else {
                    // No hay obstáculos
                    aux[x, y] = true;
                }
            }
        }

        for (int x = 0; x < mapSize.x; x++) {//vuelta para buscar vecinos
            for (int y = 0; y < mapSize.y; y++) {
                if (!aux[x, y]) {
                    continue;
                }
                var node = nodes[x, y];

                if (y < mapSize.y - 1 && aux[x, y + 1] == true)
                {
                    node.north = new AStarConnection();
                    node.north.otherNode = nodes[x, y + 1];
                    node.vecinos.Add(node.north.otherNode);
                }
                if (y > 0 && aux[x, y - 1] == true)
                {
                    node.south = new AStarConnection();
                    node.south.otherNode = nodes[x, y - 1];
                    node.vecinos.Add(node.south.otherNode);
                }

                if (x > 0 && aux[x - 1, y] == true)
                {
                    node.west = new AStarConnection();
                    node.west.otherNode = nodes[x - 1, y];
                    node.vecinos.Add(node.west.otherNode);
                }
                if (x < mapSize.x - 1 && aux[x + 1, y] == true)
                {
                    node.east = new AStarConnection();
                    node.east.otherNode = nodes[x + 1, y];
                    node.vecinos.Add(node.east.otherNode);
                }
                if (x < mapSize.x - 1 && y < mapSize.y - 1 
                    && aux[x + 1, y + 1] == true && (aux[x + 1, y] || aux[x, y + 1])) 
                {
                    node.north_east = new AStarConnection();
                    node.north_east.otherNode = nodes[x + 1, y + 1];
                    node.vecinos.Add(node.north_east.otherNode);
                }
                if (x > 0 && y < mapSize.y - 1 && aux[x - 1, y + 1] == true 
                    && (aux[x - 1, y] || aux[x, y + 1])) 
                {
                    node.north_west = new AStarConnection();
                    node.north_west.otherNode = nodes[x - 1, y + 1];
                    node.vecinos.Add(node.north_west.otherNode);
                }

                if (x < mapSize.x - 1 && y > 0 && aux[x + 1, y - 1] == true 
                    && (aux[x + 1, y] || aux[x, y - 1])) 
                {
                    node.south_east = new AStarConnection();
                    node.south_east.otherNode = nodes[x + 1, y - 1];
                    node.vecinos.Add(node.south_east.otherNode);
                }
                if (x > 0 && y > 0 && aux[x - 1, y - 1] == true && (aux[x - 1, y] || aux[x, y - 1])) 
                {
                    node.south_west = new AStarConnection();
                    node.south_west.otherNode = nodes[x - 1, y - 1];
                    node.vecinos.Add(node.south_west.otherNode);
                }
                
            }
        }

        Debug.Log("Tiempo empleado en el scan: " + (Time.realtimeSinceStartup - startTime) + " segundos");

    }

    
}