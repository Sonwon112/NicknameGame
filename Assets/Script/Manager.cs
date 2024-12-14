using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Manager
{
    public void gettingMessage(string msg);

    /// <summary>
    /// 현재 audio Mixer의 뮤트 상태를 가져온다
    /// </summary>
    /// <returns></returns>
    public bool getMuteState();
}
