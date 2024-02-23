using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route
{
    
    public RequestType requestType;
    public string route;

    public string[] controllerParts;

    public Route(RequestType type, string route) 
    {
        requestType = type;
        this.route = route;
        controllerParts = route.Trim('/').Split('/');
    }

    public bool MatchRoute(byte incomingRequestType, string incomingRoute)
    {
       
        if (incomingRequestType != (byte)requestType)
        {
            return false;
        }

        string[] incomingParts = incomingRoute.Trim('/').Split('/');
        if (incomingParts.Length != controllerParts.Length)
        {
            return false;
        }

        for (int i = 0; i < controllerParts.Length; i++)
        {
            if (controllerParts[i].StartsWith(":"))
            {
               
            }
            else if (controllerParts[i] != incomingParts[i])
            {
                return false;
            }
        }

        return true;
    }

}
