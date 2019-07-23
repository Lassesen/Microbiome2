using System;
using System.Data;

namespace UbiomeUpload
{
    interface IAnalysis
    {
        DataTable AsDataTable { get; }
        DateTime SampleDateTime { get; set; }
        string SampleId { get; set; }
        string LabName { get; }
        string LabTestName { get; }
    }
}