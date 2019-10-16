using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Componente que habilita a un objeto para calcular rutas sobre un AStarGraph
/// </summary>
public class AStarNavigator : MonoBehaviour {
    protected AStarGraph _graph; // Estructura con los nodos del grafo que recorreremos

	// Use this for initialization
	void Start () {
        _graph = FindObjectOfType<AStarGraph>();
    }
    public List<Vector2> new_path = new List<Vector2>();//Lista para el camino final
    public List<AStarNode> cerrados = new List<AStarNode>();//Lista de nodos explorados
    /// <summary>
    /// Devuelve el camino para ir desde la posición start hasta end
    /// </summary>
    /// <returns>The path.</returns>
    /// <param name="start">Posición inicial del camino.</param>
    /// <param name="end">Posición objetivo.</param>
    public List<Vector2> GetPath(Vector2 start, Vector2 end) {
        //Redondeamos el valor de start a int
        int xStart = (int)Mathf.Round(start.x);
        int yStart = (int)Mathf.Round(start.y);

        if (!_graph.aux[xStart, yStart])//encontramos piedra despues de redondear
        {
            int x = xStart;
            int y = yStart;
            bool flag = false;//aviso de que encontramos vecino
            for (int i = -1; i < 2&&flag==false; i++)       //
            {                                               //Recorremos vecinos
                for (int j = -1; j < 2&&flag==false; j++)   //
                {
                    if (i != 0 && j != 0 && _graph.aux[x + i, y + j])//Vecino libre
                    {
                        xStart = x + i;
                        yStart = y + j;
                        flag = true;//vecino encontrado
                    }
                }
            }
        }
        start.x = xStart;
        start.y = yStart;
        //
        //Declaramos listas necesarias para calcular camino
        //
        List<AStarNode> abiertos = new List<AStarNode>();//Posibles candidatos
        List<AStarNode> cerrados = new List<AStarNode>();//Nodos explorados
        AStarNode posicionActual = new AStarNode();

        var g = new Dictionary<AStarNode, float>();//suma de costes
        var f = new Dictionary<AStarNode, float>();//coste del camino desde start hasta nodo
        float h;
        var nodopadre = new Dictionary<AStarNode, AStarNode>();//Nodo y su padre
        foreach (AStarNode node in _graph.nodes)//asignamos infinito a todos los valores de g y d a infinito menos el valor g de start que es 0(coste del camino sin empezar)
        {
            g.Add(node, Mathf.Infinity);
            f.Add(node, Mathf.Infinity);
            if (node.position == _graph.nodes[(int)start.x, (int)start.y].position)

            {
                g[node] = 0;
            }
        }
        
        abiertos.Add(_graph.nodes[(int)start.x, (int)start.y]);
        while(abiertos.Count>0)//iteramos para busacar camino
        {
            posicionActual = conseguirAbiertoMin(f,abiertos);//selecionamos el mejor nodo a explorar
            abiertos.Remove(posicionActual);
            cerrados.Add(posicionActual);
            if (esSolucion(posicionActual.position, end))//si estamos en el objetivo construimos el camino
            {
                new_path= construircaminofinal(posicionActual, nodopadre);
                
                return new_path;
            }
            foreach(AStarNode vecino in posicionActual.vecinos)//Revisamos vecinos del nodo que estamos explorando
            {
                if (!abiertos.Contains(vecino)&&!cerrados.Contains(vecino))//si está sin explorar lo añadimos abiertos, caculamos sus valores y le asignamos un padre
                {
                    abiertos.Add(vecino);
                    h = calcularHeuristica(vecino.position,end);

                    g[vecino] = calcularG(posicionActual,vecino,g);
                
                    f[vecino] = calcularf(vecino,g,h);
                    
                    nodopadre.Add(vecino, posicionActual);



                }
                if (calcularG(vecino, posicionActual, g) < g[vecino])//Si el camino es mejor actualizamos los valores y actualizamos padre con mejor camino
                {
                    h = calcularHeuristica(vecino.position, end);
                    g[vecino] = calcularG( posicionActual, vecino, g);
                    
                    f[vecino] = calcularf(vecino, g, h);

                   
                    nodopadre[vecino] = posicionActual;
                    
                    
                }

            }

        }

        // TODO: Implementar
        return new List<Vector2>();
    }

    /// <summary>
    /// Devuelve el camino para ir desde la posición start hasta end. Es un método helper para facilitar la conversión entre Vector2 y Vector3 de los parámetros
    /// </summary>
    /// <returns>The path.</returns>
    /// <param name="start">Posición inicial del camino.</param>
    /// <param name="end">Posición objetivo.</param>
    public List<Vector2> GetPath(Vector3 start, Vector3 end) {
        return GetPath(new Vector2(start.x, start.y), new Vector2(end.x, end.y));
    }
    public bool esSolucion(Vector2 posicion, Vector2 final)//comprueba si es solucion
    {
        if (posicion == final)
        {
            return true;
        }
        return false;
    }

    public float calcularHeuristica(Vector2 actual, Vector2 meta) 
    {
        return Vector2.Distance(actual, meta);
    }

    public float calcularG(AStarNode actual,AStarNode vecino, Dictionary<AStarNode, float> g)
    {
        return g[actual] + Vector2.Distance(vecino.position, actual.position);
    }

    public float calcularf(AStarNode actual, Dictionary<AStarNode, float> g, float h)
    {
        return g[actual] + h;
    }

    public AStarNode conseguirAbiertoMin(Dictionary<AStarNode, float>f, List<AStarNode> abiertos)//devuelve el valor minimo mirando Value
    {
        AStarNode minAbierto=new AStarNode();
        foreach (KeyValuePair<AStarNode, float> peso in f.OrderBy(KeyValuePair => KeyValuePair.Value))
        {
            if (abiertos.Contains(peso.Key))
            {
                minAbierto = peso.Key;
                return minAbierto;
            }
        }
        return minAbierto;

    }
    public List<Vector2> construircaminofinal(AStarNode actual, Dictionary<AStarNode, AStarNode> nodopadre)//contruimos camino a traves de los padres
    {
        
        List<Vector2> final_path = new List<Vector2>
        {
            actual.position
        };
        
        while (nodopadre.ContainsKey(actual))
        {
            
            actual = nodopadre[actual];
            
            final_path.Add(actual.position);
        }
        final_path.Reverse();
       
        return final_path.GetRange(1, final_path.Count - 1);
    }
    
}
