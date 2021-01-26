using BookLibrary.ViewModels.Sorting.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookLibrary.ViewModels.Sorting
{
    public class BooksSortViewModel
    {
        public SortEnum TitleSort { get; private set; }
        public SortEnum YearSort { get; private set; }
        public SortEnum AuthorNameSort { get; private set; }
        public SortEnum AuthorSurnameSort { get; private set; }
        public SortEnum RateSort { get; private set; }
        public SortEnum Current { get; private set; }
        public bool Up { get; set; }
        public BooksSortViewModel(SortEnum sortOrder)
        {
            TitleSort = sortOrder == SortEnum.TITLE_ASC ? SortEnum.TITLE_DESC : SortEnum.TITLE_ASC;
            YearSort = sortOrder == SortEnum.YEAR_ASC ? SortEnum.YEAR_DESC : SortEnum.YEAR_ASC;
            AuthorNameSort = sortOrder == SortEnum.AUTHOR_NAME_ASC ? SortEnum.AUTHOR_NAME_DESC : SortEnum.AUTHOR_NAME_ASC;
            AuthorSurnameSort = sortOrder == SortEnum.AUTHOR_SURNAME_ASC ? SortEnum.AUTHOR_SURNAME_DESC : SortEnum.AUTHOR_SURNAME_ASC;
            RateSort = sortOrder == SortEnum.RATE_ASC ? SortEnum.RATE_DESC : SortEnum.RATE_ASC;

            Up = true;

            if (sortOrder == SortEnum.TITLE_DESC 
                || sortOrder == SortEnum.YEAR_DESC 
                || sortOrder == SortEnum.AUTHOR_NAME_DESC 
                || sortOrder == SortEnum.AUTHOR_SURNAME_DESC 
                || sortOrder == SortEnum.RATE_DESC)
            {
                Up = false;
            }
            Current = sortOrder;
        }
    }
}
