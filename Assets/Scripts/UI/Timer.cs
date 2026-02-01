using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private TMP_Text timerText;
    private float elapsedTime = 0.0F;

    [Tooltip("If true, timer starts on scene load. If false, timer waits for startTimer() to be called.")]
    public bool isRunning = true;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        timerText.text = "00:00";
        
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

    }


    public void startTimer()
    {
        isRunning = true;
    }
    public void stopTimer()
    {
        isRunning = false;
    }
    public void resetTimer()
    {
        elapsedTime = 0.0F;
    }
    public float getTimeAsFloat()
    {
        return elapsedTime;
    }
    public string getTimeAsString()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
