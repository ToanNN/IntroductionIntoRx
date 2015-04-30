using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveBasics.TamingSequences
{
    public class AdvanceErrorHandling
    {
        public static void Run()
        {
            //SwallowExceptions();

            HandlingException();
        }

        private static void HandlingException()
        {
            var source = new Subject<int>();

           IObservable<int> final= source.Finally(() => Console.WriteLine("Actually"));

            //var result = source.Catch<int, TimeoutException>(ex => Observable.Return(-1));  //Return -1 whenever a timeout exception happens

            //result.Dump("Handled");

            final.Dump("Finally");
            source.OnNext(10);

            //source.OnError(new TimeoutException("Timed out"));
            source.OnNext(20);

            source.OnCompleted();
        }

        private static void SwallowExceptions()
        {
            var source = new Subject<int>();

            source.Catch(Observable.Empty<int>()).Dump("Catch you");


            source.OnNext(1);

            source.OnNext(10);

            source.OnError(new Exception("error"));

            

            
        }
    }
}