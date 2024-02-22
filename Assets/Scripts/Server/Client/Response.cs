using System;

public class Response 
{
    public static int globalTimeoutTimeMs = 5000;

    public byte ResponseId;
    public Action onResponse;

    public void StartTimeoutCounter()
    {
        // Some object will be needed where it will invoke timeout
        onResponse.Invoke();
    }

    public void InvokeResponse() 
    {
        onResponse.Invoke();
    }

}
