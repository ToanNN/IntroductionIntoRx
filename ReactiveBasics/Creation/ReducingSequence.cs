using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveBasics
{
    public class ReducingSequence
    {
        public static void Run()
        {
            //WhereAreYou();
            //DistinctYourself();

            //SkipOrTake();

            SkipOrTakeUntil();
        }

        private static void SkipOrTakeUntil()
        {
            var subject = new Subject<int>();

            var otherSubject = new Subject<int>();

            subject.OnNext(15);

            subject.SkipUntil(otherSubject).Subscribe(Console.WriteLine, () => Console.WriteLine("The Subject has completed"));;

            subject.OnNext(10);

            otherSubject.OnNext(1);  //Subject Accepts elements from this points on
            subject.OnNext(20);

            
        }

        private static void SkipOrTake()
        {
            Observable.Range(0, 10).Skip(3).Take(5).Subscribe(Console.WriteLine);

            Observable.Range(10, 16).SkipWhile(it => it < 15).TakeWhile(it => it < 20).Subscribe(Console.WriteLine);
        }

        private static void DistinctYourself()
        {
            var subject = new Subject<int>();
            //Show only distinct numbers
            subject.DistinctUntilChanged().Subscribe(Console.WriteLine);
            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(2);

        }

        private static void WhereAreYou()
        {
            var subject = new Subject<int>();

            //Only run for even numbers
            subject.Where(it => it%2 == 0).Subscribe(it => Console.WriteLine("Received value: {0}", it));

            subject.OnNext(1);
            subject.OnNext(2);
            subject.OnNext(3);
            subject.OnNext(8);
        }
    }
}