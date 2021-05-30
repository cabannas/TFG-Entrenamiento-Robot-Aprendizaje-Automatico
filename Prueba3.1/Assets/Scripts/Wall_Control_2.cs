using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Wall_Control_2 : MonoBehaviour
{

    public GameObject obj_outer_wall_1;
    public GameObject obj_outer_wall_2;
    public GameObject obj_outer_wall_3;
    public GameObject obj_outer_wall_4;

    public void PrepareWalls()
    {
      List<GameObject> outer_walls_list = new List<GameObject>();

      outer_walls_list.Add(obj_outer_wall_1);
      outer_walls_list.Add(obj_outer_wall_2);
      outer_walls_list.Add(obj_outer_wall_3);
      outer_walls_list.Add(obj_outer_wall_4);

      // Primero desactivamos todos los muros
      foreach (GameObject wall in outer_walls_list){
          wall.SetActive(false);
      }


      // Elegimos un muro aleatorio a partir de su indice en la lista
      int randWall;

      randWall = Random.Range(0,4);

      outer_walls_list[randWall].SetActive(true);
    }
}
