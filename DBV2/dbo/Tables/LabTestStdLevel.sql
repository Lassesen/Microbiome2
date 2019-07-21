CREATE TABLE [dbo].[LabTestStdLevel] (
    [LabTestStandardsId] INT        NOT NULL,
    [LabTestLevelId]     INT        NOT NULL,
    [FromLevelCount]     FLOAT (53) NOT NULL,
    [ToLevelCount]       FLOAT (53) NOT NULL,
    CONSTRAINT [PK_LabTestStdLevel] PRIMARY KEY CLUSTERED ([LabTestStandardsId] ASC, [LabTestLevelId] ASC),
    CONSTRAINT [FK_LabTestStdLevel_LabTestLevel] FOREIGN KEY ([LabTestLevelId]) REFERENCES [dbo].[LabTestLevel] ([LabTestLevelId]),
    CONSTRAINT [FK_LabTestStdLevel_LabTestStandards] FOREIGN KEY ([LabTestStandardsId]) REFERENCES [dbo].[LabTestStandards] ([LabTestStandardsId])
);

