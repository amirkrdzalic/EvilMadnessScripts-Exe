using UnityEngine;
using System.Collections;

public class ShotNoiseScript : MonoBehaviour
{

    

    // Use this for initialization
    void Start()
    {

        gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            gameObject.GetComponent<AudioSource>().Play();
        }

    }
}