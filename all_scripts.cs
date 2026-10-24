// ===== CoinCollector3D/Assets/Scripts/PlayerController.cs =====

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpForce = 5f;

    private Rigidbody rb;
    private bool isGrounded;
    private Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPos = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        if (transform.position.y < -10f) Die();
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(h, 0f, v);
        rb.AddForce(move * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.instance.AddCoin();
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("Obstacle"))
            Die();
    }

    void Die()
    {
        GameManager.instance.LoseLife();
        transform.position = startPos;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}


// ===== CoinCollector3D/Assets/Scripts/Coin.cs =====

using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 90f;

    void Update()
    {
        transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }
}


// ===== CoinCollector3D/Assets/Scripts/MovingObstacle.cs =====

using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float distance = 4f;
    public float speed = 2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float x = Mathf.PingPong(Time.time * speed, distance);

        transform.position = startPos + new Vector3(x, 0f, 0f);
    }
}


// ===== CoinCollector3D/Assets/Scripts/GameManager.cs =====

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int coins = 0;
    public int lives = 3;
    private int totalCoins;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI livesText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        UpdateUI();
    }

    public void AddCoin()
    {
        coins++;
        UpdateUI();

        if (coins >= totalCoins)
        {
            coinText.text = "You Win!";
            Time.timeScale = 0f;
        }
    }

    public void LoseLife()
    {
        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void UpdateUI()
    {
        coinText.text = "Coins: " + coins + "/" + totalCoins;
        livesText.text = "Lives: " + lives;
    }
}


// ===== CoinCollector3D/Assets/Scripts/CameraFollow.cs =====

using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 5f, -8f);

    void LateUpdate()
    {
        transform.position = player.position + offset;
        transform.LookAt(player);
    }
}


// ===== CoinCollector2D/Assets/Scripts/Player2D.cs =====

using UnityEngine;

public class Player2D : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 7f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(h * speed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (h != 0) transform.localScale = new Vector3(Mathf.Sign(h), 1f, 1f);

        if (transform.position.y < -10f) Die();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.instance.AddCoin();
            Destroy(other.gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = true;

        if (collision.gameObject.CompareTag("Obstacle"))
            Die();
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }

    void Die()
    {
        GameManager.instance.LoseLife();
        transform.position = startPos;
        rb.velocity = Vector2.zero;
    }
}


// ===== CoinCollector2D/Assets/Scripts/Coin2D.cs =====

using UnityEngine;

public class Coin2D : MonoBehaviour
{
    public float rotateSpeed = 180f;

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
    }
}


// ===== CoinCollector2D/Assets/Scripts/MovingObstacle.cs =====

using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    public float distance = 4f;
    public float speed = 2f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float x = Mathf.PingPong(Time.time * speed, distance);

        transform.position = startPos + new Vector3(x, 0f, 0f);
    }
}


// ===== CoinCollector2D/Assets/Scripts/GameManager.cs =====

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int coins = 0;
    public int lives = 3;
    private int totalCoins;

    public TextMeshProUGUI coinText;
    public TextMeshProUGUI livesText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        totalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
        UpdateUI();
    }

    public void AddCoin()
    {
        coins++;
        UpdateUI();

        if (coins >= totalCoins)
        {
            coinText.text = "You Win!";
            Time.timeScale = 0f;
        }
    }

    public void LoseLife()
    {
        lives--;
        UpdateUI();

        if (lives <= 0)
        {
            Debug.Log("Game Over");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void UpdateUI()
    {
        coinText.text = "Coins: " + coins + "/" + totalCoins;
        livesText.text = "Lives: " + lives;
    }
}


// ===== CoinCollector2D/Assets/Scripts/CameraFollow2D.cs =====

using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f, 0f, -10f);

    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}
