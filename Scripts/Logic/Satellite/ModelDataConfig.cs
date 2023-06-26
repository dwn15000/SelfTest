using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelDataConfig
{
    public float ClosureTime;
    public float ExplosionTime;
    public ModelDataConfig(string c_time,string e_time)
    {
        this.ClosureTime = float.Parse(c_time);
        this.ExplosionTime = float.Parse(e_time); 
    }
}
