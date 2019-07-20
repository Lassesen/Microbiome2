CREATE TABLE [dbo].[Statistics] (
    [StatisticsId]   INT          IDENTITY (1, 1) NOT NULL,
    [StatisticsName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Statistics] PRIMARY KEY CLUSTERED ([StatisticsId] ASC)
);

