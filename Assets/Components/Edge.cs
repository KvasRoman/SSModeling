using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    EdgeSocket socket;
    public int ToId => socket.transform.parent.GetInstanceID();
    public bool HasSocket => socket != null;
    public void SetEdgeSocket(GameObject gObj)
    {
        socket = gObj.GetComponent<EdgeSocket>();
            
    }
    public void UnLockSocket()
    {
        socket.UnLock();
    }
    public EdgeSocket GetSocket()
    {
        return socket;
    }
}
