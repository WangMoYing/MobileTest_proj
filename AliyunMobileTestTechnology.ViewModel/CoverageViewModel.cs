using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using CoverageTestTechnology.Report;
using AliyunMobileTestTechnology.ViewModel.Commands;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace AliyunMobileTestTechnology.ViewModel
{
    public class CoverageViewModel : INotifyPropertyChanged
    {
        private string coverageResultPath="";
        private ObservableCollection<CoverageRowViewModel> data = new ObservableCollection<CoverageRowViewModel>();
        private SourceCodeCoverageViewModel sourceCode;

        public ObservableCollection<CoverageRowViewModel> Data 
        {
            get { return data; }
            set 
            { 
                data = value;
            }
        }

        public SourceCodeCoverageViewModel SourceCodeCoverageVM 
        {
            set 
            {
                sourceCode = value;
                SourceCodeVMChangedEventHandler handler = SourceCodeVMChanged;
                if (handler != null) 
                {
                    handler.Invoke();
                }
            }
            get 
            {
                return sourceCode;
            }
        }

        public ICommand LoadCoverage 
        {
            get { return new LoadCoverageCommand(this); }
        }

        public ICommand LoadSourceCode
        {
            get { return new LoadSourceCodeCommand(this); }
        }

        public string CoverageResultPath
        {
            set 
            {
                coverageResultPath = value;
                RaisePropertyChanged("CoverageResultPath");
            }
            get
            { 
                return coverageResultPath;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            //得到一个副本以预防线程问题
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void RaiseFileNotFound(string path) 
        {
            FileNotFoundEventHandler handler = OnFileNotFound;
            if (handler != null) 
            {
                handler.Invoke(path);
            }
        }

        public delegate void SourceCodeVMChangedEventHandler();
        public event SourceCodeVMChangedEventHandler SourceCodeVMChanged;
        public delegate void FileNotFoundEventHandler(string path);
        public event FileNotFoundEventHandler OnFileNotFound;
    }
}
