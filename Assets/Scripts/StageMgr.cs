using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMgr : MonoBehaviour
{

    static List<IStage> stageLIst;

    Stage currentStage;
    
    void Awake()
    {
        DontDestroyOnLoad(this);
        LoadDataTable();
    }

    void LoadDataTable()
    {
        stageLIst = new List<IStage>();

        //Stage 1 Load
        Stage1_DataMgr mgr1 = new Stage1_DataMgr();
        mgr1.Init();
        stageLIst.Add(mgr1);
        //>
        Nishu_DataMgr mgr2 = new Nishu_DataMgr();
        mgr2.Init();
        stageLIst.Add(mgr2);

        StageCounter_DataMgr mgr3 = new StageCounter_DataMgr();
        mgr3.Init();
        stageLIst.Add(mgr3);
    }

    public T GetDataMgr<T>()
    {
        return (T)stageLIst.Find(node => node is T);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public Stage GetStage()
    {
        return currentStage;
    }

    public void Init(Stage stage)
    {
        currentStage = stage;
        currentStage.Init();
    }

    public void StageComplete()
    {
        currentStage.StageComplete();
    }

    public void StageCompleteFinish()
    {
        currentStage.StageSuccessFinish();
    }

    public void StageFailed()
    {
        currentStage.StageFailed();
    }

    public void StageFailedFinish()
    {
        currentStage.StageFailedFinish();
    }

    public void StageStart()
    {
        currentStage.StartGame();
    }
}