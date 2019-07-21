CREATE TABLE [dbo].[LabTestStandards] (
    [LabTestId]          INT          NOT NULL,
    [Taxon]              INT          NOT NULL,
    [LabTestStandardsId] INT          IDENTITY (1, 1) NOT NULL,
    [DisplayOrder]       FLOAT (53)   CONSTRAINT [DF_LabTestStandards_DisplayOrder] DEFAULT (CONVERT([float],getdate())) NOT NULL,
    [DisplayName]        VARCHAR (50) NULL,
    CONSTRAINT [PK_LabTestStandards] PRIMARY KEY CLUSTERED ([LabTestStandardsId] ASC),
    CONSTRAINT [FK_LabTestStandards_LabTests] FOREIGN KEY ([LabTestId]) REFERENCES [dbo].[LabTests] ([LabTestId]),
    CONSTRAINT [FK_LabTestStandards_TaxonHierarchy] FOREIGN KEY ([Taxon]) REFERENCES [dbo].[TaxonHierarchy] ([Taxon])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_LabTestStandards]
    ON [dbo].[LabTestStandards]([LabTestId] ASC, [Taxon] ASC);

