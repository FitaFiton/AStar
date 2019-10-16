using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Modela el enlace entre dos nodos de un grafo
/// </summary>
public class AStarConnection {
    public float distance = 1f; // Peso de la arista, por defecto será 1 para todas, salvo que la implementación elegida requiera otros valores
    public AStarNode otherNode; // Nodo con el que nos conectamos
}