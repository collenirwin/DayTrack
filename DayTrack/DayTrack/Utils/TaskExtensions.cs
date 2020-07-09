using DayTrack.ViewModels;
using System;
using System.Threading.Tasks;

namespace DayTrack.Utils
{
    /// <summary>
    /// Contains extension methods for <see cref="Task"/> objects.
    /// </summary>
    public static class TaskExtensions
    {
        /// <summary>
        /// Awaits the given task, mutating <see cref="ViewModelBase.IsLoading"/> to reflect the state of the task.
        /// </summary>
        /// <typeparam name="TViewModel">Type of the view model object.</typeparam>
        /// <param name="task">Task to await.</param>
        /// <param name="viewModel">View model running the task.</param>
        /// <exception cref="ArgumentNullException"/>
        public static async Task ExpressLoading<TViewModel>(this Task task, TViewModel viewModel)
            where TViewModel : ViewModelBase
        {
            task = task ?? throw new ArgumentNullException(nameof(task));
            viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            try
            {
                viewModel.IsLoading = true;
                await task;
                
            }
            finally
            {
                viewModel.IsLoading = false;
            }
        }
    }
}
