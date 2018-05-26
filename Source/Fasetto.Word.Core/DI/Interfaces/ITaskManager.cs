using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Fasetto.Word.Core
{
    /// <summary>
    /// Handles anything to do with Tasks
    /// </summary>
    public interface ITaskManager
    {
        /// <summary>
        /// Queues the specified work to run on the thread pool and returns a proxy for the
        /// task returned by function.
        /// </summary>
        /// <param name="function">The work to execute asynchronously</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A task that represents a proxy for the task returned by function.</returns>
        /// <exception cref="ArgumentNullException">The function parameter was null.</exception>
        Task Run(Func<Task> function, [CallerMemberName]string origin = "", [CallerFilePath]string filePath = "", [CallerLineNumber]int lineNumber = 0);

        /// <summary>
        /// Queues the specified work to run on the thread pool and returns a proxy for the
        /// Task(TResult) returned by function.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the proxy task.</typeparam>
        /// <param name="function">The work to execute asynchronously</param>
        /// <param name="cancellationToken">A cancellation token that should be used to cancel the work</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A Task(TResult) that represents a proxy for the Task(TResult) returned by function.</returns>
        /// <exception cref="ArgumentNullException">The function parameter was null.</exception>
        /// <exception cref="TaskCanceledException">The task has been canceled.</exception>
        /// <exception cref="ObjectDisposedException">The System.Threading.CancellationTokenSource associated with cancellationToken was disposed.</exception>
        Task<TResult> Run<TResult>(Func<Task<TResult>> function, CancellationToken cancellationToken, [CallerMemberName]string origin = "", [CallerFilePath]string filePath = "", [CallerLineNumber]int lineNumber = 0);

        /// <summary>
        /// Queues the specified work to run on the thread pool and returns a proxy for the
        /// Task(TResult) returned by function.
        /// </summary>
        /// <typeparam name="TResult">The type of the result returned by the proxy task.</typeparam>
        /// <param name="function">The work to execute asynchronously</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A Task(TResult) that represents a proxy for the Task(TResult) returned by function.</returns>
        /// <exception cref="ArgumentNullException">The function parameter was null.</exception>
        Task<TResult> Run<TResult>(Func<Task<TResult>> function, [CallerMemberName]string origin = "", [CallerFilePath]string filePath = "", [CallerLineNumber]int lineNumber = 0);

        /// <summary>
        /// Queues the specified work to run on the thread pool and returns a Task(TResult)
        /// object that represents that work. A cancellation token allows the work to be
        /// canceled.
        /// </summary>
        /// <typeparam name="TResult">The result type of the task.</typeparam>
        /// <param name="function">The work to execute asynchronously</param>
        /// <param name="cancellationToken">A cancellation token that should be used to cancel the work</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A Task(TResult) that represents the work queued to execute in the thread pool.</returns>
        /// <exception cref="ArgumentNullException">The function parameter was null.</exception>
        /// <exception cref="TaskCanceledException">The task has been canceled.</exception>
        /// <exception cref="ObjectDisposedException">The System.Threading.CancellationTokenSource associated with cancellationToken was disposed.</exception>
        Task<TResult> Run<TResult>(Func<TResult> function, CancellationToken cancellationToken, [CallerMemberName]string origin = "", [CallerFilePath]string filePath = "", [CallerLineNumber]int lineNumber = 0);

        /// <summary>
        /// Queues the specified work to run on the thread pool and returns a System.Threading.Tasks.Task`1
        /// object that represents that work.
        /// </summary>
        /// <typeparam name="TResult">The return type of the task.</typeparam>
        /// <param name="function">The work to execute asynchronously.</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A task object that represents the work queued to execute in the thread pool.</returns>
        /// <exception cref="ArgumentNullException">The function parameter was null.</exception>
        Task<TResult> Run<TResult>(Func<TResult> function, [CallerMemberName]string origin = "", [CallerFilePath]string filePath = "", [CallerLineNumber]int lineNumber = 0);

        /// <summary>
        /// Queues the specified work to run on the thread pool and returns a proxy for the
        /// task returned by function.
        /// </summary>
        /// <param name="function">The work to execute asynchronously.</param>
        /// <param name="cancellationToken">A cancellation token that should be used to cancel the work.</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A task that represents a proxy for the task returned by function.</returns>
        /// <exception cref="ArgumentNullException">The function parameter was null.</exception>
        /// <exception cref="TaskCanceledException">The task has been canceled.</exception>
        /// <exception cref="ObjectDisposedException">The System.Threading.CancellationTokenSource associated with cancellationToken was disposed.</exception>
        Task Run(Func<Task> function, CancellationToken cancellationToken, [CallerMemberName]string origin = "", [CallerFilePath]string filePath = "", [CallerLineNumber]int lineNumber = 0);

        /// <summary>
        /// Queues the specified work to run on the thread pool and returns a System.Threading.Tasks.Task
        /// object that represents that work. A cancellation token allows the work to be
        /// canceled.
        /// </summary>
        /// <param name="action">The work to execute asynchronously</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A task that represents the work queued to execute in the thread pool.</returns>
        /// <exception cref="ArgumentNullException">The action parameter was null.</exception>
        /// <exception cref="TaskCanceledException">The task has been canceled.</exception>
        /// <exception cref="ObjectDisposedException">The System.Threading.CancellationTokenSource associated with cancellationToken was disposed.</exception>
        Task Run(Action action, CancellationToken cancellationToken, [CallerMemberName]string origin = "", [CallerFilePath]string filePath = "", [CallerLineNumber]int lineNumber = 0);

        /// <summary>
        /// Queues the specified work to run on the thread pool and returns a System.Threading.Tasks.Task
        /// object that represents that work.
        /// </summary>
        /// <param name="action">The work to execute asynchronously</param>
        /// <param name="origin">The method/function this message was logged in</param>
        /// <param name="filePath">The code filename that this message was logged from</param>
        /// <param name="lineNumber">The line of code in the filename this message was logged from</param>
        /// <returns>A task that represents the work queued to execute in the ThreadPool.</returns>
        /// <exception cref="ArgumentNullException">The action parameter was null.</exception>
        Task Run(Action action, [CallerMemberName]string origin = "", [CallerFilePath]string filePath = "", [CallerLineNumber]int lineNumber = 0);
    }
}
