using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Maui.Platform.Helper.Tools
{
    /// <summary>
    /// In order to start the database initialization,
    /// avoid blocking execution,
    /// and have the opportunity to catch exceptions,
    /// the sample application uses asynchronous lazy initalization,
    /// represented by the class
    /// <see cref="https://docs.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/data/databases"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        readonly Lazy<Task<T>> instance;

        public AsyncLazy(Func<T> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public AsyncLazy(Func<Task<T>> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return instance.Value.GetAwaiter();
        }
    }
}

