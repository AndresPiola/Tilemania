using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Utils : MonoBehaviour {

    public static T RandomEnumValue<T>()
    {
        var values = Enum.GetValues(typeof(T));
        int random = UnityEngine.Random.Range(0, values.Length);
        return (T)values.GetValue(random);
    }

    public static int GetRandomIndex<T>(T[] _array, int _maxLenght = -1)
    {
        int randIndex = Random.Range(0, _array.Length);

        

        return randIndex;

    }
    public static T  GetRandomItem<T>(T[] _array,int _maxLenght=-1)
    {
        int randIndex=Random.Range(0,_array.Length);

        if(_maxLenght>0)
        {
            randIndex = Mathf.Min(randIndex, _maxLenght);

        }

        return _array[randIndex];

    }

    public static T[] Shuffle<T>(T[] list)
    {
        System.Random rng = new System.Random();
        int n = list.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        return list;
    }

    
    public  static void Swap<T>(IList<T> list, int indexA, int indexB)
    {
        T tmp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = tmp;
    }
    public static int[] IntToIntArray(int num)
    {
        if (num == 0)
            return new int[1] { 0 };

        List<int> digits = new List<int>();

        for (; num != 0; num /= 10)
            digits.Add(num % 10);

        int[] array = digits.ToArray();
        System.Array.Reverse(array);

        return array;
    }
    public static Vector3 DirectionToVector(EDirections _dire)
    {

        if (_dire == EDirections.DOWN) return Vector3.back;
        if (_dire == EDirections.UP) return Vector3.forward;
        if (_dire == EDirections.LEFT) return Vector3.left;
        if (_dire == EDirections.RIGHT) return Vector3.right;

        return Vector3.zero;

    }
    //////////////////////////////////////////////////////////////////////////
    ///coroutines
    static Dictionary<float, WaitForSeconds> _timeInterval = new Dictionary<float, WaitForSeconds>(100);
    
    static WaitForEndOfFrame _endOfFrame = new WaitForEndOfFrame();
    public static WaitForEndOfFrame EndOfFrame
    {
        get { return _endOfFrame; }
    }

    static WaitForFixedUpdate _fixedUpdate = new WaitForFixedUpdate();
    public static WaitForFixedUpdate FixedUpdate
    {
        get { return _fixedUpdate; }
    }
    
    public static WaitForSeconds GetWaitForSeconds(float seconds)
    {
        if (!_timeInterval.ContainsKey(seconds))
            _timeInterval.Add(seconds, new WaitForSeconds(seconds));
        return _timeInterval[seconds];
    }
}
