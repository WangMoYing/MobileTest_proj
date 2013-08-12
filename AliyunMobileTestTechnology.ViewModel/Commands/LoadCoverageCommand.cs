using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.IO;
using CoverageTestTechnology.Executor.CommandExecutor;
using CoverageTestTechnology.Report;
using System.Windows;

namespace AliyunMobileTestTechnology.ViewModel.Commands
{
    class LoadCoverageCommand:ICommand
    {
        private CoverageViewModel coverageViewModel;

        public LoadCoverageCommand(CoverageViewModel viewModel) 
        {
            coverageViewModel = viewModel;
            coverageViewModel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(coverageViewModel_PropertyChanged);
        }

        void coverageViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("CoverageResultPath"))
            {
                CanExecuteChanged.Invoke(this, new EventArgs());
            }
        }

        public bool CanExecute(object parameter)
        {
            if (coverageViewModel.CoverageResultPath.EndsWith(".cttr"))
            {
                return File.Exists(coverageViewModel.CoverageResultPath);
            }
            else 
            {
                return false;
            }
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            coverageViewModel.Data.Clear();
            ReportDataModel data = CTTExecutor.LoadCttFile(coverageViewModel.CoverageResultPath);
            data.View.AddUp();
            foreach (Item packageItem in data.View.Children) 
            {
                CoverageRowViewModel packageRow = InitializePackageRow(packageItem);
                AddRow(packageRow);
                foreach (Item item_l1 in packageItem.Children) 
                {
                    CoverageRowViewModel row_l1 = item_l1 is ClassItem ? InitializeClassRow(item_l1) : InitializeSrcFileRow(item_l1);
                    row_l1.TreeLevel = 1;
                    AddRow(row_l1);
                    foreach (Item item_l2 in item_l1.Children) 
                    {
                        CoverageRowViewModel row_l2 = item_l2 is MethodItem ? row_l2 = InitializeMethodRow(item_l2) : InitializeClassRow(item_l2);
                        row_l2.TreeLevel = 2;
                        AddRow(row_l2);
                        if (item_l2 is ClassItem)
                        {
                            foreach (Item item_l3 in item_l2.Children)
                            {
                                CoverageRowViewModel row_l3 = InitializeMethodRow(item_l3);
                                row_l3.SrcFileName = row_l2.SrcFileName;
                                row_l3.TreeLevel = 3;
                                AddRow(row_l3);
                            }
                        }
                        else 
                        {
                            row_l2.SrcFileName = row_l1.SrcFileName;
                        }
                    }
                }
            }
        }

        private void AddRow(CoverageRowViewModel row) 
        {
            int count = coverageViewModel.Data.Count;
            if (count > 0)
            {
                CoverageRowViewModel parent;
                if (coverageViewModel.Data[count - 1].TreeLevel < row.TreeLevel)
                {
                    parent = coverageViewModel.Data[count - 1];
                }
                else
                {
                    parent = coverageViewModel.Data[count - 1].ParentRow;
                    for (int i = 0; i < coverageViewModel.Data[count - 1].TreeLevel - row.TreeLevel; i++)
                    {
                        parent = parent.ParentRow;
                    }
                }
                row.ParentRow = parent;
                parent.ChildRows.Add(row);
            }
            coverageViewModel.Data.Add(row);
        }

        private void GetCoverage(CoverageRowViewModel row, Item item) 
        {
            row.TotalBlocksCount = item.TotlaBlocksCount;
            row.TotalLinesCount = item.TotalLinesCount;
            row.CoveredBlocksCount = item.CoveredBlocksCount;
            row.CoveredLinesCount = item.CoveredLinesCount;
        }

        private CoverageRowViewModel InitializePackageRow(Item packageItem) 
        {
            CoverageRowViewModel packageRow = new CoverageRowViewModel();
            packageRow.Name = packageItem.Name;
            packageRow.TreeLevel = 0;
            packageRow.IsCollapse = false;
            GetCoverage(packageRow, packageItem);
            return packageRow;
        }

        private CoverageRowViewModel InitializeClassRow(Item classItem) 
        {
            CoverageRowViewModel classRow = new CoverageRowViewModel();
            classRow.Name = classItem.Name;
            classRow.IsCollapse = true;
            ClassItem ci = classItem as ClassItem;
            classRow.SrcFileName = ci.Descriptor.SrcFileName;
            GetCoverage(classRow, classItem);
            return classRow;
        }

        private CoverageRowViewModel InitializeSrcFileRow(Item srcFiletem)
        {
            CoverageRowViewModel srcFileRow = new CoverageRowViewModel();
            srcFileRow.Name = srcFiletem.Name;
            srcFileRow.IsCollapse = false;
            srcFileRow.IsSrcFileModel = true;
            srcFileRow.SrcFileName = ((SrcFileItem)srcFiletem).FullName;
            srcFileRow.CoverageStates = srcFiletem.CoveredLines;
            srcFileRow.Lines = srcFiletem.Lines;
            GetCoverage(srcFileRow, srcFiletem);
            return srcFileRow;
        }

        private CoverageRowViewModel InitializeMethodRow(Item methodItem)
        {
            CoverageRowViewModel methodRow = new CoverageRowViewModel();
            methodRow.Name = methodItem.Name;           
            methodRow.TreeIconVisiblity = Visibility.Collapsed;
            methodRow.RowVisiblity = Visibility.Collapsed;
            MethodItem mi = methodItem as MethodItem;
            GetCoverage(methodRow, methodItem); 
            return methodRow;
        }
    }
}
