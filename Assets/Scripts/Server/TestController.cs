using UnityEngine;

[Controller("api/test")]
public class TestController : MonoBehaviour
{

    [Get("")]
    public void index(Request request) 
    {
        Debug.Log("Index called");
        Debug.Log($"Index called on {transform.name}");
        Debug.Log(request.query);
    }

    [Get("tests")]
    public void tests(Request request)
    {
        Debug.Log("invoked tests");
    }

    [Get(":id")]
    public void getSingleTest(Request request)
    {
        Debug.Log("Get single test");
    }

    [Post(":id")]
    public void createTest(Request request)
    {
        Debug.Log("Creating test");
    }

    [Post("tests/more-tests/:id")]
    public void massiveTest(Request request)
    {
        Debug.Log("Massive test");
    }

}
