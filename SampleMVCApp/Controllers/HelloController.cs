using Microsoft.AspNetCore.Mvc;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;

namespace SampleMVCApp.Controllers;

public class HelloController : Controller
{

    [HttpGet("Hello/{id?}/{name?}")]
    public IActionResult Index(int id, string name)
    {
        ViewData["message"] = "※セッションにIDとNameを保存しました。";
        MyData ob = new MyData(id, name);
        String s = ObjectToString(ob);
        HttpContext.Session.SetString("object", s);
        ViewData["object"] = ob;
        return View();
    }

    [HttpGet("Other")]
    public IActionResult Other()
    {
        ViewData["message"] = "保存されたセッションの値を表示します。";
        String s = HttpContext.Session.GetString("object") ?? "";
        ViewData["object"] = StringToObject(s);
        return View("Index");
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