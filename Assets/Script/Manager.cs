using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Manager
{
    public void gettingMessage(string msg);

    /// <summary>
    /// ���� audio Mixer�� ��Ʈ ���¸� �����´�
    /// </summary>
    /// <returns></returns>
    public bool getMuteState();
}
