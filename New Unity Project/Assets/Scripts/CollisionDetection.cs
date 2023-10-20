using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float go_direction = 0;
        int go_weight = 0;

        GameObject[] birds;
        birds = GameObject.FindGameObjectsWithTag("Bird");

        float angle;
        float dist;

        foreach (GameObject bird in birds)
        {
            if (bird.transform.position != transform.position) //if another bird
            {
                angle = Vector2.Angle(bird.transform.position - transform.position, transform.up);
                dist = Vector3.Distance(bird.transform.position, transform.position);

                if (angle < 35 && dist < 2) //visable to bird
                {
                    if (dist < 0.01)
                    {
                        dist = (float) 0.01;
                    }
                    Debug.DrawLine(bird.transform.position, transform.position, Color.white);
                    
                    float want = (-angle/45 +1) * (-dist/2 +1) * Vector2.Dot(bird.transform.position - transform.position, transform.right);
                    go_weight++;
                    go_direction = (go_direction*(go_weight-1) + want*90) / go_weight;
                    



                }
            }
            
        }
        Debug.Log(go_direction);
        Vector3 wanted_angle = new Vector3(0,0, transform.rotation.z + go_direction);
        Debug.DrawLine(wanted_angle + transform.position, transform.position, Color.green);
        transform.Rotate(0,0,go_direction);

    }




    
}
