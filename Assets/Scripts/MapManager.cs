using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject ApplePref;
    public GameObject BlockPref;
    public GameObject WinPanel;
    public GameObject LosePanel;
    public GameObject LoseAutoRestartTimer;
    public Image LoseAutoRestartTimerImage;   
    private float waitTime = 3.0f;
    public Text CountAppleT;
    public Text CountSpeedT;
    void Awake()
    {
        BlockGenerator();
        AppleGenerator();        
    }
    private void Update()
    {
        if (LoseAutoRestartTimer.activeSelf == true)//таймер для автоперезапуска уровня
        {
            if (LoseAutoRestartTimerImage.fillAmount < 1)
            {
                LoseAutoRestartTimerImage.fillAmount += 1f / waitTime * Time.deltaTime;
            }
            else ButtonRestart();
        }
    }
    private void BlockGenerator() 
    {
        int randomCountBlocks = Random.Range(3, 6);
        for (int i = 0; i < randomCountBlocks; i++)
        {
            Instantiate(BlockPref, new Vector2(Random.Range(0, 21),Random.Range(0, 21)), Quaternion.identity);
        }
    }
    public void AppleGenerator()
    {        
            Instantiate(ApplePref, new Vector2(Random.Range(0, 21), Random.Range(0, 21)), Quaternion.identity);        
    }
    public void WinActivated(int CountApple, float CountSpeed)//активирует панель когда выграл
    {
        WinPanel.SetActive(true);
        CountAppleT.text = CountApple.ToString();
        CountSpeedT.text = CountSpeed.ToString("F2") + "s/move";
    }
    public void LoseActivated(bool WithAutoRestart)//активирует панель когда проиграл
    {
        LosePanel.SetActive(true);
        if (WithAutoRestart == true) 
        {
            LoseAutoRestartTimer.SetActive(true);
        }        
    }
    public void ButtonRestart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
