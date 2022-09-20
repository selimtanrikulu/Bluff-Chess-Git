using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public class SceneObject
{
    public GameObject trailerGameObject;
    public float startTime;
    public float endTime;

    public SceneObject(GameObject trailerGameObject,float startTime,float endTime)
    {
        this.trailerGameObject = trailerGameObject;
        this.startTime = startTime;
        this.endTime = endTime;

    }

}




public class TrailerScenarist : MonoBehaviour
{

    [SerializeField] GameObject[] trailerGameObjects;
    [SerializeField] float[] startTimes;
    [SerializeField] float[] endTimes;

    float timePast = 0;

    List<SceneObject> sceneObjects = new List<SceneObject>();

    void Start()
    {
        for(int i=0;i<trailerGameObjects.Length;i++)
        {
            sceneObjects.Add(new SceneObject(trailerGameObjects[i],startTimes[i],endTimes[i]));
        }

    }

    void Update()
    {

        timePast += Time.deltaTime;
        CheckSceneObjects();
    }


    void CheckSceneObjects()
    {
        foreach(SceneObject sceneObject in sceneObjects)
        {
            if(sceneObject.startTime <= timePast)
            {
                sceneObject.trailerGameObject.SetActive(true);
            }

            if(sceneObject.endTime <= timePast)
            {
                sceneObject.trailerGameObject.SetActive(false);
            }
        }
    }
  

}
