using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveBasics.Creation
{
    public class FunctionalFolds
    {
        public static void Run()
        {
            //UseFirst();

            //UseAggregate();
            //Scan();

            //CustomRunningMinMax();

            Partitioning();
        }

        private static void Partitioning()
        {
            IObservable<long> source = Observable.Interval(TimeSpan.FromSeconds(0.1)).Take(20);

            IObservable<IGroupedObservable<long,long>> groups = source.GroupBy(it => it%3);

            groups.Subscribe(grp => grp.Max().Subscribe(minVal => Console.WriteLine("{0} Key has Max {1}", grp.Key, minVal)),
                ()=>Console.WriteLine("All groups completed"));


        }

        private static void CustomRunningMinMax()
        {
            var numbers = new Subject<int>();
            //numbers.RunningMin().Dump("Running Min");

            numbers.RunningMax().Dump("Running Max");

            numbers.OnNext(1);
            numbers.OnNext(10);

            numbers.OnNext(3);
            numbers.OnCompleted();
        }

        private static void Scan()
        {
            var numbers = new Subject<int>();
            //Calculate the sum as the number posted to the original sequence
            IObservable<int> scan = numbers.Scan(0, (sum, currentVal) => sum + currentVal);

            numbers.Dump("Numbers");

            scan.Dump("Scans");

            numbers.OnNext(1);  //1

            numbers.OnNext(10);  //11

            numbers.OnNext(999); //1010
            numbers.OnCompleted();
        }

        private static void UseAggregate()
        {
            var numbers = new Subject<int>();

            IObservable<int> mins= numbers.XMin();

            mins.Subscribe(it => Console.WriteLine("Min is {0}", it),
                           exception=>Console.WriteLine("Error: " + exception.Message),
                            () => Console.WriteLine("Min completes"));

            numbers.XMax().Dump("Max");
            numbers.OnNext(10);
            numbers.OnNext(1000);
            numbers.OnCompleted();
            


        }

        private static void UseFirst()
        {
            Console.WriteLine(DateTime.Now.ToLongTimeString());
            //var interval = Observable.Interval(TimeSpan.FromSeconds(3));
            var subject = new Subject<int>();
            subject.OnCompleted();

            //Wait for the first element to arrive, if nothing arrives, nothing will happen
            subject.FirstAsync().Subscribe(it => Console.WriteLine(DateTime.Now.ToLongTimeString() + ":" + it));
            
            Console.WriteLine("Hong Hoi Hoi");
        }
    }
}