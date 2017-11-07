using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour {

    [SerializeField]
    private float OscillateSpeed = 1.0f;
    [SerializeField]
    private float OscillateDistance= 1.0f;


    //-----------------------------------Unity Functions-----------------------------------

    private void Update()
    {
        var pos = this.transform.position;
        pos.x = Mathf.Sin(Time.time * OscillateSpeed) * OscillateDistance;
        this.transform.position = pos;
    }
}
