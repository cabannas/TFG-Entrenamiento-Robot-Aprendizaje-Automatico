using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Wall_Control_3 : MonoBehaviour
{


    public GameObject obj_inner_wall_1;
    public GameObject obj_inner_wall_2;
    public GameObject obj_inner_wall_3;
    public GameObject obj_inner_wall_4;

    public GameObject obj_outer_wall_1;
    public GameObject obj_outer_wall_2;
    public GameObject obj_outer_wall_3;
    public GameObject obj_outer_wall_4;
    public GameObject obj_outer_wall_5;
    public GameObject obj_outer_wall_6;
    public GameObject obj_outer_wall_7;
    public GameObject obj_outer_wall_8;

    private List<GameObject> inner_walls_list = new List<GameObject>();
    private List<GameObject> outer_walls_list = new List<GameObject>();

    // Start is called before the first frame update
    public void PrepareWalls()
    {
      
      inner_walls_list.Add(obj_inner_wall_1);
      inner_walls_list.Add(obj_inner_wall_2);
      inner_walls_list.Add(obj_inner_wall_3);
      inner_walls_list.Add(obj_inner_wall_4);


      outer_walls_list.Add(obj_outer_wall_1);
      outer_walls_list.Add(obj_outer_wall_2);
      outer_walls_list.Add(obj_outer_wall_3);
      outer_walls_list.Add(obj_outer_wall_4);
      outer_walls_list.Add(obj_outer_wall_5);
      outer_walls_list.Add(obj_outer_wall_6);
      outer_walls_list.Add(obj_outer_wall_7);
      outer_walls_list.Add(obj_outer_wall_8);


      // Primero los 4 muros internos activados y los 8 externos desactivados
      foreach (GameObject wall in inner_walls_list){
          wall.SetActive(true);
      }
      foreach (GameObject wall in outer_walls_list){
          wall.SetActive(false);
      }


      // Elegimos 2 padres al hacer y los abrimos
      int randInner1, randInner2;

      randInner1 = Random.Range(0,4);

      do{
        randInner2 = Random.Range(0,4);
      }while (randInner1 == randInner2);

      inner_walls_list[randInner1].SetActive(false);
      inner_walls_list[randInner2].SetActive(false);

      int randOuter1, randOuter2;
      randOuter1 = Random.Range(0,4);
      randOuter2 = 0; // Necesario definirlo para que Unity no se queje

      // A1 y B1 son los hijos de randInner1
      // A2 y B2 son los hijos de randInner2
      // A1 = 0; B1 = 1; A2 = 2; B2 = 3;


      // A continuación se establecen una serie de restricciones entre los muros
      // generados aleatoriamente para evitar cerrar celdas

      if(((randInner1 == 0 || randInner1 == 2) &&
      (randInner2 == 0 || randInner2 == 2))
      ||
      ((randInner1 == 1 || randInner1 == 3) &&
      (randInner2 == 1 || randInner2 == 3))){

      //Si salen las opciones 1-3 o 2-4

        if (randOuter1 == 0){ // No puede ser A1-A1 o A1-B2
          do{
            randOuter2 = Random.Range(0,4);
          }while ((randOuter1 == randOuter2) || (randOuter2 == 3));
        }
        if (randOuter1 == 1){ // No puede ser B1-B1 o B1-A2
          do{
            randOuter2 = Random.Range(0,4);
          }while ((randOuter1 == randOuter2) || (randOuter2 == 2));
        }
        if (randOuter1 == 2){ // No puede ser A2-A2 o A2-B1
          do{
            randOuter2 = Random.Range(0,4);
          }while ((randOuter1 == randOuter2) || (randOuter2 == 1));
        }
        if (randOuter1 == 3){ // No puede ser B2-B2 o B2-A1
          do{
            randOuter2 = Random.Range(0,4);
          }while ((randOuter1 == randOuter2) || (randOuter2 == 0));
        }

      }else{ // Si salen las opciones 1-2, 1-4, 2-3 y 3-4

        if (randOuter1 == 0){ // No puede ser A1-A1 o A1-A2
          do{
            randOuter2 = Random.Range(0,4);
          }while ((randOuter1 == randOuter2) || (randOuter2 == 2));
        }
        if (randOuter1 == 1){ // No puede ser B1-B1 o B1-B2
          do{
            randOuter2 = Random.Range(0,4);
          }while ((randOuter1 == randOuter2) || (randOuter2 == 3));
        }
        if (randOuter1 == 2){ // No puede ser A2-A2 o A2-A1
          do{
            randOuter2 = Random.Range(0,4);
          }while ((randOuter1 == randOuter2) || (randOuter2 == 0));
        }
        if (randOuter1 == 3){ // No puede ser B2-B2 o B2-B1
          do{
            randOuter2 = Random.Range(0,4);
          }while ((randOuter1 == randOuter2) || (randOuter2 == 1));
        }
      }

      int padre1 = randInner1 + 1;
      int padre2 = randInner2 + 1;
      int hijo1, hijo2;

      if (randOuter1 == 0){
        hijo1 = 2 * padre1 - 1;
      }else if (randOuter1 == 1){
        hijo1 = 2 * padre1;
      }else if (randOuter1 == 2){
        hijo1 = 2 * padre2 - 1;
      }else{
        hijo1 = 2 * padre2;
      }

      if (randOuter2 == 0){
        hijo2 = 2 * padre1 - 1;
      }else if (randOuter2 == 1){
        hijo2 = 2 * padre1;
      }else if (randOuter2 == 2){
        hijo2 = 2 * padre2 - 1;
      }else{
        hijo2 = 2 * padre2;
      }

      outer_walls_list[hijo1 - 1].SetActive(true);
      outer_walls_list[hijo2 - 1].SetActive(true);

    }
}
