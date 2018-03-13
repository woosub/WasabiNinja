using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour {

    private StageMgr mStageMgr;
    protected StageMgr stageMgr
    {
        get
        {
            if (mStageMgr == null)
            {
                mStageMgr = FindObjectOfType<StageMgr>();
            }
            return mStageMgr;
        }
    }

    private StageIntro mStageIntro;
    protected StageIntro stageIntro
    {
        get
        {
            if (mStageIntro == null)
            {
                mStageIntro = FindObjectOfType<StageIntro>();
            }
            return mStageIntro;
        }
    }

    private bool isStartGame;
    private bool isFinishGame;

    public bool IsStart { get { return isStartGame; } }
    public bool IsFinish { get { return isFinishGame; } }

    public GameObject rewardItem;
    public GameObject player;

    public virtual void Init()
    {
        isStartGame = false;
        isFinishGame = false;

        StartCoroutine(stageIntro.StartEvent(stageMgr.StageStart));
    }
    public virtual void StartGame() { isStartGame = true; }
    public virtual void CheckInput(char c) { }
    public virtual void StageComplete()
    {
        isFinishGame = true;
        StartCoroutine(stageIntro.StageComplete(stageMgr.StageCompleteFinish, rewardItem, player, gameObject));
    }

    public virtual void StageFailed()
    {
        isFinishGame = true;
        StartCoroutine(stageIntro.StageFailed(stageMgr.StageFailedFinish, player, gameObject));
    }

    public virtual void StageSuccessFinish() { }
    public virtual void StageFailedFinish() { }

}
