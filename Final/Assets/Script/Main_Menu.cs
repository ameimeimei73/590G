using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Menu : MonoBehaviour
{
	public void scene_loader(int SenceIndex) {
		SceneManager.LoadScene(SenceIndex);
	}

    public void quit_game()
    {
        Debug.Log ("QUIT!");
        Application.Quit();
    }
}
