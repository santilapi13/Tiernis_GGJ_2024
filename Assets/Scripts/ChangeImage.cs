using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private float timeToChange = 0.5f;
    private float currentTime = 0;
    private int currentSprite = 0;
    void Start()
    {
        currentTime = timeToChange;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            return;
        }
        
        currentTime = timeToChange;
        currentSprite++;
        if (currentSprite >= sprites.Length) currentSprite = 0;
        image.sprite = sprites[currentSprite];
    }
}
