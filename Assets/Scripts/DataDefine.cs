using UnityEngine;
using System.Collections.Generic;


public interface IStage
{
    void Init();
}


public class Stage1_DataMgr : IStage
{
    public static int currentRound = 0;
    static List<Stage1_Data> list;

    public void Init()
    {
        list = new List<Stage1_Data>();

        string noteData = Resources.Load<TextAsset>("testData").text;

        string[] stageDatas = noteData.Split(new string[] { "#" }, System.StringSplitOptions.RemoveEmptyEntries);

        for(int i = 0; i<stageDatas.Length; i++)
        {
            Stage1_Data data = new Stage1_Data();
            data.SetData(stageDatas[i].Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries));

            list.Add(data);
        }
    }

    public static Stage1_Data GetStageData(int round)
    {
        if (list.Count <= round)
            currentRound = round = 0;

        return list[round];
    }
}

public struct Stage1_Data
{
    public int stage;
    public int attackCount;
    public int chance;
    public string note;

    public void SetData(string[] data)
    {
        int cnt = 0;

        stage = data[cnt].ToInt(); cnt++;
        attackCount = data[cnt].ToInt(); cnt++;
        chance = data[cnt].ToInt(); cnt++;
        note = data[cnt]; cnt++;
    }
}

public class Nishu_DataMgr : IStage
{
    public static int currentRound = 0;
    static List<Nishu_Data> list;

    public void Init()
    {
        list = new List<Nishu_Data>();

        string noteData = Resources.Load<TextAsset>("testNishuData").text;

        string[] stageDatas = noteData.Split(new string[] { "#" }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < stageDatas.Length; i++)
        {
            Nishu_Data data = new Nishu_Data();
            data.SetData(stageDatas[i].Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries));

            list.Add(data);
        }
    }

    public static Nishu_Data GetStageData(int round)
    {
        if (list.Count <= round)
            currentRound = round = 0;

        return list[round];
    }
}

public struct Nishu_Data
{
    public int stage;
    public int chance;
    public string note;

    public void SetData(string[] data)
    {
        int cnt = 0;

        stage = data[cnt].ToInt(); cnt++;
        chance = data[cnt].ToInt(); cnt++;
        note = data[cnt]; cnt++;
    }
}

public class StageCounter_DataMgr : IStage
{
    public static int currentRound = 0;
    static List<StageCounter_Data> list;

    public void Init()
    {
        list = new List<StageCounter_Data>();

        string noteData = Resources.Load<TextAsset>("testCounterData").text;

        string[] stageDatas = noteData.Split(new string[] { "#" }, System.StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < stageDatas.Length; i++)
        {
            StageCounter_Data data = new StageCounter_Data();
            data.SetData(stageDatas[i].Split(new string[] { "\r\n" }, System.StringSplitOptions.RemoveEmptyEntries));

            list.Add(data);
        }
    }

    public static StageCounter_Data GetStageData(int round)
    {
        if (list.Count <= round)
            currentRound = round = 0;

        return list[round];
    }
}

public struct StageCounter_Data
{
    public int stage;
    public int counterCount;
    public int staminaCount;
    public int chance;
    public string note;

    public void SetData(string[] data)
    {
        int cnt = 0;

        stage = data[cnt].ToInt(); cnt++;
        counterCount = data[cnt].ToInt(); cnt++;
        staminaCount = data[cnt].ToInt(); cnt++;
        chance = data[cnt].ToInt(); cnt++;
        note = data[cnt]; cnt++;
    }
}
