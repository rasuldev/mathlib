using System;
using System.Linq.Expressions;

namespace mathlib
{
    public delegate T VarFunc<T>(params T[] args);
    /// <summary>
    /// Arguments count of DynFunc can be set in runtime
    /// </summary>
    public class DynFunc<T>
    {
        private readonly Func<T[], T> _func;
        public int ArgsCount { get; }

        public DynFunc(int argsCount, Func<T, T> func)
        {
            _func = args => func(args[0]);
            ArgsCount = argsCount;
        }

        public DynFunc(Func<T, T, T> func)
        {
            _func = args => func(args[0], args[1]);
            ArgsCount = 2;
        }

        public DynFunc(Func<T, T, T, T> func)
        {
            _func = args => func(args[0], args[1], args[2]);
            ArgsCount = 3;
        }

        //public DynFunc(Expression<VarFunc<T>> d)
        //{
        ////    _func = args => func(args[0], args[1], args[2]);
        ////    //ArgsCount = argsCount;
        //}

        public DynFunc(int argsCount, Func<T, T, T, T, T> func)
        {
            _func = args => func(args[0], args[1], args[2], args[3]);
            ArgsCount = argsCount;
        }


        public DynFunc(int argsCount, Func<T[], T> func)
        {
            _func = func;
            ArgsCount = argsCount;
        }

        public T Invoke(params T[] args)
        {
            if (args.Length != ArgsCount)
                throw new ArgumentOutOfRangeException($"Incorrect number of arguments: should be {ArgsCount}, but was {args.Length}");
            return _func(args);
        }

    }
}