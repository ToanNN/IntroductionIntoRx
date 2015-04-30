using System;

namespace ReactiveBasics.TamingSequences
{
    public class MyEventArgs:EventArgs
    {
        public long Value { get; private set; }

        public MyEventArgs(long l)
        {
            Value = l;
        }

    }
}