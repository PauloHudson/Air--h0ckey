using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public AudioSource source;
    private float speed = 15.0f;
    private float boundXLeft = -6.81f;
    private float boundXRight = -0.3f;
    private float boundY = 2.25f;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var pos = transform.position;
        
        Vector2 targetPos = new Vector2(mousePos.x, mousePos.y);
        Vector2 currentPos = new Vector2(pos.x, pos.y);
        
        Vector2 newPos = Vector2.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);
        
        newPos.x = Mathf.Clamp(newPos.x, boundXLeft, boundXRight);
        newPos.y = Mathf.Clamp(newPos.y, -boundY, boundY);
        
        pos.x = newPos.x;
        pos.y = newPos.y;
        transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (source != null)
            source.Play();
    }

}