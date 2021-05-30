using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;
using System.Diagnostics;
using Random = UnityEngine.Random;

public class AgentBackup : Agent
{

    Rigidbody rBody;

    private void Awake() => rBody = GetComponent<Rigidbody>();

    // Constraints de tiempo
    public float timeOut = 10;

    Stopwatch sw = new Stopwatch();

    private bool maxTime = false;

    public Transform Target;

    public bool floor_reached = false;

    // Start is called before the first frame update
    void Start () {
        rBody = GetComponent<Rigidbody>();

        // Target Color is red
        Target.GetComponent<Renderer>().material.color = Color.red;

    }

    public override void OnEpisodeBegin(){

      sw.Start();

      if (maxTime == true)
      {
          maxTime = false;
          // If time out
          this.rBody.angularVelocity = Vector3.zero;
          this.rBody.velocity = Vector3.zero;
          this.transform.localPosition = new Vector3( 0, 0.1f, 0);
      }

      // Move the target to a new spot
        Target.localPosition = new Vector3(Random.value * 6 - 4,
                                           0.001f,
                                           Random.value * 6 - 4);

    }

    public override void CollectObservations(VectorSensor sensor){

      // Agent velocity
      sensor.AddObservation(rBody.velocity.x);
      sensor.AddObservation(rBody.velocity.z);

      sensor.AddObservation(floor_reached);
    }

    public override void OnActionReceived(float[] vectorAction){


      // For each step, negative reward to avoid spinning out of control
      if(vectorAction[0] == 0){
        // We punish more if robot goes backward to avoid backwalking
        AddReward(-0.0001f);
      }else{
        AddReward(-0.00001f);
      }
      // Turning means a waste of energy, we have to make it as eficient as
      // possible
      if(vectorAction[1] != 1){
        AddReward(-0.00004f); //This adds to the previous negative reward
      }

        // Continuous Actions
        /*
        GetComponent<RobotControl>().Action1 = vectorAction[0];
        GetComponent<RobotControl>().Action2 = vectorAction[1];
        */

        // Discrete Actions

        GetComponent<RobotControl>().Action1 = Mathf.FloorToInt(vectorAction[0]);
        GetComponent<RobotControl>().Action2 = Mathf.FloorToInt(vectorAction[1]);


        // Implement raycast pointing to the floor to know if agent has
        // reached the target

        Vector3 vector_raycast_down = transform.TransformDirection(Vector3.down);
        Vector3 vector_raycast_fw = transform.TransformDirection(Vector3.forward);

        RaycastHit hit_down;
        RaycastHit hit_fw;

        // A color that is not red or green, to compare with red
        Color floorColor = Color.blue;

        // Raycast down --> To show if the robot is in the red platform or not
        if (Physics.Raycast(transform.position, vector_raycast_down, out hit_down, 0.1f)){
            floorColor = hit_down.transform.gameObject.GetComponent<Renderer>().material.color;
         }
        if (floorColor == Color.red){
          floor_reached = true;
        }else{
          floor_reached = false;
        }

        // Raycast forward --> To know if the robot is near to a border
        if (Physics.Raycast(transform.position, vector_raycast_fw, out hit_fw, 0.3f)){
          if(vectorAction[0] == 0){
            // We award going backward if robot is near wall
            AddReward(0.0001f);
          }else{
            AddReward(-0.0002f);
          }
         }
        // Raycast forward --> To know if the robot is hitting a border
        if (Physics.Raycast(transform.position, vector_raycast_fw, out hit_fw, 0.2f)){
            // Bigger punishment
            AddReward(-0.001f);
         }

        if (floor_reached){

          SetReward(1.0f);

          //Constraints de tiempo
          sw.Stop();
          sw.Reset();

          EndEpisode();
        }

        // Agent can fall?
        if (this.transform.localPosition.y < 0)
        {
          sw.Stop();
          sw.Reset();

          EndEpisode();
        }


        //Constraints de tiempo
        if (sw.ElapsedMilliseconds > 1000*timeOut)
        {
            sw.Stop();
            sw.Reset();
            maxTime = true;
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut){

      // Continuous Actions
      /*
      actionsOut[0] = Input.GetAxis("Vertical");
      actionsOut[1] = Input.GetAxis("Horizontal");
      */
      // Discrete Actions

      actionsOut[0] = Mathf.FloorToInt(Input.GetAxis("Vertical")) + 1;
      actionsOut[1] = Mathf.FloorToInt(Input.GetAxis("Horizontal")) + 1;


    }


}
