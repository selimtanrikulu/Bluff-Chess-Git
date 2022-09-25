using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lines : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    float startY;
    [SerializeField] float maxY;

    // Start is called before the first frame update
    void Start()
    {
        startY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        Ascend();
    }

    void Ascend()
    {
        Vector3 pos = transform.localPosition;
        pos.y += movementSpeed*Time.deltaTime;
        if(pos.y > maxY) pos.y = startY;
        transform.localPosition = pos;
    }
}
