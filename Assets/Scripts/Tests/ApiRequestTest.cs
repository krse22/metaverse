using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApiRequestTest : MonoBehaviour
{
    
    public void SendApiRequestToServer()
    {
        TCPClient.Get("api/test?friend=test&yes=and", "jsonBody", OnResponse);
    }

    public void SendTestWallets()
    {
        TCPClient.Get("api/test/:id/wallets/:testic", "jsonBody", (Response response) => { return; });
    }

    void OnResponse(Response response)
    {
        Debug.Log(response.body);
    }

}
