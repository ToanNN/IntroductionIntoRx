using System;
using System.IO;
using System.Reactive.Subjects;

namespace ReactiveExtensionsInUse
{
    public class LifetimeManagement
    {
        public static void Run()
        {
            ExceptionHandling();
        }

        private static void ExceptionHandling()
        {
            var values = new Subject<int>();

            var fs = values.Subscribe(x => Console.WriteLine("Received value for first Subscription: {0}", x), OnErrorHappens);

            values.Subscribe(x => Console.WriteLine("Second Subscription: {0}", x));

            values.OnNext(10);
            //Remove the subscription. it does not affect the second subscription
            fs.Dispose();

            values.OnNext(20);

            values.OnError(new Exception("Invalid message")); //will throw an unhandled exception for the second subscription
            

        }

        private static void OnErrorHappens(Exception obj)
        {
            Console.WriteLine("Handle Error: {0}",obj.Message);       
        }
    }
}