using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{

    public UIDocument UIDoc;

    private Button buttonStart;
    private Button buttonExit;
    
    void Start()
    {
        VisualElement root = UIDoc.rootVisualElement;
        
        buttonStart = root.Q<Button>("Start");
        buttonExit = root.Q<Button>("Exit");
        
        buttonStart.clicked += () => LoadScene("MainScene");
        buttonExit.clicked += () => QuitGame();
    }

    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    void QuitGame()
    {
        Application.Quit();
    }
    
    void Update()
    {
        
    }
}
