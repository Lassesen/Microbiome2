CREATE TABLE [dbo].[LabTests] (
    [LabTestId]                 INT           IDENTITY (1, 1) NOT NULL,
    [LabId]                     INT           NOT NULL,
    [LabTestName]               VARCHAR (100) NOT NULL,
    [FullStatisticsComputeDate] DATETIME      NULL,
    CONSTRAINT [PK_LabTests] PRIMARY KEY CLUSTERED ([LabTestId] ASC),
    CONSTRAINT [FK_LabTests_Labs] FOREIGN KEY ([LabId]) REFERENCES [dbo].[Labs] ([LabId])
);

