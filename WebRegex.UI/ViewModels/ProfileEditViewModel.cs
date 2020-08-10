using Caliburn.Micro;
using WebRegex.Core.Models;
using WebRegex.Data;
using WebRegex.UI.Core;

namespace WebRegex.UI.ViewModels
{
    class ProfileEditViewModel : Screen
    {
        private BindableCollection<Expression> _expressions;

        public ProfileEditViewModel(int profileId)
        {
            Expressions = new DataHandling().ListToBindableCollection(new ProfileSQLData().GetExpressions(profileId, Helper.CnnVal("WebRegexDB")));
        }

        public BindableCollection<Expression> Expressions
        {
            get
            {
                return _expressions;
            }
            set
            {
                _expressions = value;
                NotifyOfPropertyChange(() => Expressions);
            }
        }

    }
}
