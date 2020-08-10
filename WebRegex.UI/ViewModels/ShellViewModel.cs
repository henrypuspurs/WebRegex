using Caliburn.Micro;
using WebRegex.Core.Models;
using WebRegex.Data;
using WebRegex.UI.Core;

namespace WebRegex.UI.ViewModels
{
    class ShellViewModel : Conductor<object>
    {
        private BindableCollection<Profile> _profiles;
        private Profile _selectedProfile;

        public ShellViewModel()
        {
            Profiles = new DataHandling().ListToBindableCollection(new ProfileSQLData().GetProfiles(Helper.CnnVal("WebRegexDB")));
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
                NotifyOfPropertyChange(() => SelectedProfile);
                if (SelectedProfile != null)
                {
                    LoadProfile();
                }
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
            SelectedProfile = newProfile;
        }

        public void LoadProfile()
        {
            ActivateItemAsync(new ProfileEditViewModel(SelectedProfile.Id));
        }
    }
}
