using System.Diagnostics;

namespace myTask.Middlewares{
    public class TheTaskLogMiddeleware
{
    private RequestDelegate next;
    private string logger;

    public TheTaskLogMiddeleware(RequestDelegate next, string logger)
    {
        this.next = next;
        this.logger = logger;
    }

    public async Task Invoke(HttpContext c)
    {
        var sw = new Stopwatch();
        sw.Start();
        await next(c);
//         יש לכתוב כל בקשה לקובץ יומן, כולל תאריך ושעה התחלה, שם של
// בקר ופעולה, שם המשתמש המחובר אם קיים, ומשך הזמן של
// פעולה באלפיות שניות.
        WriteLogToFile($"{DateTime.Now} {c.Request.Path}.{c.Request.Method} took {sw.ElapsedMilliseconds}ms."
            + $" User: {c.User?.FindFirst("Id")?.Value ?? "unknown"}");     
    }    

   private void WriteLogToFile(string logMessage)
    {
        using (StreamWriter sw = File.AppendText(logger))
        {
            sw.WriteLine(logMessage);
        }
    }
}

public static partial class MyMiddleExtensions
{
    public static IApplicationBuilder UseMyMiddleExtensions(this IApplicationBuilder builder,string FilePath)
    {
        return builder.UseMiddleware<TheTaskLogMiddeleware>(FilePath);
    }
}
}

