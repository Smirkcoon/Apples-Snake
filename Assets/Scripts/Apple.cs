using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{  
    void Start()
    {
        FindObjectOfType<SnakeMove>().Aplle = transform; 
    }
}
