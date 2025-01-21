-- Tabela "Sales"
CREATE TABLE "Sales" (
    "Id" SERIAL PRIMARY KEY,                 -- Chave primária com auto incremento
    "SalesDate" TIMESTAMP NOT NULL,          -- Data da venda
    "CustomerId" INT NOT NULL,               -- Id do cliente
    "TotalAmount" DECIMAL(18,2) NOT NULL,    -- Valor total da venda
    "Branch" VARCHAR(255) NOT NULL,          -- Filial
    "IsCancelled" BOOLEAN NOT NULL           -- Flag indicando se a venda foi cancelada
);

-- Tabela "Items"
CREATE TABLE "Items" (
    "Id" SERIAL PRIMARY KEY,                 -- Chave primária com auto incremento
    "ItemId" INT NOT NULL,                   -- Id do item
    "SalesId" INT NOT NULL,                  -- Chave estrangeira para "Sales"
    "Quantity" INT NOT NULL,                 -- Quantidade do item
    "TotalAmount" DECIMAL(18,2) NOT NULL,    -- Valor total do item
    "UnitPrice" DECIMAL(18,2) NOT NULL,      -- Preço unitário
    "Discount" DECIMAL(18,2) DEFAULT 0.00,   -- Desconto aplicado
    "IsCancelled" BOOLEAN NOT NULL,          -- Flag indicando se o item foi cancelado
    CONSTRAINT "FK_Items_SalesId" FOREIGN KEY ("SalesId") REFERENCES "Sales"("Id") ON DELETE CASCADE,  -- Relacionamento com a tabela "Sales"
    CONSTRAINT "UQ_Items_ItemId_SalesId" UNIQUE ("ItemId", "SalesId")  -- Restrição de unicidade para "ItemId" + "SalesId"
);
