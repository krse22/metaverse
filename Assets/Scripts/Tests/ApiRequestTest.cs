using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiRequestTest : MonoBehaviour
{
    
    public void SendApiRequestToServer()
    {
        TCPClient.Post("api/character", "jsonBody", OnResponse);
    }

    void OnResponse()
    {
        Debug.Log("Received resposne for api/character");
    }

}
