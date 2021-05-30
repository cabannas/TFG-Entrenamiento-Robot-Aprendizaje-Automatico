using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Velocimeter : MonoBehaviour
{


    public float velocity, angular_velocity;

    private Rigidbody _rigidbody;

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        velocity = _rigidbody.velocity.magnitude;
        angular_velocity = _rigidbody.angularVelocity.magnitude;
    }
}
