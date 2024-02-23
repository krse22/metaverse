using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Controller("api/test/:id/wallets")]
public class TestWalletsController : MonoBehaviour
{

    [Get(":walletId")]
    public void GetWallets(Request request)
    {
        foreach (string key in request.requestParams.Keys)
        {
            Debug.Log($"PARAM: [{key}]={request.requestParams[key]}");
        }

        Debug.Log($" Transform {transform.position}");
    }

}
