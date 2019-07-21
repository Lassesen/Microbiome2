CREATE TABLE [dbo].[ReplacementTaxon] (
    [ReportedTaxon] INT NOT NULL,
    [CorrectTaxon]  INT NOT NULL,
    CONSTRAINT [PK_ReplacementTaxon] PRIMARY KEY CLUSTERED ([ReportedTaxon] ASC),
    CONSTRAINT [FK_ReplacementTaxon_TaxonHierarchy] FOREIGN KEY ([CorrectTaxon]) REFERENCES [dbo].[TaxonHierarchy] ([Taxon])
);

