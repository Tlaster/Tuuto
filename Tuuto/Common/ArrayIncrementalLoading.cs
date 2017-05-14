using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mastodon.Model;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;
using PropertyChanged;
using Windows.Foundation;
using Tuuto.Common.Extensions;

namespace Tuuto.Common
{
    [ImplementPropertyChanged]
    public class ArrayIncrementalLoading<T> : ObservableCollection<T>, ISupportIncrementalLoading
    {
        protected Func<int, Task<ArrayModel<T>>> Source { get; }
        
        private bool _refreshOnLoad;
        private int _maxid;

        public bool IsLoading { get; set; }

        public void OnIsLoadingChanged()
        {
            if (IsLoading) IsError = false;
        }

        public bool HasMoreItems { get; set; } = true;

        public bool IsError { get; set; } = false;

        public void Refresh()
        {
            if (IsLoading)
            {
                _refreshOnLoad = true;
            }
            else
            {
                _maxid = 0;
                Clear();
                HasMoreItems = true;
            }
        }

        public ArrayIncrementalLoading(Func<int, Task<ArrayModel<T>>> source)
        {
            Source = source;
        }
        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count) => LoadMoreTask(count).AsAsyncOperation();

        private async Task<LoadMoreItemsResult> LoadMoreTask(uint count)
        {
            uint resultCount = 0;
            try
            {
                ArrayModel<T> data = null;
                try
                {
                    IsLoading = true;
                    data = await Source(_maxid);
                }
                catch
                {
                    IsError = true;
                }

                if (data != null && data.Result.Any())
                {
                    resultCount = (uint)data.Result.Count();

                    foreach (var item in data.Result)
                    {
                        Add(item);
                    }
                    _maxid = data.MaxId;
                }
                else
                {
                    HasMoreItems = false;
                }
            }
            finally
            {
                IsLoading = false;

                if (_refreshOnLoad)
                {
                    _refreshOnLoad = false;
                    Refresh();
                }
            }

            return new LoadMoreItemsResult { Count = resultCount };
        }

    }
}
