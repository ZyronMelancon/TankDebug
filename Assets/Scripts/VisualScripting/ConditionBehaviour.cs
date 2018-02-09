using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionBehaviour : MonoBehaviour
{
    public FloatVariable var1;
    public FloatVariable var2;
    public OperatorVariable op;
    public GameEvent gameEvent;
	
	// Update is called once per frame
	void Update ()
    {
        if (op.Evaluate(var1.Value, var2.Value))
            gameEvent.Raise();
	}
}
