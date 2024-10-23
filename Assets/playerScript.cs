using UnityEngine;
using UnityEngine.InputSystem;

public class playerScript : MonoBehaviour
{
    Vector2 mv;
    public float playerSpeed;
    public float yRot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (mv.y > 0)
        {
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isWalking", true);
        }
        else
        {
            GameObject.Find("Player").GetComponent<Animator>().SetBool("isWalking", false);
        }
    }

    void OnMove(InputValue move)
    {
        mv = move.Get<Vector2>();
    }

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up, mv.x * yRot);
        if (mv.y > 0)
        {
            transform.position += transform.forward * mv.y * playerSpeed;
        }
    }
}
