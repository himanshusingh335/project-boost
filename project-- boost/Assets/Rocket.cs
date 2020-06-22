using UnityEngine.SceneManagement;
using UnityEngine;


public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;
    [SerializeField] AudioClip engine;
    [SerializeField] AudioClip levelClear;
    [SerializeField] AudioClip deathExplosion;
    enum State { Alive, Dying, Transcending };

    State state=State.Alive; 
    // Start is called before the first frame update
    void Start()
    {
        
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == State.Alive)
        {
            Thrust();
            Rotate();
        }
        
    }

    private void Thrust()
    {
        float rotationThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
        {
            rigidBody.AddRelativeForce(Vector3.up * rotationThisFrame);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(engine);
            }
        }
        else
        {
            audioSource.Stop();
        }
    }
    private void Rotate()
    {
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        rigidBody.freezeRotation = true;

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state == State.Dying || state == State.Transcending)
        {
            return;
        }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    break;
                }
            case "Finish":
                {
                    state = State.Transcending;
                    audioSource.PlayOneShot(levelClear);
                    Invoke("LoadNextScene", 2f);
                    break;
                }
            case "Untagged":
                {
                    state = State.Dying;
                    audioSource.PlayOneShot(deathExplosion);
                    Invoke("LoadNextScene", 2f);
                    break;
                }
        }
    }

    private void LoadNextScene()
    {
        if(state == State.Transcending)
        {
            SceneManager.LoadScene(1);
        }
        else if (state == State.Dying)
        {
            SceneManager.LoadScene(0);
        }
    }
        
}
