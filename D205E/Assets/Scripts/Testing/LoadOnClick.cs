using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadOnClick : MonoBehaviour {
    public GameObject LoadingImage;

    public void LoadScene(int Level)
    {
        LoadingImage.SetActive(true);
        SceneManager.LoadScene(Level);
    
        //Application.LoadLevel(Level);
    }
}
