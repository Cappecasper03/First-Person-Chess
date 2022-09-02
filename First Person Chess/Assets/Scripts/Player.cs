using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform playerCam;
    public Transform player;
    private Rigidbody rb;

    private float mouseSens = 200f;
    private float speed = 5f;

    private float xRot;

    public static bool isMakingAMove = false;
    public static string currentTeam = "Light";

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Move camera
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        float desiredX = rot.y + mouseX;

        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        playerCam.transform.localRotation = Quaternion.Euler(xRot, desiredX, 0);
        player.transform.localRotation = Quaternion.Euler(0, desiredX, 0);

        // Move player
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        rb.transform.Translate(Vector3.right * moveX * speed * Time.deltaTime + Vector3.forward * moveZ * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMakingAMove)
            {
                isMakingAMove = true;
            }
            else
            {
                isMakingAMove = false;
            }
        }

        PlayerBoundaries();
    }

    private void OnGUI()
    {
        GUI.contentColor = Color.black;
        GUI.Label(new Rect(10f, 10f, 200f, 25f), $"Making a move: {isMakingAMove}");
        GUI.Label(new Rect(10f, 30f, 200f, 25f), $"Side: {currentTeam}");
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.collider.name != "Board" && isMakingAMove && other.collider.tag == currentTeam) // Moving piece
        {
            other.collider.GetComponent<Rigidbody>().mass = 0;
        }
        else if (other.collider.name != "Board" && !isMakingAMove && other.collider.tag != currentTeam) // Not moving piece
        {
            other.collider.GetComponent<Rigidbody>().mass = 1000;
        }
    }

    private void OnCollisionExit(Collision other) // Not moving piece
    {
        other.collider.GetComponent<Rigidbody>().mass = 1000;
    }

    private void PlayerBoundaries() // Keeps player on the board
    {
        float x = player.position.x;
        float z = player.position.z;

        if (player.position.x > 12)
        {
            x = 12f;
        }
        else if (player.position.x < -12)
        {
            x = -12f;
        }

        if (player.position.z > 12)
        {
            z = 12f;
        }
        else if (player.position.z < -12)
        {
            z = -12f;
        }

        player.position = new Vector3(x, player.position.y, z);
    }
}