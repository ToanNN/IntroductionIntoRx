using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReactiveExtensionsInUse
{
    class Program
    {
        static void Main(string[] args)
        {
            //RunNumberSequence();

           // ShowMeYourSubjects();
           // UseSubjectAsProxy();
            //UseReplaySubject();

            //UseBehaviorSubject();
            //UseAsyncSubject();

            LifetimeManagement.Run();
            
            Console.ReadLine();
        }

        private static void UseAsyncSubject()
        {
            var subject = new AsyncSubject<string>();
            subject.OnNext("WHO ZAT");
            WriteSequenceToConsole(subject);
            subject.OnNext("IT's Me");
            subject.OnNext("Cry on");
            subject.OnCompleted();  //Print "Cry on"
        }

        private static void UseBehaviorSubject()
        {
            var subject = new BehaviorSubject<string>("Cookoo");
            subject.OnNext("Bundaberg");  //This will be shown on the console
            
            WriteSequenceToConsole(subject);
            subject.OnNext("Roma");
        }

        private static void UseReplaySubject()
        {
            var cachingTime = TimeSpan.FromMilliseconds(100);
            var subject = new ReplaySubject<string>(window:cachingTime);
          
            subject.OnNext("d");
            Thread.Sleep(TimeSpan.FromMilliseconds(200));
            
            subject.OnNext("e");
            Thread.Sleep(TimeSpan.FromMilliseconds(200));
            subject.OnNext("f");
            WriteSequenceToConsole(subject);  //Print f only


        }

        private static void UseSubjectAsProxy()
        {
            //Create the source of the data
            var source = Observable.Interval(TimeSpan.FromSeconds(5));

            //This is the proxy that will subscribe for the data coming out of the source
            Subject<long> subject = new Subject<long>();
            
            //Subscribe to the data stream
           var proxySubscription=   source.Subscribe(subject);

            //Forwards the data to other subscribers
            var subSubject1 = subject.Subscribe(x => Console.WriteLine("Value published to observer  1 {0}", x),
                                                ()=>Console.WriteLine("Sequence Completed"));

            var subSubject2 = subject.Subscribe(
                         x => Console.WriteLine("Value published to observer #2: {0}", x),
                         () => Console.WriteLine("Sequence Completed."));

            subject.OnNext(1);

            subject.OnNext(2);

            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
            subject.OnCompleted();

            subSubject1.Dispose();
            subSubject2.Dispose();
            proxySubscription.Dispose();
        }

        private static void ShowMeYourSubjects()
        {
            var subject = new Subject<string>();
            WriteSequenceToConsole(subject);
            Console.WriteLine("Press any key to inject data");
            Console.ReadKey();
            subject.OnNext("Who cares?");

            Console.WriteLine("Press any key to inject data");
            Console.ReadKey();
            subject.OnNext("Lunch Time");

            
        }

        private static void WriteSequenceToConsole(IObservable<string> sequence)
        {
            sequence.Subscribe(Console.WriteLine);
        }


        private static void RunNumberSequence()
        {
            var sequence = new MySequenceOfNumbers();
            var observer = new MyConsoleObserver<int>();
            var observer2 = new MyConsoleObserver<int>();

            sequence.Subscribe(observer);
            sequence.Subscribe(observer2);


        }
    }
}
