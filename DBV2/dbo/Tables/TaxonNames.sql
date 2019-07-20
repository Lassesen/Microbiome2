CREATE TABLE [dbo].[TaxonNames] (
    [TaxonName] VARCHAR (255) NOT NULL,
    [Taxon]     INT           NOT NULL,
    CONSTRAINT [PK_TaxonName] PRIMARY KEY CLUSTERED ([TaxonName] ASC, [Taxon] ASC),
    CONSTRAINT [FK_TaxonName_TaxonHierarchy] FOREIGN KEY ([Taxon]) REFERENCES [dbo].[TaxonHierarchy] ([Taxon])
);

