using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    public float forwardSpeed;
    public float sidewaySpeed;
    public float minX;
    public float maxX;
    private Touch touch;
    private MeshRenderer mr;
    public float boostAmount;
    public ParticleSystem finishParticle;
    public float cubeSize;
    public int cubesInRow;
    public Material explodeMaterial;
    public GameManager game;
    public GameObject player;

    private void createExplosionPiece(int x, int y, int z)
    {
        //create piece
        GameObject piece;
        piece = GameObject.CreatePrimitive(PrimitiveType.Cube);

        //set piece position and scale
        piece.transform.position = transform.position + new Vector3(cubeSize * x, cubeSize * y, cubeSize * z);
        piece.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);

        //add rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass = cubeSize;
        piece.GetComponent<Renderer>().material = explodeMaterial;
    }

    private void Finish()
    {
        float succession = game.score + 1;
        float spow5 = succession * succession * succession;
        finishParticle.Emit((int)(spow5 * 10));
        finishParticle.Play();
        rb.velocity = new Vector3(0, 0, 0);
        game.status = 3;
    }

    private void Boost()
    {
        rb.transform.localScale = new Vector3(mr.bounds.size.x, mr.bounds.size.y + boostAmount, mr.bounds.size.z);
        var newPos = new Vector3(0, boostAmount / 2, 0) + rb.position;
        rb.MovePosition(newPos);
        game.ScoreUp();
    }

    private void Explode()
    {
        gameObject.SetActive(false);
        for (int i = 0; i < cubesInRow * cubesInRow * cubesInRow; i++)
        {
            createExplosionPiece((i % (cubesInRow * cubesInRow)) - (cubesInRow),
                (i % cubesInRow) - (cubesInRow),
                (i % (cubesInRow * cubesInRow) % cubesInRow) - (cubesInRow));
        }

        player.SetActive(false);
        game.status = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle")
        {
            Explode();
        }

        if (other.gameObject.tag == "Finish")
        {
            Finish();
        }

        if (other.gameObject.tag == "Booster")
        {
            Boost();
            Destroy(other.gameObject);
        }
    }

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        rb = GetComponent<Rigidbody>();
        rb.velocity = new Vector3(0, 0, forwardSpeed);
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0 && game.status == 1)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                var newPos = new Vector3(touch.deltaPosition.x * sidewaySpeed, 0, 0) + rb.position;
                rb.MovePosition(new Vector3(Mathf.Clamp(newPos.x, minX, maxX), newPos.y, newPos.z));
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            var newPos = new Vector3(1 * sidewaySpeed, 0, 0) + rb.position;
            rb.MovePosition(new Vector3(Mathf.Clamp(newPos.x, minX, maxX), newPos.y, newPos.z));
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            var newPos = new Vector3(-1 * sidewaySpeed, 0, 0) + rb.position;
            rb.MovePosition(new Vector3(Mathf.Clamp(newPos.x, minX, maxX), newPos.y, newPos.z));
        }
    }
}