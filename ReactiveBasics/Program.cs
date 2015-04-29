﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveBasics.Creation;

namespace ReactiveBasics
{
    class Program
    {
        static void Main(string[] args)
        {
            //CreationalMethods.Run();
            //FunctionalUnfolds.Run();
            //TransitioningIntoObservables.Run();
            //ReducingSequence.Run();
            //InspectingElements.Run();
            //FunctionalFolds.Run();

            TransformingSequences.Run();
            Console.ReadLine();
        }
    }
}