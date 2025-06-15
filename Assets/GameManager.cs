using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 10f;
        Time.fixedDeltaTime = 0.02f / Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
