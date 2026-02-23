using UnityEngine;

public class Player2Controls : MonoBehaviour
{
    private GameObject ball;
    public AudioSource source;
    
    public float speed = 16.0f;
    public float xMin = 0.3f;
    public float xMax = 6.81f;
    public float yMin = -2.5f;
    public float yMax = 2.5f;
    
    private float defensiveX = 5.5f;

    void Start()
    {
        ball = GameObject.FindGameObjectWithTag("Ball");
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (ball == null)
            return;

        Vector2 currentPos = transform.position;
        Vector2 ballPos = ball.transform.position;
        
        Vector2 targetPos;
        
        if (ballPos.x < 3f)
        {
            targetPos = new Vector2(defensiveX, Mathf.Clamp(ballPos.y, yMin, yMax));
        }
        else if (ballPos.x > currentPos.x)
        {
            float safeX = Mathf.Min(ballPos.x - 0.8f, xMax);
            safeX = Mathf.Max(safeX, defensiveX);
            targetPos = new Vector2(safeX, Mathf.Clamp(ballPos.y, yMin, yMax));
        }
        else
        {
            targetPos = new Vector2(defensiveX, Mathf.Clamp(ballPos.y, yMin, yMax));
        }
        
        Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);
        transform.position = newPos;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (source != null)
            source.Play();
    }
}
