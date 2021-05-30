using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMove : MonoBehaviour
{

    private Rigidbody _rigidbody;

    private void Awake() => _rigidbody = GetComponent<Rigidbody>();

    public float Rleft, Rright;
    public float Vlin, Vang;


    // Start is called before the first frame update
    void Start()
    {

    }

    void FixedUpdate()
    {

      Move();
    }


    /*TODO: añadir movimiento complejo de avance vertical y horizontal*/

    /*
    Move se utiliza para mover el robot en función del porcentaje recibido
    para cada motor (izquierdo o derecho)
    */

    private void Move(){

      Convert();

      /*
        Para conseguir la velocidad exacta e instantanea usando AddForce es
        necesario hacer la operación Vlin - VelocityChange
        Se usa VelocityChange porque aplica el cambio de velocidad sin tener en
        cuenta la masa (ver API de Rigidbody)

        Como unity tambien tiene factores de rozamiento, se ha calculado por
        proximidad que el valor "0.4825" es el indicado para alcanzar los
        0.247 m/s del robot real.
      */

      _rigidbody.AddForce(Vlin * transform.forward - _rigidbody.velocity,
                          ForceMode.VelocityChange);

      _rigidbody.AddTorque(Vang * transform.up, ForceMode.VelocityChange);

    }

    private void Convert(){

      float move_front = Rleft + Rright;
      float rotate = Rleft - Rright;

      /*
      Estos son los valores calculados para poder alcanzar una velocidad
      lineal de 0.247 m/s que son la velocidad del robot real

      0.4825 para 0.247 m/s
      0.359 para 0.1235 m/s
      */

      if (move_front > 0){
        if(rotate != 0){
          Vlin = 0.359f;
        }else{
          Vlin = 0.4825f;
        }
      }

      else if (move_front < 0){
        if(rotate != 0){
          Vlin = -0.359f;
        }else{
          Vlin = -0.4825f;
        }
      }
      else
        Vlin = 0;

      /*
      Estos son los valores calculados para alcanzar las velocidades
      angulares del robot real, en rotación parada es de 1.57 rad/s y la
      rotación en giro es de 0.523 rad/s
      */

      if (rotate > 150) // Rotación sin avance izquierda
        Vang = 1.046f;
      else if (rotate < -150) // Rotación sin avance derecha
        Vang = -1.046f;
      else if (rotate > 56 && rotate < 150) // Giro a la derecha retrocediendo
        Vang = 0.349f;
      else if (rotate > 0 && rotate < 56) // Giro izquierda avanzando
        Vang = 0.349f;
      else if (rotate < -56 && rotate > -150) // Giro a la derecha avanzando
        Vang = -0.349f;
      else if (rotate < 0 && rotate > -56) // Giro izquierda retrocediendo
        Vang = -0.349f;
      else
        Vang = 0;

    }

  }
