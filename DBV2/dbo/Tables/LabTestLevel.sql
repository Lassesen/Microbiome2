CREATE TABLE [dbo].[LabTestLevel] (
    [LabTestLevelId]       INT           IDENTITY (1, 1) NOT NULL,
    [LabtestLevelName]     VARCHAR (50)  NOT NULL,
    [LabTestLevelImageUrl] VARCHAR (255) NULL,
    CONSTRAINT [PK_LabTestLevel] PRIMARY KEY CLUSTERED ([LabTestLevelId] ASC)
);

