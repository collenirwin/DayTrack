using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DayTrack.ViewModels
{
    /// <summary>
    /// Base class for all view models.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private bool _isLoading = false;

        /// <summary>
        /// The event fired upon a property change.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Is this view model in a loading state?
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => SetAndRaiseIfChanged(ref _isLoading, value);
        }

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event with the given property name.
        /// </summary>
        /// <param name="propName">Name of the property that changed.</param>
        protected void RaiseChange(string propName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// Assigns to the given backing field and calls <see cref="RaiseChange"/>
        /// with the name of the calling property,
        /// but only if the new value differs from the old.
        /// </summary>
        /// <typeparam name="T">Type of the property.</typeparam>
        /// <param name="field">Reference to the backing field to set.</param>
        /// <param name="newValue">New value for the property.</param>
        /// <param name="propName">Automatically assigned property name.</param>
        protected void SetAndRaiseIfChanged<T>(ref T field, T newValue, [CallerMemberName] string propName = "")
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                RaiseChange(propName);
            }
        }
    }
}
