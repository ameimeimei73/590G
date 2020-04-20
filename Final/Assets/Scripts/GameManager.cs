using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool dev_mode = true;

    // variables for development and debug
    public Camera main_camera;
    public Camera player_camera;
    public float main_camera_speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        main_camera.enabled = true;
        player_camera.enabled = false;
    }

    void process_input() {
        // Switch camera
        if (dev_mode && Input.GetKeyDown(KeyCode.Tab)) {
            main_camera.enabled = !main_camera.enabled;
            player_camera.enabled = !player_camera.enabled;
        }

        // Move current camera
        if (dev_mode && main_camera.enabled) {
            if (Input.GetKey(KeyCode.W)) {
                main_camera.transform.position += main_camera.transform.forward * main_camera_speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S)) {
                main_camera.transform.position -= main_camera.transform.forward * main_camera_speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.A)) {
                main_camera.transform.position -= main_camera.transform.right * main_camera_speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D)) {
                main_camera.transform.position += main_camera.transform.right * main_camera_speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.Q)) {
                main_camera.transform.position += main_camera.transform.up * main_camera_speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.E)) {
                main_camera.transform.position -= main_camera.transform.up * main_camera_speed * Time.deltaTime;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // process inputs
        process_input();
    }

    void FixedUpdate() {
    }
}
