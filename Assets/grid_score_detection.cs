using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grid_score_detection : MonoBehaviour
{
    public int gridSize = 10;
    public Material visitedMaterial;
    public Material nonVisitedMaterial;

    public bool[,] visitedCells;
    private GameObject[,] gridVisuals;
    public float cellSize = 1f;

    public float yOffset = 0.01f;

    public Transform agent;

    public GameObject lastVisitedCell = null;

    public Transform startingGridPosition;

    // Start is called before the first frame update
    void Start()
    {
        visitedCells = new bool[gridSize, gridSize];
        gridVisuals = new GameObject[gridSize, gridSize];
        CreateGrid();
        Debug.Log("passing_grid");
    }

    void FixedUpdate()
    {

    }

    void CreateGrid()
    {
        for (int x =0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Quad);
                cell.layer = LayerMask.NameToLayer("agent");
                cell.transform.position = startingGridPosition.position + new Vector3(x * cellSize - gridSize / 2f, yOffset, z * cellSize - gridSize / 2f);
                cell.transform.rotation = Quaternion.Euler(90, 0, 0);
                cell.transform.parent = transform;
                gridVisuals[x, z] = cell;
            }
        }

        ResetGrid();
    }

    public void MarkWalked(Vector3 position)
    {

        int x = Mathf.FloorToInt((position.x - startingGridPosition.position.x + gridSize / 2f) / cellSize);
        int z = Mathf.FloorToInt((position.z - startingGridPosition.position.z + gridSize / 2f) / cellSize);

        if (x >= 0 && x < gridSize && z >= 0 && z < gridSize)
        {
            visitedCells[x, z] = true;
            gridVisuals[x, z].GetComponent<Renderer>().material = visitedMaterial;
        }

    }

    public bool CheckWalked(Vector3 position)
    {
        int x = Mathf.FloorToInt((position.x - startingGridPosition.position.x + gridSize / 2f) / cellSize);
        int z = Mathf.FloorToInt((position.z - startingGridPosition.position.z + gridSize / 2f) / cellSize);

        return visitedCells[x, z];
    }

    public void ResetGrid()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                visitedCells[x, z] = false;
                gridVisuals[x, z].GetComponent<Renderer>().material = nonVisitedMaterial;
            }
        }
    }

    public bool CurrentCell(Vector3 position)
    {
        int x = Mathf.FloorToInt((position.x - startingGridPosition.position.x + gridSize / 2f) / cellSize);
        int z = Mathf.FloorToInt((position.z - startingGridPosition.position.z + gridSize / 2f) / cellSize);

        if (lastVisitedCell == gridVisuals[x, z])
        {
            lastVisitedCell = gridVisuals[x, z];
            return true;
        }

        else
        {
            lastVisitedCell = gridVisuals[x, z];
            return false;
        }
    }

}
