# Acceso a los proyectos en Unity del TFG "Entrenamiento de un Robot Mediante Tareas de Aprendizaje Automático"

Estos proyectos se despliegan en la plataforma de desarrollo Unity, y tienen como objetivo poder ejecutar las distintas pruebas que se desarrollaron en el TFG.

## Pasos a Seguir para Desplegar los Proyectos

1. Descargar Unity:

[Unity](https://unity.com/)

En el proyecto se utilizó la versión de 2019.3.14f1 de Unity.

2. Descargar e implementar la librería ML-Agents.

Para ello, seguiremos los pasos de la página web de ML-Agents.

[Unity | ML-Agents | Installation](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Installation.md)

3. Descargar los proyectos en el GIT:

[TFG-Entrenamiento-Robot-Aprendizaje-Automatico](https://github.com/cabannas/TFG-Entrenamiento-Robot-Aprendizaje-Automatico)

Se recomienda descomprimir los proyectos en una carpeta para poder acceder a los proyectos más fácilmente desde el HUB de Unity.

4. Abrir los proyectos en Unity.

A través del HUB de Unity, añadiremos la carpeta del proyecto utilizando la opción "ADD".

A continuación, deberá aparecer el proyecto en la lista de proyectos. Interactuaremos con el proyecto desde el HUB de Unity para abrirlo.

5. Configuraciones en Unity:

A continuación podremos editar las configuraciones del proyecto:

  5.1. En la carpeta Materials encontraremos los elementos "material" de Unity de los componentes del proyecto.
  5.2. En la carpeta ML-Agents se encuentran los ficheros por defecto implementados por ML-Agents.
  5.3. En la carpeta Scripts podemos encontrar los scripts que dictan las configuraciones de los componentes del proyecto.
    Aquí podremos encontrar los siguientes scripts:
      * AgentControl.cs : es el script principal que se comunica con la API de Python, donde están detallados los componentes de la librería de ML-Agents como los observadores, los actuadores y el sistema de recompensas.
      * RobotControl.cs : es el script encargado de traducir las posibles acciones del robot a los porcentajes de los servomotores. El objetivo de este script es de actuar de intermediario entre el movimiento del agente en Unity y el movimiento del robot real.
      * RobotMove.cs : es el script encargado de la configuración de movimiento del agente.
      * Velocimeter.cs : este script se utiliza para monitorizar la Velocidad y la Velocidad Angular del agente.

    Existen otros scripts en algunos proyectos que gestionan la configuración de los laberintos.

6. Ejecutar los entrenamientos usando los proyectos.

Siguiendo los pasos de la guía de ML-Agents que dejamos a continuación podremos entrenar nuevos modelos usando los entornos de los proyectos, y podremos monitorizar este entrenamiento usando la herramienta TensorBoard.

[Unity | ML-Agents | Getting Started Guide](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Getting-Started.md)


## Articulos relacionados y referencias:

[Unity | ML-Agents | Overview](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/ML-Agents-Overview.md#summary-and-next-steps)

[TFG-Entrenamiento-Robot-Aprendizaje-Automatico](https://github.com/cabannas/TFG-Entrenamiento-Robot-Aprendizaje-Automatico)
