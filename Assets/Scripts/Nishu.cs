using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nishu : Stage
{
      
    const float noteSpeed = 0.2f;
    const float fadeAniGap = 0.05f;

    List<char> noteList;

    float noteTimer;

    Animation ani;

    [SerializeField]
    GameObject eye;

    [SerializeField]
    GameObject mouth;

    //Animation eyeAni;
    //Animation mouthAni;
    [SerializeField]
    GameObject[] eyes;
    [SerializeField]
    GameObject[] mouthes;

    eMotionState currentState;
    eMotionState effectState;

    char attackNoteCheck;
    char currentNote;

    bool isAttack = false;
    bool isCorrect = false;

    bool firstStart = false;

    UIMgr uiMgr;
    
    int lifeCount;
    int lifeMaxCount;
    
    Controller controller;

    int correctCount = 0;
    int incorrectCount = 0;

    enum NishuState
    {
        Preview,
        Action,
        Change,
    }

    NishuState nishuState = NishuState.Preview;

    int nishuPreviewCnt = 0;
    int nishuActionCnt = 0;

    public override void Init()
    {
        base.Init();

        Nishu_Data stageData = Nishu_DataMgr.GetStageData(Nishu_DataMgr.currentRound);
        uiMgr = FindObjectOfType<UIMgr>();
        controller = FindObjectOfType<Controller>();

        lifeMaxCount = lifeCount = stageData.chance;

        uiMgr.Reset(lifeMaxCount);

        noteTimer = 0.0f;

        ani = GetComponent<Animation>();

        noteList = new List<char>();

        char[] dataArray = stageData.note.ToCharArray();

        for (int i = 0; i < dataArray.Length; i++)
        {
            noteList.Add(dataArray[i]);
        }

        FaceAnimation(eMotionState.Idle);

        firstStart = false;

        correctCount = 0;
        incorrectCount = 0;

        nishuPreviewCnt = 0;
        nishuActionCnt = 0;
    }

    void FaceAnimation(eMotionState state)
    {
        if (state == eMotionState.Idle)
        {
            eye.GetComponent<Renderer>().sharedMaterial.color = Color.white;
            mouth.GetComponent<Renderer>().sharedMaterial.color = Color.white;

            eye.SetActive(true);
            mouth.SetActive(true);

            eyes[(int)currentState].SetActive(false);
            mouthes[(int)currentState].SetActive(false);

            return;
        }

        if (eye.activeSelf)
        {
            currentState = eMotionState.Up;
            eye.SetActive(false);
            mouth.SetActive(false);
        }

        eyes[(int)currentState].SetActive(false);
        eyes[(int)state].SetActive(true);

        mouthes[(int)currentState].SetActive(false);
        mouthes[(int)state].SetActive(true);

        eyes[(int)state].GetComponent<Renderer>().sharedMaterial.color = state == eMotionState.Attack ? Color.red : Color.white;
        mouthes[(int)state].GetComponent<Renderer>().sharedMaterial.color = state == eMotionState.Attack ? Color.red : Color.white;

        currentState = state;
    }

    void Action_Preview(char note)
    {
        currentNote = note;

        if (note == '.')
        {
            //isAttack = false;
            ani.CrossFade("Nishu_Idle", fadeAniGap);
            return;
        }
        
        //isAttack = true;
        attackNoteCheck = note;

        FaceAnimation(eMotionState.Attack);

        switch (attackNoteCheck)
        {
            case 'w':
                ani.CrossFade("Nishu_Up", fadeAniGap);
                break;

            case 'a':
                ani.CrossFade("Nishu_Left", fadeAniGap);
                break;

            case 's':
                ani.CrossFade("Nishu_Down", fadeAniGap);
                break;

            case 'd':
                ani.CrossFade("Nishu_Right", fadeAniGap);
                break;
        }

        Invoke("NishuPoint_Preview", 0.28f);
    }

    void NishuPoint_Preview()
    {
        FaceAnimation(eMotionState.Idle);
    }

    void Action(char note)
    {
        currentNote = note;

        if (note == '.')
        {
            isAttack = false;
            return;
        }
        
        Face(note);

        isAttack = true;
        attackNoteCheck = note;

        FaceAnimation(eMotionState.Attack);
        
    }

    void Face(char note)
    {

        Invoke("NishuPoint", 0.28f);
    }

    void NishuPoint()
    {
        ani.CrossFadeQueued("Nishu_Idle", fadeAniGap);

        FaceAnimation(eMotionState.Idle);
        if (isCorrect)
        {
            correctCount++;
            uiMgr.OnSuccess();
        }
        else
        {
            incorrectCount++;
            uiMgr.OnFailed();
        }
        isAttack = false;
        isCorrect = false;
        
    }
    
    public override void CheckInput(char c)
    {
        isCorrect = (isAttack && c == currentNote);
    }

    public override void StageComplete()
    {
        Nishu_DataMgr.currentRound++;
        base.StageComplete();
    }

    public override void StageFailed()
    {
        base.StageFailed();
    }

    public override void StageFailedFinish()
    {
        uiMgr.GameFinish(false);
    }

    public override void StageSuccessFinish()
    {
        uiMgr.GameFinish(true);
    }

    public override void StartGame()
    {
        Invoke("StageStart", 2.0f);
    }

    void StageStart()
    {

        NishuChange(NishuState.Preview);

    }

    void NishuChange(NishuState state)
    {
        nishuState = NishuState.Change;
        if (state == NishuState.Preview)
        {
            if (!firstStart)
            {
                CameraWork.StartWork("nishu_preview", Change_Preview, 2.0f);
            }
            else
            {

                CameraWork.StartWork(false,
                    new CameraWork.CallbackFunc[] { Change_Preview },
                    new bool[] { false, false, false, true },
                    new Transform[] { CameraWork.GetTr("nishu_change2"),
                        CameraWork.GetTr("nishu_change1"),
                        CameraWork.GetTr("GamePosition"),
                        CameraWork.GetTr("nishu_preview") },
                    new float[] { 2.0f, 2.0f, 2.0f, 2.0f });
            }
        }
        else
        {
            CameraWork.StartWork(false,
                    new CameraWork.CallbackFunc[] { Change_Action },
                    new bool[] { false, false, false, true },
                    new Transform[] { CameraWork.GetTr("GamePosition"),
                        CameraWork.GetTr("nishu_change1"),
                        CameraWork.GetTr("nishu_change2"),
                        CameraWork.GetTr("nishu_action") },
                    new float[] { 2.0f, 2.0f, 2.0f, 2.0f });
        }

    }

    void Change_Preview()
    {
        if (!firstStart)
        {
            base.StartGame();
            firstStart = true;

            ani.Play("Nishu_Start");
        }

        Invoke("PreviewStart", 2.0f);
        
    }

    void PreviewStart()
    {
        nishuState = NishuState.Preview;
    }

    void Change_Action()
    {
        Invoke("ActionStart", 2.0f);
    }

    void ActionStart()
    {
        nishuState = NishuState.Action;
    }

    // Use this for initialization
    void Start()
    {
        stageMgr.Init(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsStart)
            return;

        if (IsFinish)
            return;

        if (nishuState == NishuState.Preview)
        {
            if (noteList.Count > nishuPreviewCnt)
            {
                if (!isAttack)
                    noteTimer += Time.deltaTime;

                if (noteTimer >= noteSpeed)
                {
                    noteTimer = 0.0f;
                    Action_Preview(noteList[nishuPreviewCnt]);

                    nishuPreviewCnt++;
                }
            }
            else
            {
                NishuChange(NishuState.Action);
                nishuPreviewCnt = 0;
            }
        }
        else if (nishuState == NishuState.Action)
        {
            if (noteList.Count > nishuActionCnt)
            {
                if (!isAttack)
                    noteTimer += Time.deltaTime;

                if (noteTimer >= noteSpeed)
                {
                    noteTimer = 0.0f;
                    Action(noteList[nishuActionCnt]);

                    nishuActionCnt++;
                }
            }
            else
            {
                //
                CalculateScore();
                nishuActionCnt = 0;
            }

        }

    }

    void CalculateScore()
    {
        float val = correctCount / (float)(correctCount + incorrectCount);
        if (!IsFinish)
        {
            if (val >= 0.8f)
            {
                stageMgr.StageComplete();
            }
            else
            {
                uiMgr.OnFailed(ref lifeCount, lifeMaxCount);
                //lifeCount--;
                if (lifeCount > 0)
                {
                    NishuChange(NishuState.Preview);
                }
                else
                {
                    stageMgr.StageFailed();
                }
            }
        }
    }
}
