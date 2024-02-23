using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Controller("api/test/:id/wallets")]
public class TestWalletsController : MonoBehaviour
{

    [Get(":id")]
    public void GetWallets()
    {

    }

}
