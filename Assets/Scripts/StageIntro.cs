using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageIntro : MonoBehaviour {

    [SerializeField]
    Transform cam;

    [SerializeField]
    Transform posStart;

    [SerializeField]
    Transform posEnd;

    [SerializeField]
    Transform posGame;

    [SerializeField]
    Transform finishComplete1;

    [SerializeField]
    Transform finishComplete2;

    [SerializeField]
    Transform finishFailed1;

    [SerializeField]
    Transform finishFailed2;

    const float moveSpeed1 = 1.0f;
    const float moveSpeed2 = 6.0f;

    float val;

    UIMgr uiMgr;

    public delegate void CallbackFunc();

    void Awake()
    {
        uiMgr = FindObjectOfType<UIMgr>();
    }

    // Use this for initialization
    public IEnumerator StartEvent (CallbackFunc _func) {
        
        yield return new WaitForSeconds(0.5f);

        val = 0.0f;

        while (true)
        {
            yield return null;

            val += Time.deltaTime * moveSpeed1;

            cam.position = Vector3.Lerp(posStart.position, posEnd.position, val);
            cam.rotation = Quaternion.Lerp(posStart.rotation, posEnd.rotation, val);

            if (val >= 1.0f)
            {
                break;
            }
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Test")
        {
            uiMgr.StartUISetActive(true, "Stage " + (Stage1_DataMgr.currentRound + 1));
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Nishu")
        {
            uiMgr.StartUISetActive(true, "Stage " + (Nishu_DataMgr.currentRound + 1));
        }
        else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Counter")
        {
            uiMgr.StartUISetActive(true, "Stage " + (StageCounter_DataMgr.currentRound + 1));
        }

        yield return new WaitForSeconds(1.0f);

        uiMgr.StartUISetActive(false, "");

        yield return new WaitForSeconds(0.5f);

        //Ekuzo!
        uiMgr.StartUISetActive(true, "Ekuzo!");

        yield return new WaitForSeconds(0.5f);

        val = 0.0f;

        while (true)
        {
            yield return null;

            val += Time.deltaTime * moveSpeed2;

            cam.position = Vector3.Lerp(posEnd.position, posGame.position, val);
            cam.rotation = Quaternion.Lerp(posEnd.rotation, posGame.rotation, val);

            if (val >= 1.0f)
            {
                break;
            }
        }

        uiMgr.StartUISetActive(false, "");
        uiMgr.GameUISetActive(true);

        yield return new WaitForSeconds(0.5f);

        _func();
        //FindObjectOfType<StageMgr>().StageStart();
    }
	
    public IEnumerator StageComplete(CallbackFunc _func, GameObject rewardItem, GameObject player, GameObject npc)
    {
        uiMgr.GameUISetActive(false);
        npc.GetComponent<Animation>().Play("Success");

        yield return new WaitForSeconds(1.0f);

        player.GetComponent<Animation>().Play("Success");

        val = 0.0f;

        Vector3 curPos = cam.transform.position;
        Quaternion curRot = cam.transform.rotation;

        while (true)
        {
            yield return null;

            val += Time.deltaTime * moveSpeed2;

            cam.position = Vector3.Lerp(curPos, finishComplete1.position, val);
            cam.rotation = Quaternion.Lerp(curRot, finishComplete1.rotation, val);

            if (val >= 1.0f)
            {
                break;
            }
        }

        yield return new WaitForSeconds(3.1f);

        rewardItem.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        val = 0.0f;

        curPos = rewardItem.transform.position;
        curRot = rewardItem.transform.rotation;

        while (true)
        {
            yield return null;

            val += Time.deltaTime * moveSpeed1;

            rewardItem.transform.position = Vector3.Lerp(curPos, finishComplete2.position, val);
            //rewardItem.transform.rotation = Quaternion.Lerp(curRot, posGame.rotation, val);
            if (val >= 1.0f)
            {
                break;
            }
        }

        _func();

        while (true)
        {
            yield return null;
            rewardItem.transform.Rotate(Vector3.up, Time.deltaTime * -100);
        }

    }

    public IEnumerator StageFailed(CallbackFunc _func, GameObject player, GameObject npc)
    {
        uiMgr.GameUISetActive(false);

        yield return new WaitForSeconds(1.0f);

        player.GetComponent<Animation>().Play("Failed");
        npc.GetComponent<Animation>().Play("Failed");

        val = 0.0f;

        Vector3 curPos = cam.transform.position;
        Quaternion curRot = cam.transform.rotation;

        while (true)
        {
            yield return null;

            val += Time.deltaTime * moveSpeed2;

            cam.position = Vector3.Lerp(curPos, finishFailed1.position, val);
            cam.rotation = Quaternion.Lerp(curRot, finishFailed1.rotation, val);

            if (val >= 1.0f)
            {
                break;
            }
        }

        yield return new WaitForSeconds(0.3f);

        val = 0.0f;

        curPos = cam.transform.position;
        curRot = cam.transform.rotation;

        while (true)
        {
            yield return null;

            val += Time.deltaTime * moveSpeed2;

            cam.position = Vector3.Lerp(curPos, finishFailed2.position, val);
            cam.rotation = Quaternion.Lerp(curRot, finishFailed2.rotation, val);

            if (val >= 1.0f)
            {
                break;
            }
        }

        yield return new WaitForSeconds(0.6f);

        _func();
        
    }

    // Update is called once per frame
    void Update () {
		
	}
}
