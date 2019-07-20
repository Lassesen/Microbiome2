CREATE TABLE [dbo].[Labs] (
    [LabId]   INT          IDENTITY (1, 1) NOT NULL,
    [LabName] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_Labs] PRIMARY KEY CLUSTERED ([LabId] ASC)
);

