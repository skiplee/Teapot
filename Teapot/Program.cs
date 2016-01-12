using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Topshelf;
using Topshelf.ServiceConfigurators;
using Timer = System.Timers.Timer;

namespace Teapot
{
    class Program
    {
        public static void Main(string[] args)
        {
            StartWindowsService();
        }

        private static void StartWindowsService()
        {
            HostFactory.Run(x =>
            {
                x.Service<TeapotService>(s =>
                {
                    s.ConstructUsing(name => new TeapotService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalService();
                x.StartAutomatically();
                x.SetDescription("Service to sing I'm a Little Teapot");
                x.SetDisplayName("Teapot");
                x.SetServiceName("Teapot");
            });
        }
    }

    public class TeapotService
    {
        private Timer _timer;

        public void Start()
        {
            _timer = new Timer();
            _timer.AutoReset = true;
            _timer.Elapsed += Song.WriteLyrics;
            _timer.Interval =4000;
            _timer.Enabled = true;
        }

        public void Stop()
        {
            _timer.Stop();
        }
    }

    internal static class Song
    {
        public static IList<string> Lyrics()
        {
            return new List<string>
            {
                "I'm a little teapot, short and stout,",
                "here is my handle, here is my spout.",
                "When I get excited hear me shout,",
                "TIP ME OVER AND POUR ME OUT!",
                "\n"
            };
        }

        public static void WriteLyrics(object sender, ElapsedEventArgs e)
        {
            foreach (var lyric in Lyrics())
            {
                Console.WriteLine(lyric);
                Thread.Sleep(500);
            }
        }
    }
}