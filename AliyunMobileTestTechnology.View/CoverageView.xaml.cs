using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AliyunMobileTestTechnology.ViewModel;

namespace AliyunMobileTestTechnology.View
{
    /// <summary>
    /// CoverageWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CoverageView : UserControl
    {
        private CoverageViewModel coverageViewModel;
        public CoverageView()
        {
            InitializeComponent();
            coverageViewModel = new CoverageViewModel();
            base.DataContext = coverageViewModel;
            coverageViewModel.SourceCodeVMChanged += new CoverageViewModel.SourceCodeVMChangedEventHandler(coverageViewModel_SourceCodeVMChanged);
            coverageViewModel.OnFileNotFound += new CoverageViewModel.FileNotFoundEventHandler(coverageViewModel_OnFileNotFound);
        }

        private void coverageViewModel_OnFileNotFound(string path)
        {
            if (path.EndsWith(".java"))
            {
                MessageBox.Show(string.Format("File not found: {0}", path));
            }
            else 
            {
                MessageBox.Show(string.Format("Directory not found: {0}", path));
            }
        }

        private void coverageViewModel_SourceCodeVMChanged()
        {
            SourceCodeCoverageView sourceCodeView = new SourceCodeCoverageView();
            sourceCodeView.DataContext = ((CoverageViewModel)DataContext).SourceCodeCoverageVM;
            sourceCodeView.ShowDialog();
        }
    }
}
