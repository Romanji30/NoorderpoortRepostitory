using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRemoteControl : MonoBehaviour
{
    [Header("Input Variables")]
    BasicCarController basicCarController;
    public float forwards;
    public float turn;
    
    void Awake()
    {
        basicCarController = GetComponent<BasicCarController>();
    }

    void Update()
    {
        basicCarController.ChangeSpeed(forwards);
        basicCarController.Turn(turn);
    }
}
