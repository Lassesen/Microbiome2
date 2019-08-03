CREATE TABLE [dbo].[TaxonModifierCitation] (
    [Taxon]             INT           NOT NULL,
    [CitationId]        INT           NOT NULL,
    [ModifierId]        INT           NOT NULL,
    [DirectionOfImpact] INT           CONSTRAINT [DF_TaxonModifierCitation_DirectionOfImpact] DEFAULT ((0)) NOT NULL,
    [AmountOfImpact]    FLOAT (53)    NOT NULL,
    [UsageInformation]  VARCHAR (MAX) NULL,
    CONSTRAINT [PK_TaxonModifierCitation] PRIMARY KEY CLUSTERED ([Taxon] ASC, [CitationId] ASC, [ModifierId] ASC),
    CONSTRAINT [FK_TaxonModifierCitation_Citation] FOREIGN KEY ([CitationId]) REFERENCES [dbo].[Citation] ([CitationId]),
    CONSTRAINT [FK_TaxonModifierCitation_Modifiers] FOREIGN KEY ([ModifierId]) REFERENCES [dbo].[Modifiers] ([ModifierId]),
    CONSTRAINT [FK_TaxonModifierCitation_TaxonHierarchy] FOREIGN KEY ([Taxon]) REFERENCES [dbo].[TaxonHierarchy] ([Taxon])
);

