-- Script SQL đối chiếu nhanh schema và dữ liệu mẫu TH5 trong PostgreSQL (database: qtkd_db)

-- 1. Kiểm tra danh sách bảng đã tạo bởi EF Core Migration
SELECT table_name
FROM information_schema.tables
WHERE table_schema = 'public'
ORDER BY table_name;

-- 2. Kiểm tra khóa chính (PRIMARY KEY) và khóa ngoại (FOREIGN KEY)
SELECT
    tc.table_name,
    tc.constraint_name,
    tc.constraint_type,
    kcu.column_name,
    ccu.table_name AS foreign_table_name,
    ccu.column_name AS foreign_column_name
FROM information_schema.table_constraints AS tc
JOIN information_schema.key_column_usage AS kcu
    ON tc.constraint_name = kcu.constraint_name
    AND tc.table_schema = kcu.table_schema
LEFT JOIN information_schema.constraint_column_usage AS ccu
    ON ccu.constraint_name = tc.constraint_name
    AND ccu.table_schema = tc.table_schema
WHERE tc.table_schema = 'public'
  AND tc.constraint_type IN ('PRIMARY KEY', 'FOREIGN KEY')
ORDER BY tc.table_name, tc.constraint_type;

-- 3. Kiểm tra dữ liệu mẫu đã seed
SELECT * FROM "Customers" ORDER BY "Id";
SELECT * FROM "Contracts" ORDER BY "Id";
SELECT * FROM "PriceLists" ORDER BY "Id";
SELECT * FROM "PriceItems" ORDER BY "Id";
