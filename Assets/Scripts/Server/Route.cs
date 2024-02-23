using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route
{
    
    public RequestType requestType;
    public string route;

    public string[] controllerParts;

    public Action<Request> methodToInvokeOnRoute;

    public Route(RequestType type, string route, Action<Request> toInvoke) 
    {
        requestType = type;
        this.route = route;
        methodToInvokeOnRoute = toInvoke;
        controllerParts = route.Trim('/').Split('/');
    }

    public bool MatchRoute(byte incomingRequestType, string incomingRoute, out Dictionary<string, string> requestParameters)
    {
       
        requestParameters = new Dictionary<string, string>();

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
                requestParameters[controllerParts[i].TrimStart(':')] = incomingParts[i];
            }
            else if (controllerParts[i] != incomingParts[i])
            {
                return false;
            }
        }

        return true;
    }

}
