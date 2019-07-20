CREATE TYPE [dbo].[dt_TaxonHierarchy] AS TABLE (
    [Taxon]       INT          NULL,
    [ParentTaxon] INT          NULL,
    [Rank]        VARCHAR (20) NULL);

