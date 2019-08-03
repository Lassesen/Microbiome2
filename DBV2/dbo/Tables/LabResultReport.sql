CREATE TABLE [dbo].[LabResultReport] (
    [LabResultsId] INT NOT NULL,
    [ReportId]     INT NOT NULL,
    CONSTRAINT [PK_LabResultReport] PRIMARY KEY CLUSTERED ([LabResultsId] ASC, [ReportId] ASC),
    CONSTRAINT [FK_LabResultReport_LabResults] FOREIGN KEY ([LabResultsId]) REFERENCES [dbo].[LabResults] ([LabResultsId]),
    CONSTRAINT [FK_LabResultReport_OwnerReport] FOREIGN KEY ([ReportId]) REFERENCES [dbo].[OwnerReport] ([ReportId])
);

