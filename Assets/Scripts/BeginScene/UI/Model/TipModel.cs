using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TipModel
{
    public string Content { get; set; }
    public UnityAction ConfirmCallback { get; set; }
    public bool NeedPauseGame { get; set; }
}
