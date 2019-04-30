using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class VariableReference {

    // Store delegates in the dictionary that will allow you to retrieve and modify the variables.
    public Func<object> Get { get; private set; }
    public Action<object> Set { get; private set; }
    public Type GetCustType { get; private set; }
    public VariableReference(Func<object> getter, Action<object> setter, Type type)
    {
        Get = getter;
        Set = setter;
        GetCustType = type;
    }
}
