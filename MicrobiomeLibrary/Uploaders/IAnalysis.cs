using System;

namespace MicrobiomeLibrary.Uploaders
{
    interface IAnalysis
    {
        DateTime SampleDateTime { get; set; }
        string SampleId { get; set; }
        string LabName { get; }
        string LabTestName { get; }
        int SaveToDatabase(string ownerEmail);
    }
}