/*******************************************************************************************
* Author: German L.G Fica
* Websites: http://germanfica.xyz
* Description: Basic character controller to walk around a planet.
*******************************************************************************************/
using UnityEngine;
using System.Collections;



public class AstronautController : MonoBehaviour
{
    public Rigidbody rig;
    RaycastHit hit;
    public bool freezeRotation = true;

    public int forceConst = 4;
    private bool canJump;

    private bool onground;

    public Animator anim; // Animator 引用

    public string groundTag = "Planet"; // 检测的地面物体标签

    void Start()
    {
        Ini();
    }

    void Update()
    {
        // Raycast (doesn't affect gameplay)
        if (Physics.Raycast(transform.position, -transform.up, out hit))
        {
            Debug.DrawLine(transform.position, hit.point, Color.cyan);
        }
        // Jump Action
        if (Input.GetKeyUp(KeyCode.Space))
        {
            canJump = true;
        }
    }

    void FixedUpdate()
    {
        Move();
        Jump();
    }

    /* Some initializations
        */
    private void Ini()
    {
        rig.useGravity = false; // Disables Gravity
        if (freezeRotation)
        {
            rig.constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            rig.constraints = RigidbodyConstraints.None;
        }
    }

    /* Character jump
        */
    private void Jump()
    {
        if (canJump&&onground)
        {
            canJump = false;
            // AddForce (useless)
            //rig.AddForce(0, forceConst, 0, ForceMode.Impulse);
            // AddForceAtPosition (useless too)
            //rig.AddForceAtPosition(new Vector3(0, 0, forceConst), rig.transform.position, ForceMode.Impulse);
            // AddRelativeForce (successful)
            rig.AddRelativeForce(0, forceConst, 0, ForceMode.Impulse);
        }
    }

    /* Character movement
        */
    private void Move()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        if (Input.GetKey ("w")||Input.GetKey ("s")) {
				anim.SetInteger ("AnimationPar", 1);
			}  else {
				anim.SetInteger ("AnimationPar", 0);
			}

        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
        //transform.Translate(0, 0, 0.5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 检测与哪个物体碰撞
        if (collision.gameObject.name == "Mountain_Sea")
        {
            Debug.Log("与目标物体发生碰撞！");
            onground = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        // 检查离开的物体是否是 "Planet"
        if (collision.gameObject.name == "Mountain_Sea")
        {
            onground = false;
            Debug.Log("离开 Planet，人物不在地面上");
        }
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), "On Ground: " + onground);
    }
}
