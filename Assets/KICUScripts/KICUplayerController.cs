using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


public class KICUplayerController : MonoBehaviour
{
    private AudioSource source;
    public AudioClip jumpclip;

    public AudioClip PickUp;

    public Text CollectText;
    public Text endText;

    private int count;

    private Rigidbody2D rb2d;
    private bool facingRight = true;

    public float speed;
    public float jumpforce;


    private float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    private float timer;
    private int wholetime;

    //audio stuff




    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        count = 0;
        endText.text = "";
        SetCountText();
    }

    void Awake()
    {

        source = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float moveHorizontal = Input.GetAxis("Horizontal");

        Vector2 movement = new Vector2(moveHorizontal, 0);

        rb2d.AddForce(movement * speed);

        timer = timer + Time.deltaTime;
        if (timer >= 10)
        {
            endText.text = "You Lose! :(";
            StartCoroutine(ByeAfterDelay(2));

        }





        //stuff I added to flip my character
        if (facingRight == false && moveHorizontal > 0)
        {
            Flip();
        }
        else if (facingRight == true && moveHorizontal < 0)
        {
            Flip();
        }



    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("pickUp"))
        {
            other.gameObject.SetActive(false);

            source.PlayOneShot(PickUp);

            count = count + 2;
            GameLoader.AddScore(2);
            SetCountText();
        }
    }

    void SetCountText()
    {
        if (count >= 10)
        {
            endText.text = "You win!";
            StartCoroutine(ByeAfterDelay(2));
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground")
        {


            if (Input.GetKey(KeyCode.UpArrow))
            {
                rb2d.AddForce(new Vector2(0, jumpforce), ForceMode2D.Impulse);
                rb2d.velocity = Vector2.up * jumpforce;

                source.PlayOneShot(jumpclip);

                // Audio stuff
            }
        }

    }

    IEnumerator ByeAfterDelay(float time)
    {
        yield return new WaitForSeconds(time);

        GameLoader.gameOn = false;
    }
}
