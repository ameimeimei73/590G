using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public bool dev_mode = true;

    // variables for development and debug
    public GameObject main_camera;
    public GameObject player_camera; 
    public float main_camera_speed = 2.0f;

    public TextMeshProUGUI time_text;
    private float start_time;

    // Start is called before the first frame update
    void Start()
    {
        main_camera.SetActive(true);
        player_camera.SetActive(false);

        // Fix this
        MapGenerator map_gen = FindObjectOfType<MapGenerator>();
        map_gen.draw_map_in_editor();

        start_time = Time.time;
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

        float t = Time.time - start_time;
        string minutes = ((int) t / 60).ToString(); 
        string seconds = (t % 60).ToString("f0"); 

        time_text.text = minutes + ":" + seconds; 
    }

    void FixedUpdate() {
    }
}
