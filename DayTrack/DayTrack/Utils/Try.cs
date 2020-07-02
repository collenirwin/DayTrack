using System;
using System.Threading.Tasks;

namespace DayTrack.Utils
{
    /// <summary>
    /// Provides generic try-catch wrapper methods.
    /// </summary>
    public static class Try
    {
        /// <summary>
        /// Tries to run the given action.
        /// </summary>
        /// <param name="action">Action to attempt to run.</param>
        /// <param name="onException">Optional action to run if an <see cref="Exception"/> is thrown.</param>
        /// <returns>true if the action ran successfully.</returns>
        public static bool Run(Action action, Action<Exception> onException = null)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
                return false;
            }
        }

        /// <summary>
        /// Tries to run the given func.
        /// </summary>
        /// <typeparam name="T">Return value type.</typeparam>
        /// <param name="func">Func to attempt to run</param>
        /// <param name="onException">Optional action to run if an <see cref="Exception"/> is thrown.</param>
        /// <returns>The value returned from the passed func, otherwise default for the given type.</returns>
        public static T Run<T>(Func<T> func, Action<Exception> onException = null)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
                return default;
            }
        }

        /// <summary>
        /// Tries to run the given action.
        /// </summary>
        /// <param name="asyncAction">Action to attempt to run.</param>
        /// <param name="onException">Optional action to run if an <see cref="Exception"/> is thrown.</param>
        /// <returns>true if the action ran successfully.</returns>
        public static async Task<bool> RunAsync(Func<Task> asyncAction, Action<Exception> onException = null)
        {
            try
            {
                await asyncAction();
                return true;
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
                return false;
            }
        }

        /// <summary>
        /// Tries to run the given async func.
        /// </summary>
        /// <typeparam name="T">Return value type.</typeparam>
        /// <param name="asyncFunc">Func to attempt to run.</param>
        /// <param name="onException">Optional action to run if an <see cref="Exception"/> is thrown.</param>
        /// <returns>The value returned from the passed func, otherwise default for the given type.</returns>
        public static async Task<T> RunAsync<T>(Func<Task<T>> asyncFunc, Action<Exception> onException = null)
        {
            try
            {
                return await asyncFunc();
            }
            catch (Exception ex)
            {
                onException?.Invoke(ex);
                return default;
            }
        }
    }
}
