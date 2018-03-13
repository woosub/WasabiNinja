using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{

    [SerializeField]
    Animation ciAni;

    bool isTouchStart = false;
    bool isStageSelect = false;
    bool isSubject = false;
    bool isStageMoving = false;

    bool inPlay = false;

    public Renderer subjectRenderer;

    int currentStage = 0;
    const int maxStage = 4;

    public GameObject GameStage;

    void Awake()
    {
#if UNITY_ANDROID
        SimpleGesture.On4AxisFlickSwipeUp(SwipeUp);
        SimpleGesture.On4AxisFlickSwipeDown(SwipeDown);
        SimpleGesture.On4AxisFlickSwipeLeft(SwipeLeft);
        SimpleGesture.On4AxisFlickSwipeRight(SwipeRight);
#endif
    }

    // Use this for initialization
    IEnumerator Start()
    {
        isTouchStart = false;
        isStageSelect = false;

        //ciAni.gameObject.SetActive(false);
        ciAni.Play();
        //yield return new WaitForSecondsw(0.01f);

        //ciAni.gameObject.SetActive(true);

        yield return new WaitForSeconds(4.0f);

        Queue<CameraWork.CallbackFunc> cb = new Queue<CameraWork.CallbackFunc>();
        cb.Enqueue(PlaySubject);
        cb.Enqueue(PlayStage);

        Queue<bool> playFlagQue = new Queue<bool>();
        playFlagQue.Enqueue(true);
        playFlagQue.Enqueue(true);

        CameraWork.StartWork(true, 
            new CameraWork.CallbackFunc[] { PlaySubject, PlayStage }, 
            new bool[] { true, true });

        while (!isSubject)
            yield return null;

        while (!isStageSelect)
            yield return null;

        CameraWork.Resume(true);
    }

    void SwipeUp()
    {
        if (inPlay)
        {
            return;
        }

        if (isTouchStart)
        {
            isStageSelect = true;
        }
    }

    void SwipeDown()
    {
        if (inPlay)
        {
            return;
        }

        if (isTouchStart)
        {
            isStageSelect = true;
        }
    }

    void SwipeLeft()
    {
        if (inPlay)
        {
            if (isStageMoving)
                return;

            isStageMoving = true;
            currentStage--;
            if (currentStage < 0)
            {
                currentStage = maxStage - 1;
            }

            StartCoroutine(RotateStage());
            CameraWork.StartWork("s" + currentStage, StageMovingFinish);

            return;
        }

        if (isTouchStart)
        {
            isStageSelect = true;
        }
    }

    void SwipeRight()
    {
        if (inPlay)
        {
            if (isStageMoving)
                return;

            isStageMoving = true;
            currentStage++;
            if (currentStage >= maxStage)
            {
                currentStage = 0;
            }

            StartCoroutine(RotateStage());
            CameraWork.StartWork("s" + currentStage, StageMovingFinish);

            return;
        }

        if (isTouchStart)
        {
            isStageSelect = true;
        }
    }

    public void PlaySubject()
    {
        isSubject = true;
        StartCoroutine(routineSubjectAni());
    }

    IEnumerator routineSubjectAni()
    {
        float alpha = 0.0f;

        yield return new WaitForSeconds(0.5f);

        while (true)
        {
            yield return null;

            alpha += Time.deltaTime * 2;

            subjectRenderer.material.color = new Color(1, 1, 1, alpha);

            if (alpha >= 1)
            {
                break;
            }
        }

        isTouchStart = true;
    }

    public void PlayStage()
    {
        StartCoroutine(GoSelectState());
    }

    IEnumerator GoSelectState()
    {
        yield return new WaitForSeconds(1.5f);

        inPlay = true;
        //UnityEngine.SceneManagement.SceneManager.LoadScene("");
    }

    void StageMovingFinish()
    {
        isStageMoving = false;
    }

    IEnumerator RotateStage()
    {
        Quaternion cur = GameStage.transform.rotation;
        Quaternion dest = GameObject.Find("st" + currentStage).transform.rotation;

        float fix = 0;

        while (true)
        {
            yield return null;

            fix += Time.deltaTime * 2;

            GameStage.transform.rotation = Quaternion.Lerp(cur, dest, fix);


            if (fix >= 1)
            {
                break;
            }
        }
    }

    IEnumerator GotoStage()
    {
        //fade

        yield return new WaitForSeconds(0.5f);

        //scene go
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR


        if (inPlay)
        {
            if (isStageMoving)
                return;

            if (Input.GetKeyDown(KeyCode.A))
            {
                isStageMoving = true;
                currentStage--;
                if (currentStage < 0)
                {
                    currentStage = maxStage - 1;
                }

                StartCoroutine(RotateStage());
                CameraWork.StartWork("s" + currentStage, StageMovingFinish);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                isStageMoving = true;
                currentStage++;
                if (currentStage >= maxStage)
                {
                    currentStage = 0;
                }

                StartCoroutine(RotateStage());
                CameraWork.StartWork("s" + currentStage, StageMovingFinish);
            }

            if (Input.GetKeyDown(KeyCode.W))
            {
                isStageMoving = true;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                isStageMoving = true;
            }
        }
        else
        {
            if (isTouchStart)
            {
                if (Input.anyKeyDown)
                {
                    subjectRenderer.gameObject.SetActive(false);
                    isStageSelect = true;
                }
            }
        }

#endif
    }
}
