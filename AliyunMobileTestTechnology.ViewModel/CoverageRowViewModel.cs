using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace AliyunMobileTestTechnology.ViewModel
{
    public class CoverageRowViewModel : INotifyPropertyChanged
    {
        private int totalLineCount = 0;
        private int coveredLineCount = 0;
        private int totalBlockCount = 0;
        private int coveredBlockCount = 0;
        private List<CoverageRowViewModel> childRows = new List<CoverageRowViewModel>();
        private Visibility rowVisiblity = Visibility.Visible;
        private bool isCollapse = true;
        private Visibility treeIconVisiblity = Visibility.Visible;
        private Thickness margin;
        private int treeLevel = 0;

        public Dictionary<int,bool> CoverageStates{ set; get; }
        public int[] Lines { set; get; }
        public string SrcFileName { set; get; }
        public string Name { set; get; }
        public bool IsSrcFileModel { set; get; }
        public int TotalLinesCount 
        {
            set
            {
                totalLineCount=value;
                RaisePropertyChanged("TotalLineCount");
            }
            get
            { 
                return totalLineCount;
            } 
        }      
        public int CoveredLinesCount 
        {
            set
            {
                coveredLineCount=value;
                RaisePropertyChanged("CoveredLineCount");
            }
            get
            {
                return coveredLineCount;
            }
        }       
        public int TotalBlocksCount 
        {
            set
            {
                totalBlockCount=value;
                RaisePropertyChanged("TotalBlockCount");
            }
            get
            {
                return totalBlockCount;
            }
        }       
        public int CoveredBlocksCount 
        {
            set
            {
                coveredBlockCount=value;
                RaisePropertyChanged("CoveredBlockCount");
            }
            get
            {
                return coveredBlockCount;
            }
        }
        public double LineCoverageRate { get { return ((double)coveredLineCount / totalLineCount); } }
        public string LineCoverageColor 
        {
            get 
            {
                if (LineCoverageRate < 0.5) 
                {
                    return "red";
                }
                else if (LineCoverageRate < 1)
                {
                    return "orange";
                }
                else 
                {
                    return "green";
                }
            }
        }
        public double BlockCoverageRate { get { return ((double)coveredBlockCount / totalBlockCount); } }
        public string BlockCoverageColor
        {
            get
            {
                if (BlockCoverageRate < 0.5)
                {
                    return "red";
                }
                else if (BlockCoverageRate < 1)
                {
                    return "orange";
                }
                else
                {
                    return "green";
                }
            }
        }
        public CoverageRowViewModel ParentRow { set; get; }      
        public List<CoverageRowViewModel> ChildRows 
        {
            set
            {
                childRows = value;
            }
            get
            {
                return childRows;
            }
        }     
        public Visibility RowVisiblity 
        {
            get 
            {
                return rowVisiblity;
            }
            set
            {
                rowVisiblity = value;
                RaisePropertyChanged("RowVisiblity");
            }
        }       
        public bool IsCollapse
        {
            get
            {
                return isCollapse;
            }
            set
            {
                if (isCollapse != value)
                {
                    isCollapse = value;
                    RaisePropertyChanged("IsCollapse");
                    if (ChildRows != null)
                    {
                        foreach (CoverageRowViewModel row in ChildRows)
                        {
                            if (isCollapse)
                            {
                                row.RowVisiblity = Visibility.Collapsed;
                                row.IsCollapse = isCollapse;
                            }
                            else
                            {
                                row.RowVisiblity = Visibility.Visible;
                            }
                        }
                    }
                }                
            }
        }       
        public Visibility TreeIconVisiblity 
        {
            get { return treeIconVisiblity; }
            set 
            {
                treeIconVisiblity = value;
                RaisePropertyChanged("TreeIconVisiblity");
            }
        }       
        public Thickness Margin
        {
            get { return margin; }
        }        
        public int TreeLevel 
        {
            set
            {
                treeLevel=value;
                margin = new Thickness(15 * value, 0, 0, 0);
            }
            get
            {
               return treeLevel ;
            }
        }
        public Visibility LeafIconVisiblity 
        {
            get 
            {
                if (treeIconVisiblity == Visibility.Visible)
                {
                    return Visibility.Collapsed;
                }
                else 
                {
                    return Visibility.Visible;
                }
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            //得到一个副本以预防线程问题
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
