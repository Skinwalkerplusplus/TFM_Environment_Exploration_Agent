using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid_score_detection : MonoBehaviour
{
    public int gridSize = 10;
    public Material visitedMaterial;

    public bool[,] visitedCells;
    private GameObject[,] gridVisuals;
    private float cellSize = 1f;

    public float yOffset = 0.01f;

    public GameObject agent;

    // Start is called before the first frame update
    void Start()
    {
        visitedCells = new bool[gridSize, gridSize];
        gridVisuals = new GameObject[gridSize, gridSize];
        CreateGrid();
    }

    void CreateGrid()
    {
        for (int x =0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Quad);
                cell.transform.position = new Vector3(x * cellSize - gridSize / 2f, 0.01f, z * cellSize - gridSize / 2f);
                cell.transform.rotation = Quaternion.Euler(90, 0, 0);
                cell.transform.parent = transform;
                gridVisuals[x, z] = cell;
            }
        }
    }

    void MarkWalked(Vector3 position)
    {
        int x = Mathf.FloorToInt((position.x + gridSize / 2f) / cellSize);
        int z = Mathf.FloorToInt((position.z + gridSize / 2f) / cellSize);

        if (x >= 0 && x < gridSize && z >= 0 && z < gridSize)
        {
            visitedCells[x, z] = true;
            gridVisuals[x, z].GetComponent<Renderer>().material = visitedMaterial;
        }
    }

}
