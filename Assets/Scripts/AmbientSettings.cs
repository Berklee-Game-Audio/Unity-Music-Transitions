using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRange
{
    int minValue;
    int maxValue;
}

public abstract class AmbientSettings: ScriptableObject
{
    public List<AudioSettings> audioSettings = new List<AudioSettings>();
    public RandomRange randomRange = new RandomRange();


    // Start is called before the first frame update
    void StartAmbient()
    {
        
    }

    
}
