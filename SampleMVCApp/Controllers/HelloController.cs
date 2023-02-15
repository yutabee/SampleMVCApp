using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace SampleMVCApp.Controllers;

public class HelloController : Controller
{

    [HttpGet]
    public IActionResult Index()
    {
        ViewData["message"] = "※テーブルの表示";
        ViewData["header"] = new string[] { "id", "name", "mail" };
        ViewData["data"] = new string[][]{
        new string[]{ "1", "Taro", "taro@yamada"},
        new string[]{ "2", "Hanako", "hanako@flower"},
        new string[]{ "3", "Sachiko", "sachiko@happy"}
    };
        return View();
    }


    // convert object to String.
    private String ObjectToString(MyData ob)
    {
        return JsonSerializer.Serialize<MyData>(ob);
    }

    // convert String to object.
    private MyData? StringToObject(String s)
    {
        MyData? ob;
        try
        {
            ob = JsonSerializer.Deserialize<MyData>(s);
        }
        catch (Exception e)
        {
            ob = new MyData(0, "noname");
        }
        return ob;
    }
}

[Serializable]
class MyData
{
    public int Id { get; set; }
    public string Name { get; set; }

    public MyData(int id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    override public string ToString()
    {
        return "<" + Id + ": " + Name + ">";
    }
}