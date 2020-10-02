using Caliburn.Micro;
using System.Linq;
using System.Net;
using WebRegex.Core;
using WebRegex.Core.Models;
using WebRegex.Data;
using WebRegex.UI.Core;

namespace WebRegex.UI.ViewModels
{
    class ShellViewModel : Conductor<object>
    {
        private BindableCollection<Profile> _profiles;
        private Profile _selectedProfile;
        private string _pageBody;
        private BindableCollection<Result> _results;
        private BindableCollection<Expression> _expressions;
        private ISqlData _data;

        public ShellViewModel()
        {
            PageBody = "Load HTML to be run";
            _data = new SqlData();
            _data.GetConnectionString(Helper.CnnVal("WebRegexDB"));
            Profiles = DataHandling.ToBindableCollection(_data.SqlQuery<Profile>(@"select * from dbo.Profiles"));
        }

        public string Url { get; set; }
        public string PageBody
        {
            get
            {
                return _pageBody;
            }
            set
            {
                _pageBody = value;
                NotifyOfPropertyChange(() => PageBody);
            }
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

        public BindableCollection<Result> Results
        {
            get
            {
                return _results;
            }
            set
            {
                _results = value;
                NotifyOfPropertyChange(() => Results);
            }
        }


        public Profile SelectedProfile
        {
            get
            {
                return _selectedProfile;
            }
            set
            {
                _selectedProfile = value;
                if (SelectedProfile != null)
                {
                    LoadProfile();
                }
                NotifyOfPropertyChange(() => SelectedProfile);
            }
        }


        public BindableCollection<Profile> Profiles
        {
            get
            {
                return _profiles;
            }
            set
            {
                _profiles = value;
                NotifyOfPropertyChange(() => Profiles);
            }
        }

        public void AddProfile()
        {
            var newProfile = new Profile() { Name = "New Profile" };
            Profiles.Add(newProfile);
            SelectedProfile = Profiles.Where(p => p.Name == "New Profile").FirstOrDefault();
            Expressions.Add(new Expression { Name = "New Expression", Regex = "New Regex", IsIdentifier = true });
        }

        public void LoadResults()
        {
            SelectedProfile.RegexExpressions = Expressions.ToList();
            Results = DataHandling.ToBindableCollection(new ParsePage(Url).GetFirstResult(SelectedProfile, PageBody));
        }

        public void LoadProfile()
        {
            Expressions = DataHandling.ToBindableCollection(_data.SqlQuery<Expression>(@$"select * from dbo.Expressions where ProfileId = {SelectedProfile.Id}"));
        }

        public void AddExpression()
        {
            if (SelectedProfile != null)
            {
                Expressions.Add(new Expression() { Name = "New Expression", Regex = "New Regex", ProfileId = SelectedProfile.Id });
            }
            else
            {
                System.Windows.MessageBox.Show("Please select or create a profile");
            }
        }

        public void RemoveExpression()
        {
            if (Expressions.Count > 0)
            {
                var lastExpression = Expressions.LastOrDefault();
                Expressions.Remove(lastExpression);
            }
        }

        public void ClearIdentifier()
        {
            foreach (Expression expression in Expressions)
            {
                expression.IsIdentifier = false;
            }
        }

        public void LoadPage()
        {
            try
            {
                PageBody = new WebClient().DownloadString(Url);
            }
            catch
            {
                System.Windows.MessageBox.Show("Unable to load page");
            }
        }

        public void SaveProfile()
        {
            if (SelectedProfile.Id == 0)
            {
                SaveNewProfile();
                System.Windows.MessageBox.Show("New Profile Saved");
            }
            else
            {
                SaveExistingProfile();
                System.Windows.MessageBox.Show("Profile Saved");
            }
        }

        public void DeleteProfile()
        {
            _data.SqlExecute(@"delete from dbo.Profiles where Id = @Id; delete from dbo.Expressions where ProfileId = @Id", SelectedProfile);
            Profiles.Remove(SelectedProfile);
            SelectedProfile = null;
            Expressions.Clear();
        }

        public void SaveResult()
        {
            int records = 0;
            foreach (Result result in Results)
            {
                if (result.Regex != "" && result.Regex != "Invalid Regex" && result.Regex != null)
                {
                    _data.SqlExecute(@$"insert into dbo.Results (ProfileId, Origin, Name, Regex, IsIdentifierBit, Identifier) values (@ProfileId, @Origin, @Name, @Regex, @IsIdentifierBit, @Identifier)", result);
                    records++;
                }
            }
            System.Windows.MessageBox.Show($"{records} results saved.");
        }

        private void SaveNewProfile()
        {
            _data.SqlExecute(@"insert into dbo.Profiles values (@Name)", SelectedProfile);
            var Id = _data.SqlQuery<Profile>(@$"select Id from dbo.Profiles where Name = '{SelectedProfile.Name}'").First().Id;
            SelectedProfile.Id = Id;
            foreach (Expression expression in Expressions)
            {
                _data.SqlExecute(@$"insert into dbo.Expressions (ProfileId, Name, Regex, IsIdentifierBit) values ('{Id}', @Name, @Regex, @IsIdentifierBit)", expression);
            }
        }

        private void SaveExistingProfile()
        {
            _data.SqlExecute($"update dbo.Profiles set Name = @Name where Id = @Id", SelectedProfile);
            foreach (Expression expression in Expressions)
            {
                if (expression.Id != 0)
                {
                    _data.SqlExecute($"update dbo.Expressions set Name = @Name where Id = @Id", expression);
                    _data.SqlExecute($"update dbo.Expressions set Regex = @Regex where Id = @Id", expression);
                    _data.SqlExecute($"update dbo.Expressions set IsIdentifierBit = @IsIdentifierBit where Id = @Id", expression);
                }
                else
                {
                    _data.SqlExecute(@$"insert into dbo.Expressions (ProfileId, Name, Regex, IsIdentifierBit) values ('{SelectedProfile.Id}', @Name, @Regex, @IsIdentifierBit)", expression);
                }
            }
            var savedExpressions = _data.SqlQuery<Expression>(@$"select * from dbo.Expressions where ProfileId = '{SelectedProfile.Id}'");
            foreach (Expression expression in Expressions)
            {
                try
                {
                    expression.Id = savedExpressions.Where(n => n.Name == expression.Name).First().Id;
                }
                catch
                {
                    System.Windows.MessageBox.Show("Error Saving Expressions");
                }
            }
            foreach (Expression savedexpression in savedExpressions.Where(x => Expressions.Where(y => y.Id == x.Id).Count() == 0))
            {
                _data.SqlExecute($"delete from dbo.Expressions where Id = @Id", savedexpression);
            }
            LoadProfile();
        }
    }
}
