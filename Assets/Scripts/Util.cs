using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {
  public static void Log(params object[] items) {
    string str = "";
    foreach (object item in items) {
      str += item;
      str += " ";
    }
    Debug.Log(str);
  }

  public static float Mod(float x, float m) {
    return (x % m + m) % m;
  }
}
