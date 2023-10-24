using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    [SerializeField] int number_of_birds;
    [SerializeField] GameObject bird;  
    // Start is called before the first frame update
    void Start()
    {
        int number = 0;
        while (number < number_of_birds)
        {
            Instantiate(bird, new Vector3(0,0,0), new Quaternion(0, 0 ,(Random.value*2)-1, (Random.value*2)-1));
            number++;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
