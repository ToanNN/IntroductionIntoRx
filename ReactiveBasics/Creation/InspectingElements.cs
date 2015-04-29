using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Security.Policy;

namespace ReactiveBasics
{
    public class InspectingElements
    {
        public static void Run()
        {
            //UseAny();

            //UseContains();

            SequenceEqual();

            //Counting();
            //Min();


        }

        private static void Min()
        {
            var numbers = new Subject<int>();
            //numbers.Dump("Numbers");

            //Evaluated when the original sequence completes
            IObservable<int> mins=numbers.Min();

            mins.Dump("Min");

            numbers.OnNext(1);
            numbers.OnNext(2);
            numbers.OnNext(100);
            numbers.OnNext(-1);
            numbers.OnCompleted();


        }

        private static void Counting()
        {
            var numbers = Observable.Range(0, 8);
            numbers.Dump("numbers");
            numbers.Count().Dump("count");


        }

        private static void SequenceEqual()
        {
            var subject1 = new Subject<int>();
            var subject2 = new Subject<int>();

            IObservable<bool> sequenceEqual = subject1.SequenceEqual(subject2);
            // Must be subscribe before posting elements to the sequence
            //The result will be returned immeditately if there is unmatched element or when we call OnCompleted on the sequences
            sequenceEqual.Subscribe(b => Console.WriteLine("Two sequences are equal ? {0}", b),()=>Console.WriteLine("Sequence Completed"));

            subject1.OnNext(1);
            subject1.OnNext(2);

            subject2.OnNext(1);
            subject2.OnNext(3);

            subject1.OnCompleted();
            subject2.OnCompleted();

        }

        private static void UseContains()
        {
            var subject = new Subject<int>();

            IObservable<bool> containSubject = subject.Contains(10);

            //The observer will be notified when the sequence has the searching element or it completes
            containSubject.Subscribe(b => Console.WriteLine("The sequence contains 10 ? {0}", b));

            subject.OnNext(1);
            subject.OnNext(3);
            subject.OnNext(10);
            //Console.ReadLine();
            subject.OnCompleted();

          

        }

        private static void UseAny()
        {
            var subject = new Subject<int>();

            //Test the sequence and completes immediately
            subject.Any().Subscribe(Console.WriteLine, () => Console.WriteLine("The sequence has completed"));

            subject.OnNext(10);
        }
    }
}