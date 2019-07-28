CREATE TABLE [dbo].[ContinuousReference] (
    [ContinuousId]    INT          IDENTITY (1, 1) NOT NULL,
    [ContinuousName]  VARCHAR (50) NOT NULL,
    [ContinuousUnits] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_ContinuousReference] PRIMARY KEY CLUSTERED ([ContinuousId] ASC)
);

