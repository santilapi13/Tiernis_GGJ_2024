using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Image autofiledBar;
    public TextMeshProUGUI livesText;

    public float allEnemyes = 0;
    public float progrecion = 0;

    [SerializeField] public Player Player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        autofiledBar.fillAmount = 0;
        
    }

    public float GetProgrecion()
    {
        return progrecion;
    }

    public void setProgrecion()
    {
        if (progrecion >= allEnemyes)
            return;

        progrecion++;
        autofiledBar.fillAmount = progrecion / allEnemyes;

        if (progrecion == 5f || progrecion == 15f || progrecion == 29f)
            Player.Evolve();
    }
    
    public void SetLives(int lives)
    {
        
        livesText.text = "x " + lives.ToString();
    }

}
