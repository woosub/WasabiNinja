using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    Animation ani;

    Stage pm;

    const float inputDelay = 0.5f;
    float inputTimer = 0.0f;

    bool inputFlag = false;

    string aniUp;
    string aniDown;
    string aniLeft;
    string aniRight;
    string aniIdle;

    private StageMgr mStageMgr;
    StageMgr stageMgr
    {
        get
        {
            if(mStageMgr == null)
            {
                mStageMgr = FindObjectOfType<StageMgr>();
            }

            return mStageMgr;
        }
    }

    public void AvoidFailed(eMotionState state)
    {
        switch(state)
        {
            case eMotionState.Up:
                ani.Play("Damage_Up");
                break;

            case eMotionState.Down:
                ani.Play("Damage_Down");
                break;

            case eMotionState.Left:
                ani.Play("Damage_Left");
                break;

            case eMotionState.Right:
                ani.Play("Damage_Right");
                break;
        }

        ani.PlayQueued("Idle");
    }
    
    // Use this for initialization
    void Start()
    {
        ani = GetComponent<Animation>();

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Nishu")
        {
            aniUp = "Nishu_Up";
            aniLeft = "Nishu_Left";
            aniRight = "Nishu_Right";
            aniDown = "Nishu_Down";
            aniIdle = "Nishu_Idle";

          
        }
        else
        {
            aniUp = "Up";
            aniLeft = "Left";
            aniRight = "Right";
            aniDown = "Down";
            aniIdle = "Idle";

            ani["Left"].speed = 2f;
            ani["Up"].speed = 2f;
            ani["Right"].speed = 2f;
            ani["Down"].speed = 2f;

            ani["Damage_Up"].speed = 1.7f;
            ani["Damage_Down"].speed = 1.7f;
            ani["Damage_Left"].speed = 1.7f;
            ani["Damage_Right"].speed = 1.7f;
        }

        

        pm = FindObjectOfType<Stage>();

        inputFlag = false;

#if UNITY_ANDROID
        SimpleGesture.On4AxisFlickSwipeUp(SwipeUp);
		SimpleGesture.On4AxisFlickSwipeDown(SwipeDown);
		SimpleGesture.On4AxisFlickSwipeLeft(SwipeLeft);
        SimpleGesture.On4AxisFlickSwipeRight(SwipeRight);
#endif
    }

#if UNITY_ANDROID
    
    public void SwipeUp()
    {
        if (!stageMgr.GetStage().IsStart)
            return;

        if (stageMgr.GetStage().IsFinish)
            return;

        if (inputFlag)
        {
            return;
        }

        ani.Stop();
        ani.Play(aniUp, PlayMode.StopAll);
        ani.PlayQueued(aniIdle);

        stageMgr.GetStage().CheckInput('w');

        inputFlag = true;
    }

    public void SwipeDown()
    {
        if (!stageMgr.GetStage().IsStart)
            return;

        if (stageMgr.GetStage().IsFinish)
            return;

        if (inputFlag)
        {
            return;
        }

        ani.Stop();
        ani.Play(aniDown, PlayMode.StopAll);
        ani.PlayQueued(aniIdle);

        stageMgr.GetStage().CheckInput('s');

        inputFlag = true;
    }

    public void SwipeLeft()
    {
        if (!stageMgr.GetStage().IsStart)
            return;

        if (stageMgr.GetStage().IsFinish)
            return;

        if (inputFlag)
        {
            return;
        }

        ani.Stop();
        ani.Play(aniLeft, PlayMode.StopAll);
        ani.PlayQueued(aniIdle);

        stageMgr.GetStage().CheckInput('a');

        inputFlag = true;
    }

    public void SwipeRight()
    {
        if (!stageMgr.GetStage().IsStart)
            return;

        if (stageMgr.GetStage().IsFinish)
            return;

        if (inputFlag)
        {
            return;
        }

        ani.Stop();
        ani.Play(aniRight, PlayMode.StopAll);
        ani.PlayQueued(aniIdle);

        stageMgr.GetStage().CheckInput('d');

        inputFlag = true;
    }

#endif

    // Update is called once per frame
    void Update()
    {
        if (!stageMgr.GetStage().IsStart)
            return;

        if (stageMgr.GetStage().IsFinish)
            return;

        if (inputFlag)
        {
            inputTimer += Time.deltaTime;

            if (inputTimer >= inputDelay)
            {
                inputTimer = 0.0f;
                inputFlag = false;
            }
        }


#if UNITY_EDITOR
        if (!inputFlag)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //ebimaruAni.Play("Attack_Left");
                ani.Stop();
                ani.Play(aniLeft, PlayMode.StopAll);
                ani.PlayQueued(aniIdle);

                stageMgr.GetStage().CheckInput('a');

                inputFlag = true;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                //ebimaruAni.Play("Attack_Right");
                ani.Stop();
                ani.Play(aniRight, PlayMode.StopAll);
                ani.PlayQueued(aniIdle);

                stageMgr.GetStage().CheckInput('d');

                inputFlag = true;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                //ebimaruAni.Play("Attack_Up");
                ani.Stop();
                ani.Play(aniUp, PlayMode.StopAll);
                ani.PlayQueued(aniIdle);

                stageMgr.GetStage().CheckInput('w');

                inputFlag = true;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                //ebimaruAni.Play("Attack_Down");
                ani.Stop();
                ani.Play(aniDown, PlayMode.StopAll);
                ani.PlayQueued(aniIdle);

                stageMgr.GetStage().CheckInput('s');

                inputFlag = true;
            }

        }
#endif
    }
}
