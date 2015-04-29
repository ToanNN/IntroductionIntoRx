using System;
using System.ComponentModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;

namespace ReactiveBasics
{
    public class TransitioningIntoObservables
    {
        public static void Run()
        {
           // StartUnit();
            //FromEvents();

            FromAPM();
        }

        private static void FromAPM()
        {
            using (var fileStream = File.OpenRead(@"c:\temp\data.txt"))
            {
                var fileLength = fileStream.Length;

                var buffer = new byte[fileLength];
                IObservable<int> read = Observable.FromAsync(ct => fileStream.ReadAsync(buffer, 0, (int) fileLength, ct));

                read.Subscribe(byteCount => Console.WriteLine("Read {0} bytes", byteCount));
            }
        
        }

        private static void FromEvents()
        {


            //IObservable<EventPattern<AssemblyLoadEventArgs>> appActivated = Observable.FromEventPattern<AssemblyLoadEventHandler, AssemblyLoadEventArgs>(
            //   h=>h.Invoke,
            //    h => AppDomain.CurrentDomain.AssemblyLoad += h,
            //    h => AppDomain.CurrentDomain.AssemblyLoad -= h);

            //appActivated.Subscribe((EventPattern<AssemblyLoadEventArgs> ip) => Console.WriteLine(ip.EventArgs.LoadedAssembly.FullName));

           

        }

        private static void StartUnit()
        {
            //The consuming work is wrapped in a Unit
            IObservable<string> units = Observable.Start(() => {

                
                                                                 Console.WriteLine("Performing the work .....");
                                                                 for (int i = 0; i < 10; i++)
                                                                 {
                                                                     Thread.Sleep(100);
                                                                     Console.Write(".");
                                                                 }

                                                                 return "Who Zat";
            });

            units.Subscribe(Console.WriteLine,   
                () => Console.WriteLine("All units have been completed"));

            //Print All dots and then A Unit it processed
        }
    }
}