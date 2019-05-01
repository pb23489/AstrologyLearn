using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Timers;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool hideCursor = true;
    public Slider healthBarSlider;
    public int maxHealth;
    public Text healthText;
    public Slider pollenBarSlider;
    public int maxPollen;
    public Text pollenText;
    const float speed = 50;
    const float jumpSpeed = 20;
    const float gravityScale = 6;
    const float airControl = 100;
    const float groundControl = 10;
    const float maxSpeed = 75;
    int pollensCollected = 0;
    int health = 40;
    bool hasKey = false;
    bool activeKey = false;
    bool doorOpen = false;

    float h, flip, velocityX, lerp;
    Vector2 velocity;
    Vector3 flipScale = new Vector3();
    RaycastHit2D groundHit;
    Rigidbody2D rb;
    PhysicsMaterial2D mat;
    LayerMask mask = 1;
    SpriteRenderer spriteRenderer;

    Animator anim;

    void Awake()
    {
        Time.fixedDeltaTime = 1 / 100f;
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravityScale;
        mat = new PhysicsMaterial2D();
        mat.friction = 0;
        rb.sharedMaterial = mat;
        spriteRenderer = GetComponent<SpriteRenderer>();
        Cursor.visible = !hideCursor;
        health = maxHealth;

        InvokeRepeating("Clock", 1, 1);
    }

    void Update()
    {
        // input
        h = Input.GetAxisRaw("Horizontal");

        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = health;
        healthText.text ="Health: " + health.ToString();

        pollenBarSlider.maxValue = maxPollen;
        pollenBarSlider.value = pollensCollected;
        pollenText.text = "Pollen: " + pollensCollected.ToString() + "/" + maxPollen.ToString();

        groundHit = Physics2D.CircleCast(rb.position, 0.6f, Vector2.zero, 0, mask.value);

        lerp = airControl; // air control
        mat.friction = 0;
        if (groundHit) // grounded
        {
            lerp = groundControl;
            mat.friction = 1;
            if (Input.GetButtonDown("Jump")) { rb.velocity += Vector2.up * jumpSpeed; }
        }

        // flip sprite
        if (Mathf.Abs(h) > 0.01f)
        {
            spriteRenderer.flipX = h < 0f;
        }
    }

    void FixedUpdate()
    {
        if (Physics2D.Linecast(rb.position - Vector2.right * 0.7f, rb.position + Vector2.right * 0.7f, mask.value)) { h *= 0.2f; }

        velocity.Set((velocityX = Mathf.Lerp(velocityX, h, Time.deltaTime * lerp)) * speed, rb.velocity.y);
        rb.velocity = velocity;

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        if (groundHit)
        {
            Rigidbody2D r = groundHit.collider.GetComponentInParent<Rigidbody2D>();
            if (r != null) { rb.AddForceAtPosition(r.velocity * 0.5f, groundHit.point, ForceMode2D.Force); } // stick to stuffs
        }
    }

    void Clock()
    {
        if (transform.position.y < -50) { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
    }

    void OnTriggerEnter2D(Collider2D collision) {

        if (collision.tag == "Pollen")
        {
            Instantiate(Resources.Load("Effect"), collision.transform.position, collision.transform.rotation);
            Destroy(collision.gameObject);
            pollensCollected++;
            Debug.Log(pollensCollected);
        }

        else if (collision.tag == "Damage")
        {
            Instantiate(Resources.Load("Ouch"), collision.transform.position, collision.transform.rotation);
            health = health - 10;
            Debug.Log(health);
            if (health <= 0)
            {
                SceneManager.LoadScene(0);
            }
        }

        else if (collision.tag == "Key")
        {
            if (pollensCollected == 8)
            {
                Instantiate(Resources.Load("Found"), collision.transform.position, collision.transform.rotation);
                Destroy(collision.gameObject);
                hasKey = true;
                Debug.Log("Got Key!");
            }

        }

        else if (collision.tag == "Door")
        {
            if (hasKey)
            {
                Instantiate(Resources.Load("Whoosh"), collision.transform.position, collision.transform.rotation);
                collision.GetComponent<Animator>().SetTrigger("Open");
                doorOpen = true;
                hasKey = false;
            }
        }

        else if (collision.tag == "Trunk")
        {
            if (doorOpen)
            {
                Debug.Log("Trunk works");
                SceneManager.LoadScene(1);
            }
        }

        else if (collision.tag == "Restart")
        {
            SceneManager.LoadScene(0);
        }
    }
}