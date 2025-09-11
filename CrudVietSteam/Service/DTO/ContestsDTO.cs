using CrudVietSteam.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CrudVietSteam.Service.DTO
{
    public class ContestsDTO : ViewModelBase
    {
        #region
        private bool _isChecked;
        public bool IsCheckedCT
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                RaisePropertyChange(nameof(IsCheckedCT));
            }
        }
        private string _name;
        public string name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    RaisePropertyChange(nameof(name));
                }
            }
        }

        private string _introduce;
        public string introduce
        {
            get => _introduce;
            set
            {
                _introduce = value;
                RaisePropertyChange(nameof(introduce));
            }
        }

        private string _rule;
        public string rule
        {
            get => _rule;
            set
            {
                _rule = value;
                RaisePropertyChange(nameof(rule));
            }
        }

        private string _guide;
        public string guide
        {
            get => _guide;
            set
            {
                _guide = value;
                RaisePropertyChange(nameof(guide));
            }
        }

        private int _fromGrade;
        public int fromGrade
        {
            get => _fromGrade;
            set
            {
                _fromGrade = value;
                RaisePropertyChange(nameof(fromGrade));
            }
        }

        private int _toGrade;
        public int toGrade
        {
            get => _toGrade;
            set
            {
                _toGrade = value;
                RaisePropertyChange(nameof(toGrade));
            }
        }

        private string _status;
        public string status
        {
            get => _status;
            set
            {
                _status = value;
                RaisePropertyChange(nameof(status));
            }
        }

        private string _description;
        public string description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChange(nameof(description));
            }
        }

        private string _title;
        public string title
        {
            get => _title;
            set
            {
                _title = value;
                RaisePropertyChange(nameof(title));
            }
        }

        private string _keywords;
        public string keywords
        {
            get => _keywords;
            set
            {
                _keywords = value;
                RaisePropertyChange(nameof(keywords));
            }
        }

        private int? _id;
        public int? id
        {
            get => _id;
            set
            {
                _id = value;
                RaisePropertyChange(nameof(id));
            }
        }

        private string _accountId;
        public string accountId
        {
            get => _accountId;
            set
            {
                _accountId = value;
                RaisePropertyChange(nameof(accountId));
            }
        }

        private int _cityId;
        public int cityId
        {
            get => _cityId;
            set
            {
                _cityId = value;
                RaisePropertyChange(nameof(cityId));
            }
        }

        private DateTime? _createdAt;
        public DateTime? createdAt
        {
            get => _createdAt;
            set
            {
                _createdAt = value;
                RaisePropertyChange(nameof(createdAt));
            }
        }

        private DateTime? _updatedAt;
        public DateTime? updatedAt
        {
            get => _updatedAt;
            set
            {
                _updatedAt = value;
                RaisePropertyChange(nameof(updatedAt));
            }
        }
    }
    #endregion
    public class Item
    {
        public int count { get; set; }
    }
    public class ContestSearch
    {
        public string KeyWord { get; set; }
        public DateTime? CreatedAtForm { get; set; }
        public DateTime? CreatedAtTo { get; set; }
    }
}
