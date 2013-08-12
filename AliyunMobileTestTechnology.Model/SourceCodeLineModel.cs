using System;
using System.Collections.Generic;
using System.Text;

namespace AliyunMobileTestTechnology.Model
{
    public class SourceCodeLineModel
    {
        public string Line { set; get; }
        public LineType CoverageType { set; get; }
        public enum LineType
        {
            FullyCovered = 0,
            PartiallyCovered = 1,
            NotCovered = 2,
            CommonLine = 3
        }
    }
}
