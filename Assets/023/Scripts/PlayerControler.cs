//Wiktoria Burniak, Paweł Gardzijewski
using UnityEngine;

namespace Game023
{
    public class PlayerControler : MonoBehaviour
    {
        private Rigidbody2D rb2D;
        private Animator animator;
        private bool isRunning = false;
        private SpriteRenderer spriteRenderer;


        [SerializeField] private LayerMask groundLayer;


        const float rayLength = 1.5f;

        private Vector2 startPosition;

        [Header("Movement parameters")]
        [Range(0.01f, 15.0f)][SerializeField] private float moveSpeed = 0.2f;
        [SerializeField] private float jumpForce = 6.0f;

        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Exit"))
            {
                GameManager.instance.AddPoints(100 * GameManager.instance.GetLives());
                GameManager.instance.LevelCompleted();
                Debug.Log("Level completed!");
            }

            if (col.CompareTag("LevelFall"))
            {
                Debug.Log("Player has fallen....");
                GameManager.instance.AddLives(-1);
                Debug.LogFormat("Utrata życia! Pozostała liczba żyć: {0}", GameManager.instance.GetLives());
            }

            if (col.CompareTag("Bonus"))
            {
                GameManager.instance.AddPoints(10);
                col.gameObject.SetActive(false);
            }

            if (col.CompareTag("Enemy"))
            {
                if (transform.position.y > col.gameObject.transform.position.y)
                {
                    Debug.Log("Killed an enemy");
                }
                else
                {
                    transform.position = startPosition;
                    GameManager.instance.AddLives(-1);
                    
                    Debug.LogFormat("Utrata życia! Pozostała liczba żyć: {0}", GameManager.instance.GetLives());
                    Debug.Log("Game Over");
                }
            }
            if (col.CompareTag("Heart"))
            {
                GameManager.instance.AddLives(1);
                Debug.LogFormat("Zdobyto życie! Aktualny stan: {0}", GameManager.instance.GetLives());
                col.gameObject.SetActive(false);
            }

        }

        bool IsGrounded()
        {
            return Physics2D.Raycast(transform.position, Vector2.down, rayLength, groundLayer.value);
        }

        void Jump()
        {
            if (IsGrounded())
            {
                rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                Debug.Log("Jumping....");
            }
        }

        void Awake()
        {
            startPosition = transform.position;

            rb2D = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.instance == null ||
                GameManager.instance.currentGameState != GameState.GS_GAME)
            {
                return;
            }

            isRunning = false;
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                spriteRenderer.flipX = false;
                isRunning = true;
                transform.Translate(Time.deltaTime * moveSpeed, 0.0f, 0.0f, Space.World);
            }

            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                spriteRenderer.flipX = true;
                isRunning = true;
                transform.Translate(-(Time.deltaTime * moveSpeed), 0.0f, 0.0f, Space.World);
            }

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            Debug.DrawRay(transform.position, rayLength * Vector2.down, Color.white, 0.25f, false);
            animator.SetBool("isGrounded", IsGrounded());
            animator.SetBool("isRunning", isRunning);
        }
    }
}
