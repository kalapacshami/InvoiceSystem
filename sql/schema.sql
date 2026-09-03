CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "Customers" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Customers" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Country" TEXT NOT NULL,
    "Address" TEXT NOT NULL
);

CREATE TABLE "Products" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Products" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NOT NULL,
    "Category" TEXT NOT NULL,
    "UnitPrice" TEXT NOT NULL,
    "IsHazardous" INTEGER NOT NULL,
    "IsDiscountEligible" INTEGER NOT NULL
);

CREATE TABLE "Orders" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_Orders" PRIMARY KEY AUTOINCREMENT,
    "OrderDate" TEXT NOT NULL,
    "CustomerId" INTEGER NOT NULL,
    CONSTRAINT "FK_Orders_Customers_CustomerId" FOREIGN KEY ("CustomerId") REFERENCES "Customers" ("Id") ON DELETE CASCADE
);

CREATE TABLE "OrderItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_OrderItems" PRIMARY KEY AUTOINCREMENT,
    "Quantity" INTEGER NOT NULL,
    "UnitPriceAtOrderTime" TEXT NOT NULL,
    "OrderId" INTEGER NOT NULL,
    "ProductId" INTEGER NOT NULL,
    CONSTRAINT "FK_OrderItems_Orders_OrderId" FOREIGN KEY ("OrderId") REFERENCES "Orders" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_OrderItems_Products_ProductId" FOREIGN KEY ("ProductId") REFERENCES "Products" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_OrderItems_OrderId" ON "OrderItems" ("OrderId");

CREATE INDEX "IX_OrderItems_ProductId" ON "OrderItems" ("ProductId");

CREATE INDEX "IX_Orders_CustomerId" ON "Orders" ("CustomerId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260903171909_InitialCreate', '8.0.10');

COMMIT;

