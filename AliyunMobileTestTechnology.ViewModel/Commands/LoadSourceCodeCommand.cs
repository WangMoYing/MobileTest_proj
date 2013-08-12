using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.IO;
using System.Linq;
using AliyunMobileTestTechnology.Model;

namespace AliyunMobileTestTechnology.ViewModel.Commands
{
    public class LoadSourceCodeCommand:ICommand
    {
        private CoverageViewModel coverageViewModel;

        public LoadSourceCodeCommand(CoverageViewModel coverageViewModel)
        {
            this.coverageViewModel = coverageViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            string testOnly=@"D:\SwingSet2\SwingSet2\src\";
            CoverageRowViewModel rowViewModel = parameter as CoverageRowViewModel;
            if (rowViewModel != null) 
            {
                while (!rowViewModel.IsSrcFileModel&&rowViewModel.ParentRow!=null) 
                {
                    rowViewModel = rowViewModel.ParentRow;
                }
                if (rowViewModel.IsSrcFileModel)
                {
                    int lineCount = 1;
                    try
                    {
                        using (StreamReader reader = new StreamReader(testOnly + rowViewModel.SrcFileName))
                        {
                            SourceCodeCoverageViewModel sourceCodeViewModel = new SourceCodeCoverageViewModel();
                            Dictionary<int, bool> covered = rowViewModel.CoverageStates;
                            int[] lines = rowViewModel.Lines;
                            while (!reader.EndOfStream)
                            {
                                string line = reader.ReadLine();
                                SourceCodeLineModel lineModel = new SourceCodeLineModel();
                                lineModel.Line = line;
                                SourceCodeCoverageLineViewModel lineViewModel = new SourceCodeCoverageLineViewModel(lineModel);
                                sourceCodeViewModel.SourceCode.Add(lineViewModel);
                                if (covered.ContainsKey(lineCount))
                                {
                                    lineModel.CoverageType = covered[lineCount] ? SourceCodeLineModel.LineType.FullyCovered : SourceCodeLineModel.LineType.PartiallyCovered;
                                }
                                else if (lines.Contains<int>(lineCount))
                                {
                                    lineModel.CoverageType = SourceCodeLineModel.LineType.NotCovered;
                                }
                                else 
                                {
                                    lineModel.CoverageType = SourceCodeLineModel.LineType.CommonLine;
                                }
                                lineCount++;
                            }
                            coverageViewModel.SourceCodeCoverageVM = sourceCodeViewModel;
                        }
                    }
                    catch (DirectoryNotFoundException de) 
                    {
                        coverageViewModel.RaiseFileNotFound(testOnly);
                    }
                    catch (FileNotFoundException fe) 
                    {
                        coverageViewModel.RaiseFileNotFound(testOnly + rowViewModel.SrcFileName);
                    }
                }
            }
        }
    }
}
