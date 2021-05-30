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

    // El numero de suelos totales
    private int floors2Reach = 9;
    // El numero de suelos acertados correctamente
    private int floorsReached = 0;

    public bool hitting_fw = false, hitting_left = false, hitting_right = false;


    public GameObject Floor1, Floor2, Floor3, Floor4, Floor5,
                      Floor6, Floor7, Floor8, Floor9;

    public Wall_Control_3 wallScriptInstance;


    // Start is called before the first frame update
    void Start () {
        rBody = GetComponent<Rigidbody>();

    }



    public override void OnEpisodeBegin(){

      // Ponemos todos los suelos rojos
      Floor1.GetComponent<Renderer>().material.color = Color.red;
      Floor2.GetComponent<Renderer>().material.color = Color.red;
      Floor3.GetComponent<Renderer>().material.color = Color.red;
      Floor4.GetComponent<Renderer>().material.color = Color.red;
      Floor5.GetComponent<Renderer>().material.color = Color.red;
      Floor6.GetComponent<Renderer>().material.color = Color.red;
      Floor7.GetComponent<Renderer>().material.color = Color.red;
      Floor8.GetComponent<Renderer>().material.color = Color.red;
      Floor9.GetComponent<Renderer>().material.color = Color.red;

      // Reseteamos el número de suelos acertados
      floorsReached = 0;

      wallScriptInstance.GetComponent<Wall_Control_3>().PrepareWalls();

      //Constraints de tiempo ====================

      sw.Start();

      // If agents falls
      if (this.transform.localPosition.y < 0){

        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3( 0, 0.1f, 0);
      }

      if (maxTime == true)
      {
          maxTime = false;
          // If time out
          this.rBody.angularVelocity = Vector3.zero;
          this.rBody.velocity = Vector3.zero;
          this.transform.localPosition = new Vector3( 0, 0.1f, 0);
      }

    }

    public override void CollectObservations(VectorSensor sensor){

      // Agent velocity
      sensor.AddObservation(rBody.velocity.x);
      sensor.AddObservation(rBody.velocity.z);

      // Amount of targets reached
      sensor.AddObservation(floorsReached/floors2Reach);
      sensor.AddObservation(hitting_fw);
      sensor.AddObservation(hitting_left);
      sensor.AddObservation(hitting_right);
    }

    public override void OnActionReceived(float[] vectorAction){

        // For each step, negative reward to avoid spinning out of control
        if(vectorAction[0] == 0){
          // We punish more if robot goes backward to avoid backwalking
          AddReward(-0.0001f);
        }else{
          AddReward(-0.00001f);
        }

        // Discrete Actions
        GetComponent<RobotControl>().Action1 = Mathf.FloorToInt(vectorAction[0]);
        GetComponent<RobotControl>().Action2 = Mathf.FloorToInt(vectorAction[1]);

        // Implement raycast pointing to the floor to know if agent has
        // reached the target

        Vector3 vector_raycast_down = transform.TransformDirection(Vector3.down);
        Vector3 vector_raycast_fw = transform.TransformDirection(Vector3.forward);
        Vector3 vector_raycast_left = transform.TransformDirection(-1,0,1);
        Vector3 vector_raycast_right = transform.TransformDirection(1,0,1);

        RaycastHit hit_down;
        RaycastHit hit_fw;
        RaycastHit hit_left;
        RaycastHit hit_right;

        // A color that is not red or green, to compare with red
        Color floorColor = Color.blue;

        // Raycast down --> To show if the robot is in the red platform or not
        if (Physics.Raycast(transform.position, vector_raycast_down, out hit_down, 0.1f)){
            floorColor = hit_down.transform.gameObject.GetComponent<Renderer>().material.color;
         }
        if (floorColor == Color.red){
          floorsReached += 1;
          // Añadimos una reward cada vez mayor
          AddReward(floorsReached/floors2Reach);
          // Transformamos el suelo a verde
          hit_down.transform.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }

        // Raycast forward --> To know if the robot is near to a border
        if (Physics.Raycast(transform.position, vector_raycast_fw, out hit_fw, 0.3f)){
          hitting_fw = true;
          if(vectorAction[0] == 0){
            // We award going backward if robot is near wall
            AddReward(0.0001f);
          }else{
            AddReward(-0.0002f);
          }
        // Raycast forward --> To know if the robot is hitting a border
        }else if (Physics.Raycast(transform.position, vector_raycast_fw, out hit_fw, 0.2f)){
          hitting_fw = true;
            // Bigger punishment
            AddReward(-0.001f);
         }else{
           hitting_fw = false;
         }

         // Raycast left --> To know if the robot is near to a border
         if (Physics.Raycast(transform.position, vector_raycast_left, out hit_left, 0.3f)){
            hitting_left = true;
            AddReward(-0.0002f);

         // Raycast left --> To know if the robot is hitting a border
        }else if (Physics.Raycast(transform.position, vector_raycast_left, out hit_left, 0.2f)){
            hitting_left = true;
             // Bigger punishment
             AddReward(-0.001f);
          }else{
            hitting_left = false;
          }
          // Raycast right --> To know if the robot is near to a border
          if (Physics.Raycast(transform.position, vector_raycast_right, out hit_right, 0.3f)){
            hitting_right = true;
             AddReward(-0.0002f);

          // Raycast left --> To know if the robot is hitting a border
        }else if (Physics.Raycast(transform.position, vector_raycast_right, out hit_right, 0.2f)){
            hitting_right = true;
              // Bigger punishment
              AddReward(-0.001f);
           }else{
             hitting_right = false;
           }

        if (floorsReached == floors2Reach){

            //Constraints de tiempo
            SetReward(1.0f);
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

      // Discrete Actions
      actionsOut[0] = Mathf.FloorToInt(Input.GetAxis("Vertical")) + 1;
      actionsOut[1] = Mathf.FloorToInt(Input.GetAxis("Horizontal")) + 1;


    }


}
