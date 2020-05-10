using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }

    State state = State.Alive;

    //for inspector show
    [SerializeField] float RcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("collision");
        switch(collision.gameObject.tag.ToLower())
        {
            case "friendly":
                print("ok");
                break;
            case "fuel":
                print("fuel");
                break;
            case "finish":
                print("next level");
                LoadNextScene();
                break;
            default:
                print("dead");
                SceneManager.LoadScene(0);
                break;
        }
    }

    private static void LoadNextScene()
    {
        SceneManager.LoadScene(1);
    }

    private void Thrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true; //stop physics when take manual control

        float rotationThisFrame = RcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.left * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.right * rotationThisFrame);
        }

       rigidBody.freezeRotation = false; //start physics after manual control

    }


}
