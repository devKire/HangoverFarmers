using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
       
    }

    public void updateScene(string Main)
    {
        SceneManager.LoadScene(Main);

    }

    public void doExitGame()
    {
        Application.Quit();
    }

}
