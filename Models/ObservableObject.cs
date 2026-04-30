using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Chess.Models;

/// A base class for objects that need to notify the UI when a property value changes
public partial class ObservableObject : INotifyPropertyChanged
{
    /// Event triggered when a property value on this object is updated
    public event PropertyChangedEventHandler? PropertyChanged;

    /// Notifies the UI and other listeners that a specific property has changed
    /// The [CallerMemberName] attribute automatically provides the name of the calling property
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}