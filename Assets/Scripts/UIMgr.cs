using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMgr : MonoBehaviour {

    [SerializeField]
    GameObject Success;

    [SerializeField]
    GameObject Failed;

    [SerializeField]
    Text attackCount;

    [SerializeField]
    Text lifeCount;

    [SerializeField]
    GameObject Finish;

    [SerializeField]
    GameObject gameUI;

    [SerializeField]
    GameObject startUI;
    
    [SerializeField]
    Slider stamina;

    // Use this for initialization
    void Start () {
        OffSign();
        GameUISetActive(false);
        StartUISetActive(false, "");

        lifeCount.color = Color.white;

        Finish.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Reset(int life, int attCount)
    {
        lifeCount.text = life.ToString() + " / " + life;
        attackCount.text = attCount.ToString();
    }

    public void Reset(int life)
    {
        lifeCount.text = life.ToString() + " / " + life;
    }

    public void GameUISetActive(bool flag)
    {
        gameUI.SetActive(flag);
    }

    public void StartUISetActive(bool flag, string text)
    {
        startUI.GetComponent<Text>().text = text;
        startUI.GetComponent<Text>().enabled = flag;
    }

    public void OnSuccess()
    {
        Success.GetComponent<Text>().enabled = true;

        Invoke("OffSign", 0.3f);
    }

    public void OnFailed(ref int val, int max)
    {
        val--;

        if (val <= 1)
        {
            lifeCount.color = Color.red;
        }

        lifeCount.text = val.ToString() + " / " + max;

        Failed.GetComponent<Text>().enabled = true;

        Invoke("OffSign", 0.3f);
    }

    public void OnFailed()
    {
        Failed.GetComponent<Text>().enabled = true;

        Invoke("OffSign", 0.3f);
    }

    void OffSign()
    {
        Success.GetComponent<Text>().enabled = false;
        Failed.GetComponent<Text>().enabled = false;
    }

    public void AttackCounting(ref int count)
    {
        count--;

        attackCount.text = count.ToString();
    }

    public void CounterCounting(ref int count)
    {
        count++;

        attackCount.text = count.ToString();
    }

    public void GameFinish(bool complete)
    {
        Finish.SetActive(true);

        Finish.transform.Find("Mission").GetComponent<Text>().color = complete ? Color.green : Color.red;
        Finish.transform.Find("Mission").GetComponent<Text>().text = complete ? "Mission Complete" : "Mission Failed";
    }

    public void GoToMain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void Stamina(float val)
    {
        stamina.value = val;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
