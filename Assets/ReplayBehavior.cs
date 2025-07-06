using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PositionData
{
    public List<Vector3> positions = new List<Vector3>();
}


public class ReplayBehavior : MonoBehaviour
{
    public bool isRecording;
    public float recordInterval = 0.2f;

    public Transform agent;

    private PositionData playedPositions = new PositionData();

    void Start()
    {
        
    }

    void Update()
    {
        if (isRecording)
        {
            StartCoroutine(LogPositionEverySecond());
        }

        else
        {
            StopAllCoroutines();
        }
    }

    public IEnumerator LogPositionEverySecond()
    {
        while (true)
        {
            playedPositions.positions.Add(agent.position);

            yield return new WaitForSeconds(recordInterval);
        }
    }

    void OnDestroy()
    {
        string json = JsonUtility.ToJson(playedPositions);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/played_positions.json", json);
        Debug.Log("exported JSON to " + Application.persistentDataPath + "/played_positions.json");
    }
}
