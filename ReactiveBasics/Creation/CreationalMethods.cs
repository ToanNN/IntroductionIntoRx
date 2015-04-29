using System;
using System.Diagnostics;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Timers;

namespace ReactiveBasics
{
    public class CreationalMethods
    {
        public static void Run()
        {
            //The source data 
            //IObservable<string> source = NonBlocking();

            //Console.WriteLine("Subscribing to the data");
            ////Owing to method extensions, we can subscribe to data using an Action delegate
            //source.Subscribe(Console.WriteLine);

            //source.Subscribe(st => Debug.WriteLine(st));

            //IObservable<int> return2Hell = MyObservableExtensions.Return(10);

            //return2Hell.Subscribe(Console.WriteLine);

            //return2Hell.Subscribe(st => Debug.WriteLine(st));

            NonBlocking_Event_Driven();


        }

        private static void NonBlocking_Event_Driven()
        {
            IObservable<string> sourceData = Observable.Create((IObserver<string> observer) =>
            {
                var timer = new System.Timers.Timer();
                timer.Interval = 1000;
                timer.Elapsed += (s, e) => observer.OnNext("On Next: " + DateTime.Now.ToShortTimeString());
                //This subscription will continue running even after the subscription has been dispose beca
                timer.Elapsed += OnTimeElapsed;
                timer.Start();

                //Incorrect return the object
                //return Disposable.Empty;


                //correct
                //return timer;

                //Or using the action which will be called when the subscription was removed

                return () =>
                {
                    timer.Elapsed -= OnTimeElapsed;
                    timer.Dispose();
                };

            });

            
            var subscription = sourceData.Subscribe(st => Debug.WriteLine(st));


            Console.ReadLine();
            Console.WriteLine("Disposing the subscription");
            subscription.Dispose();


        }

        private static void OnTimeElapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine("Signaled: " + e.SignalTime);
        }


        private static IObservable<string> NonBlocking()
        {
            return Observable.Create<string>(
                //This will be called for each subscriber
                (IObserver<string> observer) =>
                {
                    //feeding data to the observer
                    observer.OnNext("North Lakes");
                    observer.OnNext("Dakabin");
                    Thread.Sleep(1000);
                    observer.OnNext("================After sleeping=====================");
                    observer.OnNext("Ashgrove");
                    observer.OnNext("Paddington");

                    observer.OnCompleted();

                    //return a disposable object to unsubscribe
                    return Disposable.Create(() => Console.WriteLine("Observer has unsubscribed"));
                });
        }

        private static IObservable<string> BlockingMethod()
        {
            var subject = new ReplaySubject<string>();

            subject.OnNext("10");
            subject.OnNext("Dakabin");
            subject.OnNext("North Lakes");
            subject.OnCompleted();
            //Block 1 second before the subject can be returned
            Thread.Sleep(1000);
            return subject;

        }





        private static void UseObservableReturn()
        {
            IObservable<string> returnNow = Observable.Return("Who is the last murtha fucka breathing?");
            IObservable<int> emptySequence = Observable.Empty<int>();

            IObservable<string> neverEver = Observable.Never<string>(); // = new Subject<string>()

            IObservable<InvalidOperationException> alwaysThrow = Observable.Throw<InvalidOperationException>(new InvalidOperationException("What's up"));
            //= new ReplaySubject<InvalidOperationException>(new InvalidOperationException("What's up"))

            WriteSequenceToConsole(neverEver);

            WriteSequenceToConsole(returnNow);

        }
        private static void WriteSequenceToConsole(IObservable<string> sequence)
        {

            sequence.Subscribe(Console.WriteLine);
        }
    }
}