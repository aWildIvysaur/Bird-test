using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
        static float multiplier = 0.9f;
        static float Speed = 0.05f * multiplier;
        static float Turn_Speed_Wall = 0.4f * multiplier;
        static float Turn_Speed_Birds = 0.6f *  multiplier;
        static int Sight_Angle = 95;
        static float Sight_Distance = 3f;
        static float Sight_Danger_Distance = 2f;

        System.Random random = new System.Random(); 
        int direction = 0; //-1 left, 0 none, 1 right


    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {     
        
        float temp = avoidWalls(Turn_Speed_Wall);
        float total_rotation = temp;
        
        transform.Rotate(0,0, temp); //rotate away from walls      

        Debug.DrawRay(transform.position, transform.up);
        temp = avoidBirds(Turn_Speed_Birds, Sight_Angle, Sight_Danger_Distance);
        Debug.DrawRay(transform.position, transform.up);

        transform.Rotate(0,0, temp);

        total_rotation += temp;

        /* if (total_rotation == 0)
        {
            int rndNumber;
            if (direction == -1)
            {
                rndNumber = random.Next(-5, 0);
            }
            else if (direction == 1)
            {
                rndNumber = random.Next(0, 5);
            }
            else
            {
                rndNumber = random.Next(-5, 5);
                if (rndNumber < 0)
                {
                    direction = -1;
                }
                else if (rndNumber > 0)
                {
                    direction = 1;
                }
            }
            
            if (rndNumber == 0)
            {
                direction = 0;
            }
            transform.Rotate(0,0, rndNumber);
        }
        else
        {
            direction = 0;
         }*/

        transform.Translate(transform.up * Speed, Space.World);
    }

    float avoidBirds(float Turn_Speed, int Sight_Angle, float danger_distance)
    {
        float go_direction = 0;
        int go_weight = 0;
        float angle;
        float dist;
        GameObject[] birds = GameObject.FindGameObjectsWithTag("Bird");
        foreach (GameObject bird in birds)
        {
            if (bird.transform != transform) //if another bird
            {
                angle = Vector2.Angle(bird.transform.position - transform.position, transform.up);
                dist = Vector3.Distance(bird.transform.position, transform.position);

                if (angle < Sight_Angle && dist < danger_distance) //visable to bird
                {
                    if (dist < 0.01)
                    {
                        dist = (float) 0.01;
                    }                    
                    float want = (-angle/Sight_Angle +1) * (-dist/danger_distance +1) * Vector2.Dot(bird.transform.position - transform.position, transform.right);
                    go_weight++;
                    go_direction = (go_direction*(go_weight-1) + want*90) / go_weight;
                }
            }
            
        }

        return go_direction * Turn_Speed;
    }

    float avoidWalls(float Turn_Speed)
    {
        Vector3 currentFace = new Vector3(transform.rotation[1],transform.rotation[2],transform.rotation[3]);
        Vector3 go_direction = Vector3.zero;
        if (transform.position.x > 4) //if near right side
        {
            if (transform.position.x > 5){
                go_direction += Vector3.left;
            }
            else{
                go_direction += Vector3.left * 0.5f / (5 -transform.position.x);

            }
            
        }
        else if (transform.position.x < -4) //if near left side
        {
            if (transform.position.x < -5){
                go_direction += Vector3.right;
            }
            else{
                go_direction += Vector3.right * 0.5f / (5 + transform.position.x);
            }
        }

        if (transform.position.y > 4) //if near up side
        {
            if (transform.position.y > 5){
                go_direction += Vector3.down;
            }
            else{
                go_direction += Vector3.down * 0.5f / (5 -transform.position.y);
            }
            
        }
        else if (transform.position.y < -4) //if near down side
        {
            if (transform.position.y < -5){
                go_direction += Vector3.up;
            }
            else{
                go_direction += Vector3.up * 0.5f / (5 + transform.position.y);
            }
            
        }

        Vector3 actual_turn = Vector3.Slerp(transform.up, go_direction, Turn_Speed);
        //Debug.DrawRay(transform.position,  go_direction); 
        
        //Debug.DrawRay(transform.position,  actual_turn , Color.red);


        return Vector3.SignedAngle(transform.up, actual_turn, Vector3.forward);
    }

    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if( Mathf.Abs(planarFactor) < 0.0001f 
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) 
                    / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }
}
