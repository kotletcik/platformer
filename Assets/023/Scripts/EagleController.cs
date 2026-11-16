//Wiktoria Burniak, Paweł Gardzijewski
using UnityEngine;
using System.Collections;
namespace Game023
{
    public class EagleController : MonoBehaviour
    {
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        [Range(0.01f, 15.0f)][SerializeField] private float moveSpeed = 1f;
        private float startPositionX;
        [SerializeField] private float moveRange = 2.0f;
        bool isMovingRight = false;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }
        void MoveRight()
        {
            transform.Translate(Time.deltaTime * moveSpeed, 0.0f, 0.0f, Space.World);
        }
        void MoveLeft()
        {
            transform.Translate(-(Time.deltaTime * moveSpeed), 0.0f, 0.0f, Space.World);
        }
        void Awake()
        {
            startPositionX = this.transform.position.x;
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        IEnumerator KillOnAnimationEnd()
        {
            yield return new WaitForSeconds(0.75f);
            gameObject.SetActive(false);
        }
        void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (col.gameObject.transform.position.y > transform.position.y)
                {
                    animator.SetBool("isDead", true);
                    StartCoroutine(KillOnAnimationEnd());
                }
            }
        }
        // Update is called once per frame
        void Update()
        {
            if (isMovingRight)
            {
                if (this.transform.position.x < startPositionX + moveRange)
                {
                    MoveRight();
                    spriteRenderer.flipX = true;
                }
                else
                {
                    isMovingRight = false;
                    MoveLeft();
                    spriteRenderer.flipX = false;
                }
            }
            else
            {
                if (this.transform.position.x > startPositionX - moveRange)
                {
                    MoveLeft();
                    spriteRenderer.flipX = false;
                }
                else
                {
                    isMovingRight = true;
                    MoveRight();
                    spriteRenderer.flipX = true;
                }
            }
        }
    }
}