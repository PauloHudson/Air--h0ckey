using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Rigidbody2D rb2d;
    public AudioSource source;
    
    public float initialSpeed = 5f;
    public float maxSpeed = 10f;
    public float minSpeed = 4.5f;
    public float paddleBouncePower = 9f;
    public float maxBounceAngle = 65f;
    public float paddleVelocityInfluence = 0.25f;
    
    private float lastCollisionTime = -1f;
    private float collisionCooldown = 0.15f;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
        Invoke("LaunchBall", 2f);
    }

    void FixedUpdate()
    {
        float speed = rb2d.linearVelocity.magnitude;

        if (speed > maxSpeed)
        {
            rb2d.linearVelocity = rb2d.linearVelocity.normalized * maxSpeed;
        }
        else if (speed > 0.01f && speed < minSpeed)
        {
            rb2d.linearVelocity = rb2d.linearVelocity.normalized * minSpeed;
        }
        
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -8f, 8f);
        pos.y = Mathf.Clamp(pos.y, -3f, 3f);
        transform.position = pos;
    }

    void LaunchBall()
    {
        float dirX = Random.value > 0.5f ? 1f : -1f;
        float randomY = Random.Range(-0.5f, 0.5f);
        Vector2 launchDir = new Vector2(dirX, randomY).normalized;
        rb2d.linearVelocity = launchDir * initialSpeed;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (Time.time - lastCollisionTime < collisionCooldown)
            return;

        lastCollisionTime = Time.time;
        PlaySound();

        if (coll.gameObject.CompareTag("Player"))
        {
            ApplyPaddleBounce(coll);
        }
        else
        {
            ApplySurfaceReflection(coll);
        }
    }

    void ApplyPaddleBounce(Collision2D coll)
    {
        ContactPoint2D contact = coll.GetContact(0);
        Rigidbody2D playerRb = coll.rigidbody;
        Vector2 playerVel = playerRb != null ? playerRb.linearVelocity : Vector2.zero;

        Collider2D paddleCollider = coll.collider;
        Vector2 paddleCenter = paddleCollider.bounds.center;
        float halfHeight = Mathf.Max(paddleCollider.bounds.extents.y, 0.01f);

        float offset = (contact.point.y - paddleCenter.y) / halfHeight;
        offset = Mathf.Clamp(offset, -1f, 1f);

        float angle = offset * maxBounceAngle * Mathf.Deg2Rad;
        float xDirection = transform.position.x < 0f ? 1f : -1f;

        Vector2 bounceDir = new Vector2(
            Mathf.Cos(angle) * xDirection,
            Mathf.Sin(angle)
        ).normalized;

        float targetSpeed = Mathf.Clamp(rb2d.linearVelocity.magnitude + 0.35f, minSpeed, maxSpeed);
        Vector2 velocityFromPaddle = playerVel * paddleVelocityInfluence;
        Vector2 finalVelocity = (bounceDir * Mathf.Max(paddleBouncePower, targetSpeed)) + velocityFromPaddle;

        if (finalVelocity.magnitude > maxSpeed)
            finalVelocity = finalVelocity.normalized * maxSpeed;

        rb2d.linearVelocity = finalVelocity;
    }

    void ApplySurfaceReflection(Collision2D coll)
    {
        if (coll.contactCount == 0)
            return;

        ContactPoint2D contact = coll.GetContact(0);
        Vector2 reflected = Vector2.Reflect(rb2d.linearVelocity, contact.normal);

        if (reflected.sqrMagnitude < 0.01f)
            reflected = -rb2d.linearVelocity;

        float targetSpeed = Mathf.Clamp(rb2d.linearVelocity.magnitude, minSpeed, maxSpeed);
        rb2d.linearVelocity = reflected.normalized * targetSpeed;
    }

    void PlaySound()
    {
        if (source != null)
            source.Play();
    }

    public void ResetBall()
    {
        rb2d.linearVelocity = Vector2.zero;
        transform.position = Vector2.zero;
    }

    public void RestartGame()
    {
        ResetBall();
        Invoke("LaunchBall", 1f);
    }
}