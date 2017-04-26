using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuuto.Common.Controls;
using Windows.UI.Xaml;

namespace Tuuto.Common.Behaviors
{
    class ShowTootDialog : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            new TootDialog().ShowAsync();
            return null;
        }
    }
}
