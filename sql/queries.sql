-- Top 3 products by ordered quantity
SELECT
    p."Id" AS ProductId,
    p."Name" AS ProductName,
    SUM(oi."Quantity") AS TotalOrderedQuantity
FROM "Products" p
JOIN "OrderItems" oi ON oi."ProductId" = p."Id"
GROUP BY p."Id", p."Name"
ORDER BY TotalOrderedQuantity DESC
LIMIT 3;

-- Orders containing at least one hazardous productSELECT DISTINCT
    o."Id" AS OrderId,
    o."OrderDate",
    c."Name" AS CustomerName
FROM "Orders" o
JOIN "OrderItems" oi ON oi."OrderId" = o."Id"
JOIN "Products" p ON p."Id" = oi."ProductId"
JOIN "Customers" c ON c."Id" = o."CustomerId"
WHERE p."IsHazardous" = 1;