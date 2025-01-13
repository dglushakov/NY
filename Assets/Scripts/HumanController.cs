using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanController : MonoBehaviour
{
    //private Rigidbody playerRb;
    // Start is called before the first frame update
    private Animator playerAnim;
    void Start()
    {
        //playerRb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * 1.0f * Time.deltaTime);
        playerAnim.SetInteger("legs", 1);
    }
}
