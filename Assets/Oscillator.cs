using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;

    [SerializeField] [Range(0,1)] float movementFactor;  //0 not move -- 1 fully move

    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = movementVector * movementFactor;

        transform.position = startPos + offset;
    }
}
