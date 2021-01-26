using BookLibrary.ViewModels.Sorting.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.ViewModels.Sorting
{
    public class UsersSortViewModel
    {
        public SortEnum EmailSort { get; private set; }
        public SortEnum LoginNameSort { get; private set; }
        public SortEnum Current { get; private set; }
        public bool Up { get; set; }
        public UsersSortViewModel(SortEnum sortOrder)
        {
            EmailSort = sortOrder == SortEnum.EMAIL_ASC ? SortEnum.EMAIL_DESC : SortEnum.EMAIL_ASC;
            LoginNameSort = sortOrder == SortEnum.LOGINNAME_ASC ? SortEnum.LOGINNAME_DESC : SortEnum.LOGINNAME_ASC;

            Up = true;

            if (sortOrder == SortEnum.EMAIL_DESC || sortOrder == SortEnum.LOGINNAME_DESC)
            {
                Up = false;
            }
            Current = sortOrder;
        }
    }
}
