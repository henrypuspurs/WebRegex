using Caliburn.Micro;
using WebRegex.Core.Models;

namespace WebRegex.UI.ViewModels
{
    class ResultViewModel : Screen
    {
        public ResultViewModel(BindableCollection<Result> results)
        {
            Results = results;
        }

        public BindableCollection<Result> Results { get; }
    }
}
