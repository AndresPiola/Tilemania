using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FNotify ( );
public delegate void FNotify_1Params<T>(T _val1);

public delegate void FNotify_2Params<T1, T2>(T1 _val1, T2 _val2);
public delegate void FNotify_3Params<T1, T2,T3>(T1 _val1, T2 _val2, T3 _val3);