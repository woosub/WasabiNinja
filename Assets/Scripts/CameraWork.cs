using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWork : MonoBehaviour {

    [SerializeField]
    Transform[] movePositions;

    [SerializeField]
    Transform[] lookAtPoints;

    [SerializeField]
    float[] moveSpeed;

    int moveCount = 0;
    int lookAtCount = 0;

    Vector3 currentPos;
    Quaternion currentRot;
    Vector3 currentLook;

    float lookAtTime = 0;
    float moveTime = 0;

    static bool isWork = false;

    bool isMoveFinish = false;
    bool isLookAtFinish = false;

    static GameObject point;
    static GameObject cam;

    static CameraWork m_this;

    static bool breakWorkFlag = false;

    public delegate void CallbackFunc();
    public CallbackFunc callback;

    static Queue<bool> callbackPlayQueue = new Queue<bool>();
    static Queue<CallbackFunc> callbackFuncQueue;

    public float stageSelectSpeed;
    bool stageSelectFlag;

    public static Transform GetTr(string name)
    {
        return GameObject.Find(name).transform;
    }

    static void AddCallBack(CallbackFunc[] callbacks)
    {
        callbackFuncQueue = new Queue<CallbackFunc>();
        for (int i = 0; i < callbacks.Length; i++)
        {
            callbackFuncQueue.Enqueue(callbacks[i]);
        }
    }

    static void AddPlayQueue(bool[] que)
    {
        callbackPlayQueue = new Queue<bool>();
        for (int i = 0; i < que.Length; i++)
        {
            callbackPlayQueue.Enqueue(que[i]);
        }
    }

    public static void StartWork(bool breakWork, CallbackFunc[] callbacks, bool[] que, Transform[] movePos, float[] moveSpeed)
    {
        m_this.movePositions = movePos;
        m_this.moveSpeed = moveSpeed;

        StartWork(breakWork, callbacks, que);
    }

    public static void StartWork(bool breakWork, CallbackFunc[] callbacks, bool[] que)
    {
        m_this.stageSelectFlag = false;

        AddPlayQueue(que);
        AddCallBack(callbacks);

        m_this.moveCount = 0;
        m_this.lookAtCount = 0;

        m_this.lookAtTime = 0;
        m_this.moveTime = 0;

        m_this.isMoveFinish = false;
        m_this.isLookAtFinish = false;

        cam = GameObject.Find("Main Camera");

        if (m_this.lookAtPoints.Length > 0)
        {
            point = new GameObject("LookAtPoint");
            point.transform.position = m_this.lookAtPoints[0].position;
            m_this.currentLook = point.transform.position;
        }
        else
        {
            m_this.isLookAtFinish = true;
        }

        isWork = true;

        m_this.currentPos = cam.transform.position;
        m_this.currentRot = cam.transform.rotation;

        breakWorkFlag = breakWork;
    }

    public static void StartWork(string name, CallbackFunc callback, float moveSpeed)
    {
        m_this.moveSpeed = new float[] { moveSpeed };
        StartWork(name, callback, false);
    }

    public static void StartWork(string name, CallbackFunc callback, bool stageSelectFlag = true)
    {
        m_this.stageSelectFlag = stageSelectFlag; 

        m_this.moveCount = 0;
        m_this.lookAtCount = 0;

        m_this.lookAtTime = 0;
        m_this.moveTime = 0;

        m_this.isMoveFinish = false;
        m_this.isLookAtFinish = true;

        cam = GameObject.Find("Main Camera");

        Transform tr = GameObject.Find(name).transform;
        m_this.movePositions = new Transform[] { tr };

        isWork = true;

        m_this.currentPos = cam.transform.position;
        m_this.currentRot = cam.transform.rotation;

        breakWorkFlag = true;

        callbackPlayQueue.Enqueue(true);

        callbackFuncQueue = new Queue<CallbackFunc>();
        callbackFuncQueue.Enqueue(callback);
    }

    public static void Pause()
    {
        isWork = false;
    }

    public static void Resume(bool breakWork)
    {
        breakWorkFlag = breakWork;
        isWork = true;
    }

    public static void StopWork()
    {
        DestroyImmediate(point);
        point = null;

        isWork = false;
    }

	// Use this for initialization
	void Start () {
        m_this = this;
    }
	
	// Update is called once per frame
	void Update () {
		if(isWork)
        {
            MoveWork();
            LookAtWork();

            if (point != null)
            {
                cam.transform.LookAt(point.transform);
            }

            if (isMoveFinish && isLookAtFinish)
            {
                StopWork();
            }
        }
	}

    void MoveWork()
    {
        if (m_this.stageSelectFlag)
        {
            moveTime += Time.deltaTime * stageSelectSpeed;
        }
        else
        {
            moveTime += Time.deltaTime * moveSpeed[moveCount];
        }

        cam.transform.position = Vector3.Lerp(currentPos, movePositions[moveCount].position, moveTime);

        if (lookAtPoints.Length == 0 || isLookAtFinish)
        {
            cam.transform.rotation = Quaternion.Lerp(currentRot, movePositions[moveCount].rotation, moveTime);
        }

        if (moveTime >= 1)
        {
            if (moveCount < movePositions.Length)
            {
                moveTime = 0.0f;
                moveCount++;

                currentPos = cam.transform.position;
                currentRot = cam.transform.rotation;

                if (callbackPlayQueue.Dequeue())
                {
                    callbackFuncQueue.Dequeue().Invoke();
                }

                if (moveCount == movePositions.Length)
                {
                    isMoveFinish = true;
                    return;
                }

                if (breakWorkFlag)
                {
                    Pause();
                }
            }
        }
    }

    void LookAtWork()
    {
        if (point == null)
            return;

        if (m_this.stageSelectFlag)
        {
            lookAtTime += Time.deltaTime * stageSelectSpeed;
        }
        else
        {
            lookAtTime += Time.deltaTime * moveSpeed[lookAtCount];
        }

        point.transform.position = Vector3.Lerp(currentLook, lookAtPoints[lookAtCount].position, lookAtTime);

        if (lookAtTime >= 1)
        {
            if (lookAtCount < lookAtPoints.Length)
            {
                lookAtCount++;

                lookAtTime = 0.0f;
                currentLook = point.transform.position;
                
                if (lookAtCount == lookAtPoints.Length)
                {
                    isLookAtFinish = true;
                    return;
                }

                if (breakWorkFlag)
                {
                    Pause();
                }
            }
        }
    }
}