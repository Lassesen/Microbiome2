CREATE TABLE [dbo].[LabResultTaxon] (
    [LabResultsId]   INT        NOT NULL,
    [Taxon]          INT        NOT NULL,
    [BaseOneMillion] FLOAT (53) NOT NULL,
    [Count]          INT        NULL,
    [Count_Norm]     INT        NULL,
    CONSTRAINT [PK_LabResultTaxon] PRIMARY KEY CLUSTERED ([LabResultsId] ASC, [Taxon] ASC),
    CONSTRAINT [FK_LabResultTaxon_LabResults] FOREIGN KEY ([LabResultsId]) REFERENCES [dbo].[LabResults] ([LabResultsId]),
    CONSTRAINT [FK_LabResultTaxon_TaxonHierarchy] FOREIGN KEY ([Taxon]) REFERENCES [dbo].[TaxonHierarchy] ([Taxon])
);

