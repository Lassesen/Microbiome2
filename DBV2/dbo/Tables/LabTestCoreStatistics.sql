CREATE TABLE [dbo].[LabTestCoreStatistics] (
    [LabTestId]         INT        NOT NULL,
    [Taxon]             INT        NOT NULL,
    [Mean]              FLOAT (53) NOT NULL,
    [Median]            FLOAT (53) NOT NULL,
    [Mode]              FLOAT (53) NOT NULL,
    [Minimum]           FLOAT (53) NOT NULL,
    [Maximum]           FLOAT (53) NOT NULL,
    [StandardDeviation] FLOAT (53) NOT NULL,
    [Variance]          FLOAT (53) NOT NULL,
    [Skewness]          FLOAT (53) NOT NULL,
    [Kurtosis]          FLOAT (53) NOT NULL,
    [Count]             FLOAT (53) NOT NULL,
    [LastUpdated]       DATETIME   NOT NULL,
    CONSTRAINT [PK_LabTestCoreStatistics] PRIMARY KEY CLUSTERED ([LabTestId] ASC, [Taxon] ASC),
    CONSTRAINT [FK_LabTestCoreStatistics_LabTests] FOREIGN KEY ([LabTestId]) REFERENCES [dbo].[LabTests] ([LabTestId]),
    CONSTRAINT [FK_LabTestCoreStatistics_TaxonHierarchy] FOREIGN KEY ([Taxon]) REFERENCES [dbo].[TaxonHierarchy] ([Taxon])
);

