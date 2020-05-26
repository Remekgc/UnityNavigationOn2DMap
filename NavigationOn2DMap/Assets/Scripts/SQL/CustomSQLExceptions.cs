using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EmptyResult : Exception
{
    public EmptyResult()
        : base("Empty query result")
    {
        
    }
}
