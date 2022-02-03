using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class xPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Look Left / Right
        if (Input.GetKeyUp(KeyCode.Q))
        {
            Debug.Log("Q");
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            Debug.Log("E");
        }
        // Left / Right
        if (Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log("D");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("A");
        }
        // Forward / Back
        if (Input.GetKeyUp(KeyCode.W))
        {
            Debug.Log("W");
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Debug.Log("S");
        }
        // Left Hand
        if (Input.GetKeyUp(KeyCode.Z))
        {
            // PRIMARY TRIGGER
            Debug.Log("Z");
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            // SECONDARY TRIGGER
            Debug.Log("X");
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            // DIRECTION (Closes Fist)
            Debug.Log("C");
        }
        // Right Hand
        if (Input.GetKeyUp(KeyCode.Z))
        {
            // PRIMARY TRIGGER
            Debug.Log("B");
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            // SECONDARY TRIGGER
            Debug.Log("N");
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            // DIRECTION (Closes Fist)
            Debug.Log("M");
        }
    }
}
