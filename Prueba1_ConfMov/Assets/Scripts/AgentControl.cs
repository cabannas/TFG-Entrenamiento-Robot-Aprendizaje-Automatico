using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;
using System.Diagnostics;
using Random = UnityEngine.Random;

public class AgentControl : Agent
{

    Rigidbody rBody;

    private void Awake() => rBody = GetComponent<Rigidbody>();

    // Constraints de tiempo
    public float timeOut = 10;

    Stopwatch sw = new Stopwatch();

    private bool maxTime = false;

    public Transform Target;


    // Start is called before the first frame update
    void Start () {
        rBody = GetComponent<Rigidbody>();

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
        Target.localPosition = new Vector3(Random.value * 8 - 4,
                                           0.5f,
                                           Random.value * 8 - 4);

    }

    public override void CollectObservations(VectorSensor sensor){

      // Target and Agent positions
      sensor.AddObservation(Target.localPosition);
      sensor.AddObservation(this.transform.localPosition);

      // Agent velocity
      sensor.AddObservation(rBody.velocity.x);
      sensor.AddObservation(rBody.velocity.z);
    }

    public override void OnActionReceived(float[] vectorAction){


        // For each step, negative reward to avoid spinning out of control
        // Discrete
        if(vectorAction[0] == 0){
        // Continuous
        //if(vectorAction[0] < 0){
          // We punish more if robot goes backward to avoid backwalking
          AddReward(-0.001f);
        }else{
          AddReward(-0.0001f);
        }

        // Continuous Actions
        /*
        GetComponent<RobotControl>().Action1 = vectorAction[0];
        GetComponent<RobotControl>().Action2 = vectorAction[1];
        */

        // Discrete Actions

        GetComponent<RobotControl>().Action1 = Mathf.FloorToInt(vectorAction[0]);
        GetComponent<RobotControl>().Action2 = Mathf.FloorToInt(vectorAction[1]);


        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        if (distanceToTarget < 1.42f){

            SetReward(1.0f);

            //Constraints de tiempo
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
