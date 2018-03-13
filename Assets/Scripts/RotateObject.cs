using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour {
    
    [SerializeField]
    float val;
    
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {

        gameObject.transform.Rotate(Vector3.up, val);

    }
}
