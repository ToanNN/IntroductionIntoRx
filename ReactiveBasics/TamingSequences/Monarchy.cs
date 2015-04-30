using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ReactiveBasics.TamingSequences
{
    public class Monarchy
    {
        public static void Run()
        {
            //ForEachSoldier();

            //ToEnumerable();

            //ToArrayOrList();

            //ToTask();

            //ToEvent();

            ToEventPattern();
        }

        private static void ToEventPattern()
        {
            IObservable<EventPattern<MyEventArgs>> source = Observable.Interval(TimeSpan.FromSeconds(1))
                                                           .Select(it => new EventPattern<MyEventArgs>(null, new MyEventArgs(it)));

            IEventPatternSource<MyEventArgs> resultPatterns = source.ToEventPattern();

            resultPatterns.OnNext += (sender, args) => Console.WriteLine(args.Value);


        }

        private static void ToEvent()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);

            IEventSource<long> eventSource = source.ToEvent();


            eventSource.OnNext += l => Console.WriteLine("Value : {0}", l);



        }

        private static void ToTask()
        {
            var source = Observable.Interval(TimeSpan.FromSeconds(1)).Take(5);

            Task<long> res = source.ToTask();

            Console.WriteLine("Waiting");
            Console.WriteLine(res.Result);  //Wait for 5 seconds to come

        }

        private static void ToArrayOrList()
        {
            var subject = new Subject<int>();

            //evaluate immediately 
            //IObservable<int[]> arrays = subject.Lis();

            IObservable<IList<int>> list = subject.ToList();

            list.Subscribe(arr =>
            {
                foreach (var i in arr)
                {
                    Console.WriteLine(i);
                }
            });

            subject.OnNext(10);
            subject.OnNext(20);
        }

        private static void ToEnumerable()
        {
            var subject = new Subject<int>();
            //Evaluate immediately
            var result = subject.ToEnumerable();

            //No element at this point
            foreach (var i in result)
            {
                Console.WriteLine(i);
            }

            subject.OnNext(1);
            subject.OnNext(10);
        }

        private static void ForEachSoldier()
        {
            var subject = new Subject<int>();

            //The task will be notified when the sequence completes
            Task task =subject.ForEachAsync(val => Console.WriteLine("Foreach received {0}", val));

            subject.OnNext(10);

            Console.WriteLine("Main Finished");


            task.ContinueWith(ant => Console.WriteLine("The Foreach Task finished"));

            subject.OnCompleted();
        }
    }
}