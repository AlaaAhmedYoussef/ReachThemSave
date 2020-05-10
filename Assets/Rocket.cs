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

    [SerializeField] AudioClip mainEngine_sfx;
    [SerializeField] AudioClip death_sfx;
    [SerializeField] AudioClip nextLevel_sfx;

    [SerializeField] ParticleSystem mainEngine_fx;
    [SerializeField] ParticleSystem death_fx;
    [SerializeField] ParticleSystem nextLevel_fx;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            Thrust();
            Rotate();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("collision");

        if (state != State.Alive) return;

        switch(collision.gameObject.tag.ToLower())
        {
            case "friendly":
                print("ok");
                break;
            case "fuel":
                print("fuel");
                break;
            case "finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(nextLevel_sfx);
        nextLevel_fx.Play();
        Invoke("LoadNextLevel", 1f);
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(death_sfx);
        death_fx.Play();
        Invoke("LoadFirstLevel", 1f);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }
   
    private void Thrust()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            mainEngine_fx.Play();

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine_sfx);
            }
        }
        else
        {
            audioSource.Stop();
            mainEngine_fx.Stop();
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
