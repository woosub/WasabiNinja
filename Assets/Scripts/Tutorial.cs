using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public Text talk;

    bool IsStart = false;
    bool isAttack = false;
    bool isAvoid = false;
    
    int tutorialStep = 0;

    public Animation ani;
    public Animation wasabiAni;
    float noteTimer;

    eMotionState currentState;
    eMotionState effectState;

    Queue<char> noteQueue;

    const float noteSpeed = 0.2f;
    const float fadeAniGap = 0.05f;
    
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

    char attackNoteCheck;
    char currentNote;

    [SerializeField]
    GameObject wasabi;

    bool inputFlag;

    const float inputDelay = 0.5f;
    float inputTimer = 0.0f;

    [SerializeField]
    Animation pointerAni;

    // Use this for initialization
    void Start () {
        noteTimer = 0.0f;

        pointerAni.gameObject.SetActive(false);

        ani = GetComponent<Animation>();

        ani["Attack_Up"].speed = 3.0f;
        ani["Attack_Down"].speed = 3.0f;
        ani["Attack_Right"].speed = 3.0f;
        ani["Attack_Left"].speed = 3.0f;

        noteQueue = new Queue<char>();

        FaceAnimation(eMotionState.Idle);

        wasabiAni = wasabi.GetComponent<Animation>();


        wasabiAni["Left"].speed = 2f;
        wasabiAni["Up"].speed = 2f;
        wasabiAni["Right"].speed = 2f;
        wasabiAni["Down"].speed = 2f;
        
        wasabiAni["Damage_Up"].speed = 1.7f;
        wasabiAni["Damage_Down"].speed = 1.7f;
        wasabiAni["Damage_Left"].speed = 1.7f;
        wasabiAni["Damage_Right"].speed = 1.7f;

        inputFlag = false;

#if UNITY_ANDROID
        SimpleGesture.On4AxisFlickSwipeUp(SwipeUp);
        SimpleGesture.On4AxisFlickSwipeDown(SwipeDown);
        SimpleGesture.On4AxisFlickSwipeLeft(SwipeLeft);
        SimpleGesture.On4AxisFlickSwipeRight(SwipeRight);
#endif

        StartTutorial();
    }

#if UNITY_ANDROID

    public void SwipeUp()
    {
        if (inputFlag)
        {
            return;
        }

        wasabiAni.Stop();
        wasabiAni.Play("Up", PlayMode.StopAll);
        wasabiAni.PlayQueued("Idle");

        CheckAvoid('w');

        inputFlag = true;
    }

    public void SwipeDown()
    {
        if (inputFlag)
        {
            return;
        }

        wasabiAni.Stop();
        wasabiAni.Play("Down", PlayMode.StopAll);
        wasabiAni.PlayQueued("Idle");

        CheckAvoid('s');

        inputFlag = true;
    }

    public void SwipeLeft()
    {
        if (inputFlag)
        {
            return;
        }

        wasabiAni.Stop();
        wasabiAni.Play("Left", PlayMode.StopAll);
        wasabiAni.PlayQueued("Idle");

        CheckAvoid('a');

        inputFlag = true;
    }

    public void SwipeRight()
    {
        if (inputFlag)
        {
            return;
        }

        wasabiAni.Stop();
        wasabiAni.Play("Right", PlayMode.StopAll);
        wasabiAni.PlayQueued("Idle");

        CheckAvoid('d');

        inputFlag = true;
    }

#endif


    // Update is called once per frame
    void Update () {
        if (!IsStart)
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
                if (tutorialStep != 0 && tutorialStep < 4)
                {
                    return;
                }

                    //ebimaruAni.Play("Attack_Left");
                wasabiAni.Stop();
                wasabiAni.Play("Left", PlayMode.StopAll);
                wasabiAni.PlayQueued("Idle");

                CheckAvoid('a');

                inputFlag = true;
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                if (tutorialStep != 1 && tutorialStep < 4)
                {
                    return;
                }
                //ebimaruAni.Play("Attack_Right");
                wasabiAni.Stop();
                wasabiAni.Play("Right", PlayMode.StopAll);
                wasabiAni.PlayQueued("Idle");

                CheckAvoid('d');

                inputFlag = true;
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                if (tutorialStep != 2 && tutorialStep < 4)
                {
                    return;
                }
                //ebimaruAni.Play("Attack_Up");
                wasabiAni.Stop();
                wasabiAni.Play("Up", PlayMode.StopAll);
                wasabiAni.PlayQueued("Idle");

                CheckAvoid('w');

                inputFlag = true;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                if (tutorialStep != 3 && tutorialStep < 4)
                {
                    return;
                }
                //ebimaruAni.Play("Attack_Down");
                wasabiAni.Stop();
                wasabiAni.Play("Down", PlayMode.StopAll);
                wasabiAni.PlayQueued("Idle");

                CheckAvoid('s');

                inputFlag = true;
            }

        }
#endif
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

    void CheckAvoid(char note)
    {
        if (tutorialStep == 0)
        {
            if(note == 'a')
            {
                AvoidSuccess();
            }
        }
        else if(tutorialStep == 1)
        {
            if (note == 'd')
            {
                AvoidSuccess();
            }
        }
        else if(tutorialStep == 2)
        {
            if (note == 'w')
            {
                AvoidSuccess();
            }
        }
        else if(tutorialStep == 3)
        {
            if (note == 's')
            {
                AvoidSuccess();
            }
        }
        
        isAvoid = currentNote == note;
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
            if (tutorialStep >= 4)
            {
                AvoidFailed(currentNote);
            }
        }
        isAttack = false;
        isAvoid = false;
        IsStart = false;
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
        tutorialStep++;
    }
    
    void AvoidFailed(char note)
    {
        switch (note)
        {
            case 'w':
                wasabiAni.Play("Damage_Up");
                StartCoroutine(upAttack());
                break;

            case 's':
                wasabiAni.Play("Damage_Down");
                StartCoroutine(downAttack());
                break;

            case 'a':
                wasabiAni.Play("Damage_Left");
                StartCoroutine(leftAttack());
                break;

            case 'd':
                wasabiAni.Play("Damage_Right");
                StartCoroutine(rightAttack());
                break;
        }

        wasabi.GetComponent<Animation>().PlayQueued("Idle");
    }


    public void StartTutorial()
    {
        StartCoroutine(startTutorial());
    }

    IEnumerator leftAttack()
    {
        talk.text = "쯧쯧.. 다시!!";

        yield return new WaitForSeconds(2.0f);
        
        talk.text = "자 왼쪽 공격 간다";

        noteQueue.Enqueue('a');
        noteQueue.Enqueue('.');
        noteQueue.Enqueue('a');

        IsStart = true;
    }

    IEnumerator rightAttack()
    {
        talk.text = "쯧쯧.. 다시!!";

        yield return new WaitForSeconds(2.0f);

        talk.text = "자 오른쪽 공격 간다";

        noteQueue.Enqueue('d');
        noteQueue.Enqueue('.');
        noteQueue.Enqueue('d');

        IsStart = true;
    }

    IEnumerator upAttack()
    {
        talk.text = "쯧쯧.. 다시!!";

        yield return new WaitForSeconds(2.0f);

        talk.text = "자 윗쪽 공격 간다";

        noteQueue.Enqueue('w');
        noteQueue.Enqueue('.');
        noteQueue.Enqueue('w');

        IsStart = true;
    }

    IEnumerator downAttack()
    {
        talk.text = "쯧쯧.. 다시!!";

        yield return new WaitForSeconds(2.0f);

        talk.text = "자 아랫쪽 공격 간다";

        noteQueue.Enqueue('s');
        noteQueue.Enqueue('.');
        noteQueue.Enqueue('s');

        IsStart = true;
    }

    IEnumerator startTutorial()
    {
        talk.text = "ekuzo!!";

        yield return new WaitForSeconds(2.0f);

        talk.text = "왼쪽으로 움직여봐";

        pointerAni.gameObject.SetActive(true);
        pointerAni.Play("TutoriaLeft");

        IsStart = true;

        while (tutorialStep < 1)
            yield return null;

        pointerAni.gameObject.SetActive(false);

        IsStart = false;

        yield return new WaitForSeconds(0.5f);

        talk.text = "잘했군";

        yield return new WaitForSeconds(2.0f);

        talk.text = "오른쪽으로 움직여봐";

        pointerAni.gameObject.SetActive(true);
        pointerAni.Play("TutoriaRight");

        IsStart = true;

        while (tutorialStep < 2)
            yield return null;

        pointerAni.gameObject.SetActive(false);

        IsStart = false;

        yield return new WaitForSeconds(0.5f);

        talk.text = "잘했군";

        yield return new WaitForSeconds(2.0f);

        talk.text = "위로 움직여봐";

        pointerAni.gameObject.SetActive(true);
        pointerAni.Play("TutoriaUp");

        IsStart = true;

        while (tutorialStep < 3)
            yield return null;

        pointerAni.gameObject.SetActive(false);

        IsStart = false;

        yield return new WaitForSeconds(0.5f);

        talk.text = "잘했군";

        yield return new WaitForSeconds(2.0f);

        talk.text = "아래로 움직여봐";

        pointerAni.gameObject.SetActive(true);
        pointerAni.Play("TutoriaDown");

        IsStart = true;

        while (tutorialStep < 4)
            yield return null;

        pointerAni.gameObject.SetActive(false);

        IsStart = false;

        yield return new WaitForSeconds(0.5f);

        talk.text = "잘했군";

        yield return new WaitForSeconds(2.0f);

        talk.text = "이제 공격을 피하는 연습을 해볼까";

        yield return new WaitForSeconds(2.0f);

        talk.text = "눈이 빨개졌을 때 \n 몸을 움직여라";

        yield return new WaitForSeconds(2.0f);

        talk.text = "왼쪽으로 피해!";

        yield return new WaitForSeconds(1.0f);

        noteQueue.Enqueue('a');
        noteQueue.Enqueue('.');
        noteQueue.Enqueue('a');

        IsStart = true;

        while (tutorialStep < 5)
            yield return null;

        yield return new WaitForSeconds(0.5f);

        talk.text = "잘했군";

        yield return new WaitForSeconds(2.0f);


        talk.text = "오른쪽 피해!";

        yield return new WaitForSeconds(1.0f);

        noteQueue.Enqueue('d');
        noteQueue.Enqueue('.');
        noteQueue.Enqueue('d');

        IsStart = true;

        while (tutorialStep < 6)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        talk.text = "잘했군";

        yield return new WaitForSeconds(2.0f);



        talk.text = "윗쪽으로 피해!";

        yield return new WaitForSeconds(1.0f);

        noteQueue.Enqueue('w');
        noteQueue.Enqueue('.');
        noteQueue.Enqueue('w');

        IsStart = true;

        while (tutorialStep < 7)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        talk.text = "잘했군";

        yield return new WaitForSeconds(2.0f);



        talk.text = "아랫쪽으로 피해!";

        yield return new WaitForSeconds(1.0f);

        noteQueue.Enqueue('s');
        noteQueue.Enqueue('.');
        noteQueue.Enqueue('s');

        IsStart = true;

        while (tutorialStep < 8)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        talk.text = "잘했군";

        yield return new WaitForSeconds(2.0f);

        talk.text = "이제 본격적으로 수련에 들어가자";

        yield return new WaitForSeconds(2.0f);

        //goto stage 1
    }
}
