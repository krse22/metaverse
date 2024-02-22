using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiRequestTest : MonoBehaviour
{
    
    public void SendApiRequestToServer()
    {
        TCPClient.Post("api/character", "jsonBody", OnResponse);
    }

    void OnResponse(Response response)
    {
        Debug.Log(response.body);
    }

}
