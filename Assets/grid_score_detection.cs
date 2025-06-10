using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid_score_detection : MonoBehaviour
{
    public int gridSize = 10;
    public Material visitedMaterial;

    public bool[,] visitedCells;
    private GameObject[,] gridVisuals;

    public float yOffset = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        visitedCells = new bool[gridSize, gridSize];
        gridVisuals = new GameObject[gridSize, gridSize];
    }

    void CreateGrid()
    {

    }

}
