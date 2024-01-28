using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerChange : MonoBehaviour
{
    [SerializeField] private float timeToChange;
    private float timer = 0f;
    [SerializeField] private string sceneToChange;

// Update is called once per frame
    void Update()
    {
        if (timer >= timeToChange || Input.GetKeyDown(KeyCode.Return))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToChange);
            return;
        }
        
        timer += Time.deltaTime;
        
        
    }
    
    
}
