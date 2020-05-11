using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;


    float movementFactor;  //0 not move -- 1 fully move
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
        if (period <= Mathf.Epsilon) return; //code defensive against 0

        float cycles = Time.time / period;
        const float tau = 2 * Mathf.PI;
        float rawSinWave = Mathf.Sin(cycles * tau);  //output from -1 to +1 
        movementFactor = (rawSinWave / 2) + 0.5f;

        Vector3 offset = movementVector * movementFactor;

        transform.position = startPos + offset;
    }
}
