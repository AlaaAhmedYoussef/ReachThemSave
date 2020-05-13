using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    bool isTransitioning = false;

    //for inspector show
    [SerializeField] float RcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    [SerializeField] AudioClip mainEngine_sfx;
    [SerializeField] AudioClip death_sfx;
    [SerializeField] AudioClip nextLevel_sfx;

    [SerializeField] ParticleSystem mainEngine_fx;
    [SerializeField] ParticleSystem death_fx;
    [SerializeField] ParticleSystem nextLevel_fx;

    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] bool collisionEnable = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTransitioning)
        {
            RespondToThrustInput();
            RespondToRotateInput();

        }

        if (Debug.isDebugBuild)
        {
            RespondToDebugKeys(); //only for debug
        }

    }

    private void RespondToDebugKeys()
    {
        if (Input.GetKey(KeyCode.L))
        {
            StartSuccessSequence();
        }
        else if (Input.GetKey(KeyCode.C))
        {
            collisionEnable = !collisionEnable;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("collision");

        if (isTransitioning || !collisionEnable) return;

        switch (collision.gameObject.tag.ToLower())
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
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(nextLevel_sfx);
        nextLevel_fx.Play();
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    private void StartDeathSequence()
    {
        isTransitioning = true;
        audioSource.Stop();
        audioSource.PlayOneShot(death_sfx);
        death_fx.Play();
        Invoke("LoadTheLevel", levelLoadDelay);
        //Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void LoadTheLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentLevelIndex);
    }

    private void LoadNextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;

        if (nextLevelIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextLevelIndex = 0;
        }

        SceneManager.LoadScene(nextLevelIndex);

    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void RespondToThrustInput()
    {

        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
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

    private void RespondToRotateInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateManually(RcsThrust * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateManually(-RcsThrust * Time.deltaTime);
        }
    }

    private void RotateManually(float rotationThisFrame)
    {
        rigidBody.freezeRotation = true; //stop physics when take manual control
        transform.Rotate(Vector3.left * rotationThisFrame);
        rigidBody.freezeRotation = false; //start physics after manual control
    }
}
