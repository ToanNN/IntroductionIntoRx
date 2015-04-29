using System;
using System.ComponentModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveBasics.Creation
{
    public class TransformingSequences
    {
        public static void Run()
        {
            //Select();
            //CastAndOfType();

            //TimeStampAndTimeInterval();

            //MaterializeAndDematerialize();
            SelectMany();
        }

        private static void SelectMany()
        {
             //Observable.Return(5).SelectMany(i=>Observable.Range(1,i)).Dump("Many");

            Func<int, char> letter = i => (char) (i + 64);
            //Recreate the select operation
            Observable.Range(1,5).SelectMany(i=>Observable.Return(letter(i))).Dump("Letters");
        }

        private static void MaterializeAndDematerialize()
        {
            var source = new Subject<int>();

            IObservable<Notification<int>> notifications = source.Materialize();
            notifications.Dump("Notifications");

            source.OnNext(1);
            source.OnNext(2);
            source.OnError(new InvalidAsynchronousStateException("Hello world"));

        }

        private static void TimeStampAndTimeInterval()
        {
            Observable.Interval(TimeSpan.FromSeconds(1)).Take(5).TimeInterval().Dump("Time Interval");


        }

        private static void CastAndOfType()
        {
            var objects = new Subject<object>();
            objects.OfType<int>().Timestamp().Dump("Who That");
            objects.OnNext(1);
            objects.OnNext(10);
            objects.OnNext(100);
            objects.OnNext("Ruby");
        }

        private static void Select()
        {
            Observable.Range(0, 5).Select(it => (char) (it + 64)).Dump("Select");

            var query = from it in Observable.Range(1, 5)
                select (char) (it + 64);

            query.Dump("Query");
        }
    }
}