using System.Collections.Generic;
using UnityEngine.UI;

public class Parameters : Singleton<Parameters>
{
    public List<float> delay;
    public List<int> force;
    public int totalTrials;
    public int trialNumber;
    public int condition;
    public List<float> autoShootDelay;
    public List<int> barrierPosition;
    public int ID;
    public List<Dropdown.OptionData> listIDs;
    public string outpath;

    public string outputHeader = "ID,TrialNumber,Condition,Delay(s),AutoShootDelay(s),Force,BarrierXpos(pixel)," // input
                                 + "VelocityX,VelocityY,CollisionY(pixel),Output"; // output

    protected Parameters()
    {
    }

    private void Awake()
    {
        ID = 0;
        force = new List<int>();
        autoShootDelay = new List<float>();
        barrierPosition = new List<int>();
        delay = new List<float>();
        listIDs = new List<Dropdown.OptionData>();
    }

    public float GetDelay()
    {
        return delay[trialNumber];
    }

    public float GetAutoShootDelay()
    {
        return autoShootDelay[trialNumber];
    }

    public float GetBarrierPosition()
    {
        return barrierPosition[trialNumber];
    }

    public float GetForce()
    {
        return force[trialNumber];
    }

    public void SetPath()
    {
        outpath = @"Log\" + ID.ToString() + @"\output\" +
                  ID.ToString() + "." + condition.ToString() + "." +
                  System.DateTime.Now.ToString("yy-MM-dd_HH-mm-ss") + ".csv";
    }
}