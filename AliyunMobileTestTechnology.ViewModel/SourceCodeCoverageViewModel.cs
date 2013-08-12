using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using AliyunMobileTestTechnology.Model;
using System.Windows.Input;
using AliyunMobileTestTechnology.ViewModel.Commands;

namespace AliyunMobileTestTechnology.ViewModel
{
    public class SourceCodeCoverageViewModel
    {
        ObservableCollection<SourceCodeCoverageLineViewModel> sourceCode = new ObservableCollection<SourceCodeCoverageLineViewModel>();
        public ObservableCollection<SourceCodeCoverageLineViewModel> SourceCode 
        {
            set 
            {
                sourceCode = value;
            }
            get 
            {
                return sourceCode;
            }
        }
    }
}
