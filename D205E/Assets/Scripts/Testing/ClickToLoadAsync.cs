using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickToLoadAsync : MonoBehaviour {

    public Slider LoadingBar;
    public GameObject LoadingImage;

    private AsyncOperation Async;

    public void ClickAsync(int Level)
    {
        LoadingImage.SetActive(true);
        StartCoroutine(LoadLevelWithBar(Level));

    }

    IEnumerator LoadLevelWithBar(int Level)
    {
        Async = SceneManager.LoadSceneAsync(Level);
        while (!Async.isDone)
        {
            LoadingBar.value = Async.progress;
            Debug.Log(Async.progress);
            yield return null;
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
