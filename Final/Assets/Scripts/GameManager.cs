using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool dev_mode = true;

    // variables for development and debug
    public GameObject main_camera;
    public GameObject player_camera;
    public float main_camera_speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        main_camera.SetActive(true);
        player_camera.SetActive(false);

        // Fix this
        MapGenerator map_gen = FindObjectOfType<MapGenerator>();
        map_gen.draw_map_in_editor();
    }

    void process_input() {
        // Switch camera
        if (dev_mode && Input.GetKeyDown(KeyCode.Tab)) {
            main_camera.SetActive(!main_camera.activeSelf);
            player_camera.SetActive(!player_camera.activeSelf);
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
