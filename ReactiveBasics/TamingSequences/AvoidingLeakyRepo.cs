using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace ReactiveBasics.TamingSequences
{
    public class AvoidingLeakyRepo
    {
        private readonly ReplaySubject<string> _letters;

        public AvoidingLeakyRepo()
        {
            _letters.OnNext("A");
            _letters.OnNext("B");
            _letters.OnNext("C");
        }

        public IObservable<string> Letters
        {
            get { return _letters.AsObservable(); }
        }
    }
}