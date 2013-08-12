using System;
using System.Collections.Generic;
using System.Text;
using AliyunMobileTestTechnology.Model;

namespace AliyunMobileTestTechnology.ViewModel
{
    public class SourceCodeCoverageLineViewModel
    {
        private SourceCodeLineModel line;
        
        public string LineColor
        {
            get 
            {
                switch (line.CoverageType)
                { 
                    case SourceCodeLineModel.LineType.FullyCovered:
                        return "lawngreen";
                    case SourceCodeLineModel.LineType.PartiallyCovered:
                        return "orange";
                    case SourceCodeLineModel.LineType.NotCovered:
                        return "red";
                    case SourceCodeLineModel.LineType.CommonLine:
                        return "white";
                    default:
                        return "white";
                }
            }
        }

        public string Line 
        {
            get 
            {
                return line.Line;
            }
        }

        public SourceCodeCoverageLineViewModel(SourceCodeLineModel lineModel) 
        {
            line = lineModel;
        }
    }
}
