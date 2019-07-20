CREATE TABLE [dbo].[LabTestStatistics] (
    [LabTestId]       INT        NOT NULL,
    [TaxonId]         INT        NOT NULL,
    [StatisticsId]    INT        NOT NULL,
    [StatisticsValue] FLOAT (53) NOT NULL,
    [LastUpdated]     DATETIME   CONSTRAINT [DF_LabTestStatistics_LastUpdated] DEFAULT (getutcdate()) NOT NULL,
    CONSTRAINT [PK_LabTestStatistics] PRIMARY KEY CLUSTERED ([LabTestId] ASC, [TaxonId] ASC, [StatisticsId] ASC),
    CONSTRAINT [FK_LabTestStatistics_LabTests] FOREIGN KEY ([LabTestId]) REFERENCES [dbo].[LabTests] ([LabTestId]),
    CONSTRAINT [FK_LabTestStatistics_Statistics] FOREIGN KEY ([StatisticsId]) REFERENCES [dbo].[Statistics] ([StatisticsId]),
    CONSTRAINT [FK_LabTestStatistics_TaxonHierarchy] FOREIGN KEY ([TaxonId]) REFERENCES [dbo].[TaxonHierarchy] ([Taxon])
);

