CREATE TABLE [dbo].[ProbioticTaxons] (
    [ProbioticProductId] INT        NOT NULL,
    [Taxon]              INT        NOT NULL,
    [BCFU]               FLOAT (53) NULL,
    [LabelRank]          INT        NULL,
    CONSTRAINT [PK_ProbioticTaxon] PRIMARY KEY CLUSTERED ([ProbioticProductId] ASC, [Taxon] ASC),
    CONSTRAINT [FK_ProbioticTaxon_ProbioticProduct] FOREIGN KEY ([ProbioticProductId]) REFERENCES [dbo].[ProbioticProducts] ([ProbioticProductId]),
    CONSTRAINT [FK_ProbioticTaxon_TaxonHierarchy] FOREIGN KEY ([Taxon]) REFERENCES [dbo].[TaxonHierarchy] ([Taxon])
);

