CREATE TYPE [dbo].[dt_LabTestCoreStatistics] AS TABLE (
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
    [Count]             FLOAT (53) NOT NULL);

