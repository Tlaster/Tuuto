using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tuuto.Common
{
    public class ExIncrementalLoadingCollection<TSource, IType> : IncrementalLoadingCollection<TSource, IType> where TSource : IIncrementalSource<IType>
    {
        private bool _isError;

        public bool IsError
        {
            get => _isError;
            set
            {
                _isError = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(IsError)));
            }
        }
        public ExIncrementalLoadingCollection() : base()
        {
            OnStartLoading = () => IsError = false;
            OnError = (e) => IsError = true;
        }

        public ExIncrementalLoadingCollection(TSource source) : base(source)
        {
            OnStartLoading = () => IsError = false;
            OnError = (e) => IsError = true;
        }
    }
}
