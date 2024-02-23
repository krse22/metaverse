using UnityEngine;

[Controller("api/test")]
public class TestController : MonoBehaviour
{

    [Get("")]
    public void index() 
    {
        Debug.Log("Index called");
    }

    [Get("tests")]
    public void tests()
    {
        Debug.Log("invoked tests");
    }

    [Get(":id")]
    public void getSingleTest()
    {
        Debug.Log("Get single test");
    }

    [Post(":id")]
    public void createTest()
    {
        Debug.Log("Creating test");
    }

    [Post("tests/more-tests/:id")]
    public void massiveTest()
    {
        Debug.Log("Massive test");
    }

}
