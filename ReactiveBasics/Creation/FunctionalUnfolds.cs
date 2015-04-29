using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace ReactiveBasics
{
    public class FunctionalUnfolds
    {
        public static void Run()
        {
            //Cocersion();

            //Create a range of 15 integers from 10 to 24
            //IObservable<int> range = Observable.Range(10, 15);
            //range.Subscribe(Console.WriteLine, () => Console.WriteLine("So 6"));

            //UseRange();

            //UseInterval();
            
            //UseTimer();
            UseGenerator2GenerateTimerData();
        }

        private static void UseGenerator2GenerateTimerData()
        {
            //IObservable<long> data = MyObservableExtensions.Interval(TimeSpan.FromSeconds(1));

            IObservable<long> data = MyObservableExtensions.Timer(TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(1));

            data.Subscribe(Console.WriteLine);
        }

        private static void UseTimer()
        {
            //var timer = Observable.Timer(TimeSpan.FromSeconds(1));
            ////This source data finishes immediately after the first broadcast (0)
            //timer.Subscribe(Console.WriteLine,()=>Console.WriteLine("Bi NGa"));

            //Start now and then repeat every second
            IObservable<long> startNowTimer = Observable.Timer(TimeSpan.Zero, TimeSpan.FromSeconds(1));
            startNowTimer.Subscribe(Console.WriteLine, () => Console.WriteLine("Starting Now Completed"));
        }

        private static void UseInterval()
        {
//Increase by 1 from 0 every 2 second
            var pulses = Observable.Interval(TimeSpan.FromSeconds(2));
            pulses.Subscribe(Console.WriteLine);
        }

        private static void UseRange()
        {
            IObservable<int> myRange = MyObservableExtensions.Range(10, 15);
            myRange.Subscribe(Console.WriteLine, () => Console.WriteLine("So 15"));
        }


        private static void Cocersion()
        {
            var naturalNumbers = Unfold(1, i => i + 1);
            Console.WriteLine("10 Numbers");
            foreach (var number in naturalNumbers.Take(10))
            {
                Console.WriteLine(number);
            }
        }

        private static IEnumerable<T> Unfold<T>(T seed, Func<T, T> accumulator)
        {
            var nextValue = seed;
            while (true)
            {
                yield return nextValue;

                nextValue = accumulator(nextValue);
            }
        }
    }
}