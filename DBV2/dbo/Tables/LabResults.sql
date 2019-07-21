CREATE TABLE [dbo].[LabResults] (
    [LabResultsId] INT            IDENTITY (1, 1) NOT NULL,
    [SampleDate]   DATE           NOT NULL,
    [OwnerId]      INT            NOT NULL,
    [LabTestId]    INT            NOT NULL,
    [OtherNotes]   NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_LabResults] PRIMARY KEY CLUSTERED ([LabResultsId] ASC),
    CONSTRAINT [FK_LabResults_LabTests] FOREIGN KEY ([LabTestId]) REFERENCES [dbo].[LabTests] ([LabTestId]),
    CONSTRAINT [FK_LabResults_Owner] FOREIGN KEY ([OwnerId]) REFERENCES [dbo].[Owner] ([OwnerId])
);



