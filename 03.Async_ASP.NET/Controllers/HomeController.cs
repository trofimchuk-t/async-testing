using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Async_TestParallels.WEB.Controllers
{
    public class HomeController : Controller
    {
        Stopwatch stopwatch = new Stopwatch();
        SomeContext ctx = new SomeContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NonAsyncAction()
        {
            stopwatch.Start();
            var s1 = ctx.GetData1();
            var s2 = ctx.GetData2();
            stopwatch.Stop();

            ViewBag.Message = string.Format("Total time: {0} msec Result1: {1}, Result2: {2}", stopwatch.ElapsedMilliseconds, s1, s2);
            return View();
        }

        public async Task<ActionResult> AsyncAction()
        {
            stopwatch.Start();
            // 01.
            /*
            Task<string> t1 = Task.Factory.StartNew<string>(ctx.GetData1);
            Task<string> t2 = Task.Factory.StartNew<string>(ctx.GetData2);
            */

            // 02.
            Task<string> t1 = GetData1Async(ctx);
            Task<string> t2 = GetData2Async(ctx);

            await Task.WhenAll(t1, t2);
            stopwatch.Stop();

            var s1 = t1.Result;
            var s2 = t2.Result;

            ViewBag.Message = string.Format("Total time: {0} msec Result1: {1}, Result2: {2}", stopwatch.ElapsedMilliseconds, s1, s2);
            return View();
        }

        [AsyncTimeout(1500)]
        [HandleError(ExceptionType = typeof(System.TimeoutException), View = "TimeOutError")]
        public async Task<ActionResult> AsyncCancellationAction(CancellationToken ctk)
        {
            stopwatch.Start();
            Task<string> t1 = Task.Factory.StartNew<string>(ctx.GetData1);
            Task<string> t2 = Task.Factory.StartNew<string>(ctx.GetData2);

            await Task.WhenAll(t1, t2);
            stopwatch.Stop();

            var s1 = t1.Result;
            var s2 = t2.Result;

            ViewBag.Message = string.Format("Total time: {0} msec Result1: {1}, Result2: {2}", stopwatch.ElapsedMilliseconds, s1, s2);
            return View();
        }

        //=====================================================================

        private static Task<string> GetData1Async(SomeContext ctx)
        {
            return Task.Run(() => ctx.GetData1());
        }

        private static Task<string> GetData2Async(SomeContext ctx)
        {
            return Task.Run(() => ctx.GetData2());
        }

    }

    public class SomeContext
    {
        public string GetData1()
        {
            Thread.Sleep(2000); // Simulate some DB connections
            return "Data 1";
        }

        public string GetData2()
        {
            Thread.Sleep(2000); // Simulate some other DB connections
            return "Data 2";
        }
    }
}