using UnityEngine;

public class Ball : MonoBehaviour
{
    // config params
    private Paddle paddle1;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;
    [SerializeField] AudioClip[] ballSounds;
    [SerializeField] float randomFactor = 0.2f;

    // state
    Vector2 paddleToBallVector;
    bool hasStarted = false;

    // Cached component references
    AudioSource myAudioSource;
    Rigidbody2D myRigidBody2D;

    //new
    public static Ball instance;
    private bool firstBall = true;
    public Sprite[] ballType;
    public static bool powerBallActive;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
        paddle1 = FindObjectOfType<Paddle>();

        MakeInstance();
        SetBallPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag != "Extra Ball")
        {
            if (!hasStarted)
            {
                LockBallToPaddle();
                LaunchOnMouseClick();
            }
        }
    }

    private void LaunchOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hasStarted = true;
            myRigidBody2D.velocity = new Vector2(xPush, yPush);
        }
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2
            (Random.Range(0f, randomFactor),
            Random.Range(0f, randomFactor));

        if (hasStarted)
        {
            AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            myAudioSource.PlayOneShot(clip);
            myRigidBody2D.velocity += velocityTweak;
        }
    }

    //new
    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void SetBallPosition()
    {
        if (!powerBallActive)
        {
            GetComponent<SpriteRenderer>().sprite = ballType[0];
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = ballType[1];
        }

        if (firstBall)
        {
            paddleToBallVector = transform.position - paddle1.transform.position;
            firstBall = false;
        }
        hasStarted = false;
    }

    public void ChangeBallType(int ballNum)
    {
        powerBallActive = true;
        GetComponent<SpriteRenderer>().sprite = ballType[ballNum];
    }
}
