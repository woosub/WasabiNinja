using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternMgr : Stage {

    const float noteSpeed = 0.2f;
    const float fadeAniGap = 0.05f;

    Queue<char> noteQueue;

    float noteTimer;

    Animation ani;

    [SerializeField]
    GameObject eye;

    [SerializeField]
    GameObject mouth;

    [SerializeField]
    MeleeWeaponTrail trail;

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
    bool isAvoid = false;

    UIMgr uiMgr;

    int attCount;
    int attMaxCount;
    int lifeCount;
    int lifeMaxCount;

    [SerializeField]
    GameObject[] EffectSuccess;
    [SerializeField]
    GameObject[] EffectFailed;

    Controller controller;

    // Use this for initialization
    void Start() {
        stageMgr.Init(this);
    }

    public override void Init()
    {
        base.Init();

        Stage1_Data stageData = Stage1_DataMgr.GetStageData(Stage1_DataMgr.currentRound);
        uiMgr = FindObjectOfType<UIMgr>();
        controller = FindObjectOfType<Controller>();

        attMaxCount = attCount = stageData.attackCount;
        lifeMaxCount = lifeCount = stageData.chance;

        uiMgr.Reset(lifeMaxCount, attMaxCount);


        noteTimer = 0.0f;

        ani = GetComponent<Animation>();

        ani["Attack_Up"].speed = 3.0f;
        ani["Attack_Down"].speed = 3.0f;
        ani["Attack_Right"].speed = 3.0f;
        ani["Attack_Left"].speed = 3.0f;

        noteQueue = new Queue<char>();
        
        char[] dataArray = stageData.note.ToCharArray();

        for (int i = 0; i < dataArray.Length; i++)
        {
            noteQueue.Enqueue(dataArray[i]);
        }

        FaceAnimation(eMotionState.Idle);
    }

    public override void StartGame()
    {
        base.StartGame();
    }

    public override void StageComplete()
    {
        Stage1_DataMgr.currentRound++;
        base.StageComplete();
    }

    public override void StageFailed()
    {
        base.StageFailed();
    }

    public override void CheckInput(char c)
    {
        isAvoid = (isAttack && c == currentNote);
    }

    // Update is called once per frame
    void Update() {
        if (!IsStart)
            return;

        if (IsFinish)
            return;

        if (noteQueue.Count > 0)
        {
            if (!isAttack)
            noteTimer += Time.deltaTime;

            if (noteTimer >= noteSpeed)
            {
                noteTimer = 0.0f;
                
                Action(noteQueue.Dequeue());
            }
        }

    }

    void Action(char note)
    {
        currentNote = note;

        if (note == '.')
        {
            isAttack = false;
            return;
        }

        if (attackNoteCheck == note)
        {
            isAttack = true;
            attackNoteCheck = '.';
            Face(note);
            return;
        }

        trail.Emit = false;
        isAttack = false;
        attackNoteCheck = note;

        switch (attackNoteCheck)
        {
            case 'w':
                ani.CrossFade("StandBy_Up", fadeAniGap);
                FaceAnimation(eMotionState.Up);
                break;

            case 'a':
                ani.CrossFade("StandBy_Left", fadeAniGap);
                FaceAnimation(eMotionState.Left);
                break;

            case 's':
                ani.CrossFade("StandBy_Down", fadeAniGap);
                FaceAnimation(eMotionState.Down);
                break;

            case 'd':
                ani.CrossFade("StandBy_Right", fadeAniGap);
                FaceAnimation(eMotionState.Right);
                break;
        }
    }

    void Face(char note)
    {
        FaceAnimation(eMotionState.Attack);
        Invoke("Attack", 0.18f);
    }

    void Attack()
    {
        trail.Emit = true;
        uiMgr.AttackCounting(ref attCount);

        switch (currentNote)
        {
            case 'w':
                ani.CrossFade("Attack_Up", fadeAniGap);
                effectState = eMotionState.Up;
                break;

            case 'a':
                ani.CrossFade("Attack_Left", fadeAniGap);
                effectState = eMotionState.Left;
                break;

            case 's':
                ani.CrossFade("Attack_Down", fadeAniGap);
                effectState = eMotionState.Down;
                break;

            case 'd':
                ani.CrossFade("Attack_Right", fadeAniGap);
                effectState = eMotionState.Right;
                break;
        }

        ani.CrossFadeQueued("Idle", fadeAniGap);

        //isAttack = false;
        Invoke("AttackFinish", 0.1f);
    }

    void AttackFinish()
    {
        FaceAnimation(eMotionState.Idle);
        if (isAvoid)
        {
            AvoidSuccess();
        }
        else
        {
            AvoidFailed();
        }
        isAttack = false;
        isAvoid = false;
                
        if (!IsFinish)
        {
            if (attCount <= 0)
            {
                stageMgr.StageComplete();
            }
        }
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

    void AvoidSuccess()
    {        
        uiMgr.OnSuccess();

        EffectSuccess[(int)effectState].SetActive(true);
        Invoke("OffAllSuccessEffect", 0.3f);
    }

    void AvoidFailed()
    {
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
        Handheld.Vibrate();
#endif

        uiMgr.OnFailed(ref lifeCount, lifeMaxCount);

        controller.AvoidFailed(effectState);

        EffectFailed[(int)effectState].SetActive(true);
        Invoke("OffAllFailedEffect", 0.3f);

        if (lifeCount == 0)
        {
            //GameOver
            stageMgr.StageFailed();
        }
    }

    void OffAllSuccessEffect()
    {
        for (int i = 0; i < EffectSuccess.Length; i++)
        {
            EffectSuccess[i].SetActive(false);
        }
    }

    void OffAllFailedEffect()
    {
        for (int i = 0; i < EffectFailed.Length; i++)
        {
            EffectFailed[i].SetActive(false);
        }
    }

    public override void StageSuccessFinish()
    {
        uiMgr.GameFinish(true);
    }

    public override void StageFailedFinish()
    {
        uiMgr.GameFinish(false);
    }
}
