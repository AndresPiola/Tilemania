using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FNotify ( );
public delegate void FNotify_1Params<T>(T Param1);

public delegate void FNotify_2Params<T1, T2>(T1 Param1, T2 Param2);
public delegate void FNotify_3Params<T1, T2,T3>(T1 Param1, T2 Param2, T3 Param3);