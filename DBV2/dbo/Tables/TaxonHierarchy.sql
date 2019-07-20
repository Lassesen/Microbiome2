CREATE TABLE [dbo].[TaxonHierarchy] (
    [Taxon]       INT          NOT NULL,
    [ParentTaxon] INT          NOT NULL,
    [Rank]        VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_TaxonHierarchy] PRIMARY KEY CLUSTERED ([Taxon] ASC),
    CONSTRAINT [FK_TaxonHierarchy_TaxonHierarchy] FOREIGN KEY ([ParentTaxon]) REFERENCES [dbo].[TaxonHierarchy] ([Taxon])
);

