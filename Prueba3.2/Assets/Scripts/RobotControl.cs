using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotControl : MonoBehaviour
{

    // Discrete Actions
    public int Action1, Action2;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Control();
    }

    private void Control(){


      // Discrete Actions
      int ver = Action1 - 1;
      int hor = Action2 - 1;

      float rleft = 0;
      float rright = 0;


      if(ver == 0){ // Caso 1: No hay avance vertical
  			if(hor == 0){
  				// Caso 1.1: No hay avance horizontal ni vertical
          rleft = 0;
  				rright = 0;
  			}else if(hor > 0){
  				// Caso 1.2: Rotación a la derecha sin avance vertical
          rleft = 85;
  				rright = -85;
  			}else{
  				// Caso 1.3: Rotación a la izquierda sin avance vertical
          rleft = -85;
  				rright = 85;
  			}
  		}else if (ver > 0.1){ // Caso 2: Avance vertical (adelante)
  			if(hor == 0){
  				// Caso 2.1: No hay avance horizontal, solo vertical
          rleft = 100;
  				rright = 100;
  			}else if(hor > 0){
  				// Caso 2.2: Giro a la derecha (combinación de rotación y avance)
          rleft = 100;
  				rright = 45.28f;
  			}else{
  				// Caso 2.3: Giro a la izquierda (combinación de rotación y avance)
          rleft = 43.34f;
  				rright = 100;
  			}
  		}else{
  			if(hor == 0){ // Caso 3: Retroceso vertical (atrás)
  				// Caso 3.1: No hay avance horizontal, solo retroceso vertical
          rleft = -100;
  				rright = -100;
  			}else if(hor > 0){
  				// Caso 3.2: Giro a la derecha hacia atrás (combinación de rotación y retroceso)
          rleft = -100;
  				rright = -45.28f;
  			}else{
  				// Caso 3.3: Giro a la izquierda hacia atrás (combinación de rotación y retroceso)
          rleft = -43.34f;
  				rright = -100;
  			}
  		}

      GetComponent<RobotMove>().Rleft = rleft;
      GetComponent<RobotMove>().Rright = rright;


    }
}
