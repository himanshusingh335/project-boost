
using UnityEngine;


public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rcsThrust = 100f;
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

    private void Thrust()
    {
        float rotationThisFrame = mainThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.W))
        {
            rigidBody.AddRelativeForce(Vector3.up * rotationThisFrame);
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
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                {
                    print("ok");
                    break;
                }
            case "Untagged":
                {
                    print("dead");
                    break;
                }
        }
    }
}
