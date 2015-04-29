using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace ReactiveBasics
{
    public static class MyObservableExtensions
    {
        public static IObservable<T> Empty<T>()
        {
            IObservable<T> result = Observable.Create<T>((IObserver<T> observer) =>
            {
                observer.OnCompleted();
                return Disposable.Empty;
            });

            return result;
        }


        public static IObservable<T> Return<T>(T val)
        {
            IObservable<T> result = Observable.Create((IObserver<T> observer) =>
            {
                observer.OnNext(val);
                observer.OnCompleted();
                return Disposable.Empty;
            });

            return result;
        }

        public static IObservable<T> Never<T>()
        {
            IObservable<T> result = Observable.Create<T>((IObserver<T> observer) => Disposable.Empty);
            return result;
        }

        public static IObservable<T> Throw<T>(T exception) where T : Exception
        {
            IObservable<T> result = Observable.Create<T>((IObserver<T> observer) =>
            {
                observer.OnError(exception);
                return Disposable.Empty;
            });

            return result;
        }


        public static IObservable<int> Range(int start, int count)
        {
            var result = Observable.Generate(start, //seed value
                val => val <= (start + count - 1), //Condition True to continue
                val => val + 1, //the function that transform the current value to the next val
                val => val); //transform the input to the output 

            return result;
        }

        public static IObservable<long> Interval(TimeSpan interval)
        {
            IObservable<long> result = Observable.Generate(0l,
                val => true,
                val => val + 1,
                val => val,
                val => interval);
            return result;
        }

        public static IObservable<long> Timer(TimeSpan dueTime, TimeSpan interval)
        {
            IObservable<long> result = Observable.Generate(0l,
               val => true,
               val => val + 1,
               val => val,
               val => val == 0 ? dueTime : interval);
            return result;
        }


        public static IObservable<bool> Any<T>(this IObservable<T> sequence)
        {
            IObservable<bool> result = Observable.Create<bool>((IObserver<bool> ob) =>
            {

                var hasValues = false;

                var res = sequence.Take(1)
                     .Subscribe(
                    //Has 1 element 
                         _ => hasValues = true,
                    //Error with the one element sequence
                         ob.OnError,
                    //One element sequence completed
                         () =>
                         {
                             ob.OnNext(hasValues);
                             ob.OnCompleted();
                         });

                return res;

            });

            return result;
        }

        public static IObservable<bool> Any<T>(this IObservable<T> source, Func<T, bool> predicate)
        {
            return Any(source.Where(predicate));

        }

        public static void Dump<T>(this IObservable<T> source, string name)
        {
            source.Subscribe(
                    i=>Console.WriteLine("{0}---->{1}",name,i),
                    ex=>Console.WriteLine("{0} failed --->{1}",name,ex.Message),
                    ()=>Console.WriteLine("{0} Completed",name)
                );
        }

        public static IObservable<T> XMin<T>(this IObservable<T> sequence) where T:IComparable<T>
        {
            IObservable<T> result = sequence.Aggregate(
                                        (min, currentVal) => currentVal.CompareTo(min) == -1 ? currentVal : min);
            return result;
        }

        public static IObservable<T> XMax<T>(this IObservable<T> sequence) where T : IComparable<T>
        {
            IObservable<T> result = sequence.Aggregate(
                        (max, currentVal) => currentVal.CompareTo(max) == 1 ? currentVal : max);

            return result;
        }

        public static IObservable<T> RunningMin<T>(this IObservable<T> sequence) where T : IComparable<T>
        {
            var comparer = Comparer<T>.Default;
            Func<T, T, T> minOf = (x, y) => comparer.Compare(x, y) < 0 ? x : y;

            return sequence.Scan(minOf).Distinct();
        }

        public static IObservable<T> RunningMax<T>(this IObservable<T> sequence) where T : IComparable<T>
        {
            var comparer = Comparer<T>.Default;
            Func<T, T, T> maxOf = (x, y) => comparer.Compare(x, y) > 0 ? x : y;

            IObservable<T> result = sequence.Scan(maxOf).Distinct();

            return result;
        }

        public static IObservable<T> Where<T>(this IObservable<T> sequence, Predicate<T> filter)
        {
            IObservable<T> result = sequence.SelectMany<T,T>(it => filter(it) ? Observable.Return(it) : Observable.Empty<T>());
            return result;
        }

        public static IObservable<TResult> Select<T, TResult>(this IObservable<T> sequence, Func<T, TResult> selector)
        {
            IObservable<TResult> result = sequence.SelectMany<T, TResult>(it => Observable.Return(selector(it)));
            return result;
        }
    }





}