using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveBasics.TamingSequences
{
    public class SideEffects
    {
        public static void Run()
        {
            //IncorrectClosure();

            //ComposingDataInPipeline();

            //UsingScan();

            UsingDo();

        }

        private static void UsingDo()
        {
            var source = Observable.Range(1, 5);
            IObservable<int> result =source.Do(i => MyObservableExtensions.Log(i),
                ex => MyObservableExtensions.Log(ex),
                MyObservableExtensions.LogComplete);

            result.Dump("Do Result");


        }

        private static void UsingScan()
        {
            IObservable<int> source = Observable.Range(0, 3);

            IndexedLetter seed = new IndexedLetter()
            {
                Index = -1,
                Letter = new char()
            };
            IObservable<IndexedLetter> indexedCharacters = source.Scan<int,IndexedLetter>(seed,   //this is the starting value of the accumulator

                (accumulator, currentVal) => new IndexedLetter()   //This will be the value of the accumulator for the next operation
                {
                    Index = accumulator.Index +1,
                    Letter = (char)(currentVal + 65) 
                });


            PrintOut(indexedCharacters);


        }

        private static void ComposingDataInPipeline()
        {
            IObservable<char> letters = Observable.Range(0, 3).Select(i => (char)(i + 65));

            var indexedCharacters = letters.Select((ch, ind) => new IndexedLetter
            {
                Index = ind,
                Letter = ch
            });

            PrintOut(indexedCharacters);
        }

        private static void PrintOut(IObservable<IndexedLetter> indexedCharacters)
        {
            indexedCharacters.Subscribe(l => { Console.WriteLine("1 Received {0} at index  {1}", l.Letter, l.Index); },
                () => Console.WriteLine("First Completed")); //print 0 1 2

            indexedCharacters.Subscribe(l => { Console.WriteLine("2 Received {0} at index  {1}", l.Letter, l.Index); },
                () => Console.WriteLine("Second Completed")); //print 3 4 5
        }

        private static void IncorrectClosure()
        {
            IObservable<char> letters = Observable.Range(0, 3).Select(i => (char) (i + 65));

            var index = -1;
            IObservable<char> increaseIndex = letters.Select(l =>
            {
                index ++;
                return l;
            });

            increaseIndex.Subscribe(l =>
            {
                Console.WriteLine("1 Received {0} at index  {1}", l, index);   
            }, () => Console.WriteLine("First Completed"));   //print 0 1 2

            increaseIndex.Subscribe(l =>
            {
                Console.WriteLine("2 Received {0} at index  {1}", l, index);    
            }, () => Console.WriteLine("Second Completed"));   //print 3 4 5
        }
    }

    public struct IndexedLetter
    {
        public int Index { get; set; }

        public char Letter { get; set; }
    }
}
